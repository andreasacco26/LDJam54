using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaker: MonoBehaviour {

    public static LevelMaker shared;

    public Vector3 movingObjectsSpawnerPosition;
    public GameObject movingObjectsSpawner;
    private MovingObjectsSpawner spawner;

    public Vector3 objectsDestroyerPosition;
    public GameObject objectsDestroyer;

    void Start() {
        shared = this;
        BuildMovingObjectsSpawner();
        BuildObjectsDestroyer();
    }

    void BuildMovingObjectsSpawner() {
        var instance = Instantiate(movingObjectsSpawner, movingObjectsSpawnerPosition, Quaternion.identity);
        spawner = instance.GetComponent<MovingObjectsSpawner>();
    }

    void BuildObjectsDestroyer() {
        Instantiate(objectsDestroyer, objectsDestroyerPosition, Quaternion.identity);
    }

    public void StartSlowmo() {
        spawner.speed = 1;
    }

    public void StopSlowmo() {
        spawner.speed = spawner.initialSpeed;
    }

    public bool IsInSlowmo() {
        return spawner.speed != spawner.initialSpeed;
    }
}
