using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class LevelMaker: MonoBehaviour {

    public static LevelMaker shared;

    public const float slowmoAnimationTime = 1.5f;
    public const float stopSlowmoAnimationTime = 0.5f;

    public float currentSpeed = 5f;
    public float worldSpeed = 5f;
    public float worldSlowmoSpeed = 5f;
    public int streetWidth = 17;
    public int numberOfLanes = 4;

    public Vector3 movingObjectsSpawnerPosition;
    public GameObject movingObjectsSpawner;
    private MovingObjectsSpawner _spawner;

    public Vector3 objectsDestroyerPosition;
    public GameObject objectsDestroyer;

    public Vector3 playerPosition;
    public GameObject player;

    public Vector3 leftBuilingsSpawnerPosition;
    public GameObject leftBuilingsSpawner;

    public Vector3 rightBuilingsSpawnerPosition;
    public GameObject rightBuilingsSpawner;

    public Vector3 streetSpawnerPosition;
    public GameObject streetSpawner;
    private StreetSpawner _streetSpawner;
    private readonly List<Transform> itemsToMove = new();

    void Start() {
        shared = this;
        currentSpeed = worldSpeed;
        BuildStreetSpawner();
        BuildMovingObjectsSpawner();
        BuildBuildingsSpawners();
        BuildObjectsDestroyer();
        BuildPlayer();
    }

    private void Update() {
        MoveItems();
    }

    private void FixedUpdate() {
        CleanItemsToMove();
    }

    void BuildMovingObjectsSpawner() {
        var instance = Instantiate(movingObjectsSpawner, movingObjectsSpawnerPosition, Quaternion.identity);
        CleanName(instance);
        _spawner = instance.GetComponent<MovingObjectsSpawner>();
        _spawner.speed = worldSpeed;
        _spawner.slowmoSpeed = worldSlowmoSpeed;
        _spawner.numberOfLanes = numberOfLanes;
        _spawner.streetWidth = streetWidth;
    }

    void BuildObjectsDestroyer() {
        var instance = Instantiate(objectsDestroyer, objectsDestroyerPosition, Quaternion.identity);
        CleanName(instance);
    }

    void BuildPlayer() {
        var playerObject = Instantiate(player, playerPosition, Quaternion.identity);
        CleanName(playerObject);
        var playerController = playerObject.GetComponent<PlayerController>();
        playerController.numberOfLanes = numberOfLanes;
        playerController.streetWidth = streetWidth;
    }

    void BuildBuildingsSpawners() {
        var left = Instantiate(leftBuilingsSpawner, leftBuilingsSpawnerPosition, Quaternion.identity);
        CleanName(left);
        var right = Instantiate(rightBuilingsSpawner, rightBuilingsSpawnerPosition, Quaternion.identity);
        CleanName(right);
    }

    void BuildStreetSpawner() {
        var instance = Instantiate(streetSpawner, streetSpawnerPosition, Quaternion.identity);
        CleanName(instance);
        _streetSpawner = instance.GetComponent<StreetSpawner>();
        _streetSpawner.numberOfLanes = numberOfLanes;
        _streetSpawner.streetWidth = streetWidth;
    }

    public void StartSlowmo() {
        this.DOComplete();
        DOTween.To(() => currentSpeed,
            x => currentSpeed = x,
            worldSlowmoSpeed, slowmoAnimationTime);
        DOTween.To(() => _spawner.speed,
            x => _spawner.speed = x,
            _spawner.slowmoSpeed, slowmoAnimationTime);
    }

    public void StopSlowmo() {
        this.DOComplete();
        DOTween.To(() => currentSpeed,
            x => currentSpeed = x,
            worldSpeed, stopSlowmoAnimationTime);
        DOTween.To(() => _spawner.speed,
            x => _spawner.speed = x,
            _spawner.initialSpeed, stopSlowmoAnimationTime);
    }

    public bool IsInSlowmo() {
        return _spawner.speed != _spawner.initialSpeed;
    }

    public void AddObjectToMove(Transform obj) {
        itemsToMove.Add(obj);
        obj.parent = transform;
    }

    private void MoveItems() {
        foreach (Transform t in itemsToMove) {
            if (t == null) continue;
            t.Translate(0, 0, -currentSpeed * Time.deltaTime, Space.World);
        }
    }

    private void CleanItemsToMove() {
        itemsToMove.Remove(null);
    }

    private void CleanName(GameObject obj) {
        obj.name = obj.name.Replace("(Clone)", "");
    }
}
