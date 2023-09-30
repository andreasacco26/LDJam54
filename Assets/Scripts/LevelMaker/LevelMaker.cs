using UnityEngine;
using DG.Tweening;

public class LevelMaker: MonoBehaviour {

    public static LevelMaker shared;

    public const float slowmoAnimationTime = 1.5f;
    public const float stopSlowmoAnimationTime = 0.5f;

    public Vector3 movingObjectsSpawnerPosition;
    public GameObject movingObjectsSpawner;
    private MovingObjectsSpawner spawner;

    public Vector3 objectsDestroyerPosition;
    public GameObject objectsDestroyer;

    public Vector3 playerPosition;
    public GameObject player;

    public Vector3 leftBuilingsSpawnerPosition;
    public GameObject leftBuilingsSpawner;
    private StaticObjectsSpawner _leftBuildingsSpawner;

    public Vector3 rightBuilingsSpawnerPosition;
    public GameObject rightBuilingsSpawner;
    private StaticObjectsSpawner _rightBuildingsSpawner;

    void Start() {
        shared = this;
        BuildMovingObjectsSpawner();
        BuildBuildingsSpawners();
        BuildObjectsDestroyer();
        BuildPlayer();
    }

    void BuildMovingObjectsSpawner() {
        var instance = Instantiate(movingObjectsSpawner, movingObjectsSpawnerPosition, Quaternion.identity);
        spawner = instance.GetComponent<MovingObjectsSpawner>();
    }

    void BuildObjectsDestroyer() {
        Instantiate(objectsDestroyer, objectsDestroyerPosition, Quaternion.identity);
    }

    void BuildPlayer() {
        var playerObject = Instantiate(player, playerPosition, Quaternion.identity);
        playerObject.GetComponent<PlayerController>().numberOfLanes = spawner.numberOfLanes;
    }

    void BuildBuildingsSpawners() {
        var left = Instantiate(leftBuilingsSpawner, leftBuilingsSpawnerPosition, Quaternion.identity);
        _leftBuildingsSpawner = left.GetComponent<StaticObjectsSpawner>();
        var right = Instantiate(rightBuilingsSpawner, rightBuilingsSpawnerPosition, Quaternion.identity);
        _rightBuildingsSpawner = right.GetComponent<StaticObjectsSpawner>();
    }

    public void StartSlowmo() {
        this.DOComplete();
        DOTween.To(() => spawner.speed,
            x => spawner.speed = x,
            spawner.slowmoSpeed, slowmoAnimationTime);
        DOTween.To(() => _leftBuildingsSpawner.speed,
            x => _leftBuildingsSpawner.speed = x,
            _leftBuildingsSpawner.slowmoSpeed, slowmoAnimationTime);
        DOTween.To(() => _rightBuildingsSpawner.speed,
            x => _rightBuildingsSpawner.speed = x,
            _rightBuildingsSpawner.slowmoSpeed, slowmoAnimationTime);
    }

    public void StopSlowmo() {
        this.DOComplete();
        DOTween.To(() => spawner.speed,
                    x => spawner.speed = x,
                    spawner.initialSpeed, stopSlowmoAnimationTime);
        DOTween.To(() => _leftBuildingsSpawner.speed,
                    x => _leftBuildingsSpawner.speed = x,
                    _leftBuildingsSpawner.initialSpeed, stopSlowmoAnimationTime);
        DOTween.To(() => _rightBuildingsSpawner.speed,
                    x => _rightBuildingsSpawner.speed = x,
                    _rightBuildingsSpawner.initialSpeed, stopSlowmoAnimationTime);
    }

    public bool IsInSlowmo() {
        return spawner.speed != spawner.initialSpeed;
    }
}
