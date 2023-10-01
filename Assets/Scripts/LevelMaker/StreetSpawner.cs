using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StreetSpawner: MonoBehaviour {
    public float streetWidth = 4;
    public int numberOfLanes = 4;
    public float speed = 5;
    public float slowmoSpeed = 1;
    public Material streetMaterial;
    public GameObject road;
    public GameObject sidewalk;
    public GameObject roadStripes;
    [HideInInspector]
    public float initialSpeed = 5;

    private GameObject prototype;
    private readonly List<Transform> itemsToMove = new();
    private float streetExtent;
    private int layer;

    void Start() {
        layer = LayerMask.NameToLayer("Obstacle");
        initialSpeed = speed;
        Spawn();
    }

    void Update() {
        if (itemsToMove.Last() == null ||
            itemsToMove.Last().position.z + streetExtent < transform.position.z) {
            Spawn();
        }
        MoveItems();
        CleanItemsToMove();
    }

    private void Spawn() {
        if (!prototype) {
            SetUpPrototype();
        }
        var position = transform.position;
        position.z += streetExtent - 0.05f;
        var instantiatedItem = Instantiate(prototype);
        instantiatedItem.transform.position = position;
        itemsToMove.Add(instantiatedItem.transform);
    }

    private void SetUpPrototype() {
        var sidewalkExtents = sidewalk.GetComponent<Renderer>().bounds.extents;
        prototype = new PlaneMaker {
            width = streetWidth,
            length = sidewalkExtents.z * 2,
            material = streetMaterial
        }.GetPlane();
        var collider = prototype.GetComponent<BoxCollider>();
        collider.size = new Vector3(collider.size.x, collider.size.y, 1);
        collider.center = new Vector3(0, 0, sidewalkExtents.z);
        prototype.layer = layer;
        var rightSidewalk = Instantiate(sidewalk);
        rightSidewalk.transform.parent = prototype.transform;
        rightSidewalk.transform.localPosition = new Vector3(streetWidth * 0.5f, 0, 0);
        rightSidewalk.transform.localEulerAngles = new Vector3(0, 180, 0);

        var leftSidewalk = Instantiate(sidewalk);
        leftSidewalk.transform.parent = prototype.transform;
        leftSidewalk.transform.localPosition = new Vector3(-streetWidth * 0.5f, 0, 0);
        streetExtent = sidewalkExtents.z;
        prototype.transform.position = new Vector3(9999, 9999, 9999);
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
