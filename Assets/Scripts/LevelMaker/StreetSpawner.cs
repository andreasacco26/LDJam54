using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StreetSpawner: MonoBehaviour {
    public float streetWidth = 4;
    public int numberOfLanes = 4;
    public float speed = 5;
    public float slowmoSpeed = 1;
    public GameObject road;
    public GameObject sidewalk;
    public GameObject roadStripes;
    [HideInInspector]
    public float initialSpeed = 5;

    private GameObject prototype;
    private readonly List<Transform> itemsToMove = new();
    private Vector3 lastExtents;
    private int layer;

    void Start() {
        layer = LayerMask.NameToLayer("Obstacle");
        initialSpeed = speed;
        Spawn();
    }

    void Update() {
        //if (itemsToMove.Last() == null ||
        //    itemsToMove.Last().position.z + lastExtents.x < transform.position.z) {
        //    Spawn();
        //}
        //MoveItems();
        //CleanItemsToMove();
    }

    private void Spawn() {
        if (!prototype) {
            SetUpPrototype();
        }
    }

    private void SetUpPrototype() {
        prototype = new PlaneMaker {
            width = streetWidth,
            length = streetWidth * 2
        }.GetPlane();
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
