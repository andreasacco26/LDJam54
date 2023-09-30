using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticObjectsSpawner: MonoBehaviour {

    public Vector3 objectsRotation;
    public bool isLeft = false;
    public float offset = 0f;
    public float speed = 5;
    public float slowmoSpeed = 1;
    public GameObject[] itemsToSpawn;
    [HideInInspector]
    public float initialSpeed = 5;

    private readonly List<Transform> itemsToMove = new();
    private Vector3 lastExtents;
    private int layer;

    void Start() {
        layer = LayerMask.NameToLayer("Obstacle");
        initialSpeed = speed;
        Spawn();
    }

    void Update() {
        if (itemsToMove.Last().position.z + lastExtents.x * 2 < transform.position.z) {
            Spawn();
        }
        MoveItems();
        CleanItemsToMove();
    }

    private void Spawn() {
        var item = itemsToSpawn[Random.Range(0, itemsToSpawn.Length - 1)];
        var instantiatedItem = Instantiate(item, transform.position, Quaternion.identity);
        instantiatedItem.layer = layer;
        var boxCollider = instantiatedItem.AddComponent<BoxCollider>();
        boxCollider.size = Vector3.one;
        var extents = instantiatedItem.GetComponent<MeshFilter>().sharedMesh.bounds.extents;
        extents.x += isLeft ? -offset : offset;
        boxCollider.center = new(extents.x, 0, 0);
        var position = transform.position;
        position.x += extents.x;
        instantiatedItem.transform.position = position;
        instantiatedItem.transform.localEulerAngles = objectsRotation;
        itemsToMove.Add(instantiatedItem.transform);
        lastExtents = extents;
    }

    private void MoveItems() {
        foreach (Transform t in itemsToMove) {
            if (t == null) continue;
            t.Translate(0, 0, -speed * Time.deltaTime, Space.World);
        }
        CleanItemsToMove();
    }

    private void CleanItemsToMove() {
        itemsToMove.Remove(null);
    }
}
