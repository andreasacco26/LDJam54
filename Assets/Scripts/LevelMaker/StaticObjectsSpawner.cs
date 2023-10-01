using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticObjectsSpawner: MonoBehaviour {

    public Vector3 objectsRotation;
    public bool isLeft = false;
    public float offset = 0f;
    public GameObject[] itemsToSpawn;

    private Vector3 lastExtents;
    private int layer;
    private Transform lastSpawned;

    void Start() {
        layer = LayerMask.NameToLayer("Obstacle");
        Spawn();
    }

    void Update() {
        if (!lastSpawned ||
            lastSpawned.position.z + lastExtents.x < transform.position.z) {
            Spawn();
        }
    }

    private void Spawn() {
        var item = itemsToSpawn[Random.Range(0, itemsToSpawn.Length - 1)];
        var instantiatedItem = Instantiate(item, transform.position, Quaternion.identity);
        instantiatedItem.layer = layer;
        var boxCollider = instantiatedItem.AddComponent<BoxCollider>();
        boxCollider.size = Vector3.one;
        var extents = instantiatedItem.GetComponent<Renderer>().bounds.extents;
        extents.x += offset;
        boxCollider.center = new(extents.x * (isLeft ? -1.1f : 1.1f), 0, 0);
        var position = transform.position;
        position.x += extents.x * (isLeft ? -1 : 1);
        position.z += extents.z;
        instantiatedItem.transform.position = position;
        instantiatedItem.transform.localEulerAngles = objectsRotation;
        LevelMaker.shared.AddObjectToMove(instantiatedItem.transform);
        lastSpawned = instantiatedItem.transform;
        lastExtents = extents;
    }
}
