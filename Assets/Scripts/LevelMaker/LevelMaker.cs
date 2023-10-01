using UnityEngine;
using DG.Tweening;

public class LevelMaker: MonoBehaviour {

    public static LevelMaker shared;

    public const float slowmoAnimationTime = 1.5f;
    public const float stopSlowmoAnimationTime = 0.5f;

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
    private StaticObjectsSpawner _leftBuildingsSpawner;

    public Vector3 rightBuilingsSpawnerPosition;
    public GameObject rightBuilingsSpawner;
    private StaticObjectsSpawner _rightBuildingsSpawner;

    public Vector3 streetSpawnerPosition;
    public GameObject streetSpawner;
    private StreetSpawner _streetSpawner;

    void Start() {
        shared = this;
        BuildStreetSpawner();
        BuildMovingObjectsSpawner();
        BuildBuildingsSpawners();
        BuildObjectsDestroyer();
        BuildPlayer();
    }

    void BuildMovingObjectsSpawner() {
        var instance = Instantiate(movingObjectsSpawner, movingObjectsSpawnerPosition, Quaternion.identity);
        _spawner = instance.GetComponent<MovingObjectsSpawner>();
        _spawner.speed = worldSpeed;
        _spawner.slowmoSpeed = worldSlowmoSpeed;
        _spawner.numberOfLanes = numberOfLanes;
        _spawner.streetWidth = streetWidth;
    }

    void BuildObjectsDestroyer() {
        Instantiate(objectsDestroyer, objectsDestroyerPosition, Quaternion.identity);
    }

    void BuildPlayer() {
        var playerObject = Instantiate(player, playerPosition, Quaternion.identity);
        var playerController = playerObject.GetComponent<PlayerController>();
        playerController.numberOfLanes = numberOfLanes;
        playerController.streetWidth = streetWidth;
    }

    void BuildBuildingsSpawners() {
        var left = Instantiate(leftBuilingsSpawner, leftBuilingsSpawnerPosition, Quaternion.identity);
        _leftBuildingsSpawner = left.GetComponent<StaticObjectsSpawner>();
        _leftBuildingsSpawner.speed = worldSpeed;
        _leftBuildingsSpawner.slowmoSpeed = worldSlowmoSpeed;
        var right = Instantiate(rightBuilingsSpawner, rightBuilingsSpawnerPosition, Quaternion.identity);
        _rightBuildingsSpawner = right.GetComponent<StaticObjectsSpawner>();
        _rightBuildingsSpawner.speed = worldSpeed;
        _rightBuildingsSpawner.slowmoSpeed = worldSlowmoSpeed;
    }

    void BuildStreetSpawner() {
        var instance = Instantiate(streetSpawner, streetSpawnerPosition, Quaternion.identity);
        _streetSpawner = instance.GetComponent<StreetSpawner>();
        _streetSpawner.speed = worldSpeed;
        _streetSpawner.slowmoSpeed = worldSlowmoSpeed;
        _streetSpawner.numberOfLanes = numberOfLanes;
        _streetSpawner.streetWidth = streetWidth;
    }

    public void StartSlowmo() {
        this.DOComplete();
        DOTween.To(() => _spawner.speed,
            x => _spawner.speed = x,
            _spawner.slowmoSpeed, slowmoAnimationTime);
        DOTween.To(() => _leftBuildingsSpawner.speed,
            x => _leftBuildingsSpawner.speed = x,
            _leftBuildingsSpawner.slowmoSpeed, slowmoAnimationTime);
        DOTween.To(() => _rightBuildingsSpawner.speed,
            x => _rightBuildingsSpawner.speed = x,
            _rightBuildingsSpawner.slowmoSpeed, slowmoAnimationTime);
        DOTween.To(() => _streetSpawner.speed,
            x => _streetSpawner.speed = x,
            _streetSpawner.slowmoSpeed, slowmoAnimationTime);
    }

    public void StopSlowmo() {
        this.DOComplete();
        DOTween.To(() => _spawner.speed,
                    x => _spawner.speed = x,
                    _spawner.initialSpeed, stopSlowmoAnimationTime);
        DOTween.To(() => _leftBuildingsSpawner.speed,
                    x => _leftBuildingsSpawner.speed = x,
                    _leftBuildingsSpawner.initialSpeed, stopSlowmoAnimationTime);
        DOTween.To(() => _rightBuildingsSpawner.speed,
                    x => _rightBuildingsSpawner.speed = x,
                    _rightBuildingsSpawner.initialSpeed, stopSlowmoAnimationTime);
        DOTween.To(() => _streetSpawner.speed,
                    x => _streetSpawner.speed = x,
                    _streetSpawner.initialSpeed, stopSlowmoAnimationTime);
    }

    public bool IsInSlowmo() {
        return _spawner.speed != _spawner.initialSpeed;
    }
}
