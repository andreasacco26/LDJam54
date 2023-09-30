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

    void Start() {
        shared = this;
        BuildMovingObjectsSpawner();
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

    public void StartSlowmo() {
        this.DOComplete();
        DOTween.To(() => spawner.speed,
            x => spawner.speed = x,
            spawner.slowmoSpeed, slowmoAnimationTime);
    }

    public void StopSlowmo() {
        this.DOComplete();
        DOTween.To(() => spawner.speed,
                    x => spawner.speed = x,
                    spawner.initialSpeed, stopSlowmoAnimationTime);
    }

    public bool IsInSlowmo() {
        return spawner.speed != spawner.initialSpeed;
    }
}
