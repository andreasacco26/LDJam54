using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StreetSpawner: MonoBehaviour {
    public float streetWidth = 4;
    public int numberOfLanes = 4;
    public Material streetMaterial;
    public GameObject road;
    public GameObject sidewalk;
    public GameObject roadStripes;
    [HideInInspector]
    public float initialSpeed = 5;

    private GameObject prototype;
    private Transform lastSpawned;
    private float streetExtent;
    private int layer;

    void Start() {
        layer = LayerMask.NameToLayer("Obstacle");
        Spawn();
    }

    void Update() {
        if (!lastSpawned ||
            lastSpawned.position.z + streetExtent - 0.1f < transform.position.z) {
            Spawn();
        }
    }

    private void Spawn() {
        if (!prototype) {
            SetUpPrototype();
        }
        var position = transform.position;
        position.z += streetExtent;
        var instantiatedItem = Instantiate(prototype, position, Quaternion.identity);
        LevelMaker.shared.AddObjectToMove(instantiatedItem.transform);
        lastSpawned = instantiatedItem.transform;
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
}
