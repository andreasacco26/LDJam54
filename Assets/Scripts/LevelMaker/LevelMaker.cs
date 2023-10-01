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
    public float slowmoTimer = 4f;
    public float cooldownSlowmoTimer = 4f;

    public int streetWidth = 17;
    public int numberOfLanes = 4;

    [HideInInspector]
    public float currentSlowmoTimer = 0;

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
    private readonly List<Tweener> animators = new();
    private Tweener slowmoTimerAnimation;
    private Tweener slowmoCooldownTimerAnimation;

    void Start() {
        shared = this;
        currentSpeed = worldSpeed;
        BuildStreetSpawner();
        BuildMovingObjectsSpawner();
        BuildBuildingsSpawners();
        BuildObjectsDestroyer();
        BuildPlayer();
        currentSlowmoTimer = slowmoTimer;
    }

    private void Update() {
        MoveItems();
        Debug.Log("Timer:" + currentSlowmoTimer + " InSlowmo:" + IsInSlowmo());
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
        _streetSpawner.destroyerZ = objectsDestroyerPosition.z;
    }

    public void StartSlowmo() {
        if (!CanStartSlowmo()) {
            return;
        }
        if (slowmoCooldownTimerAnimation != null) {
            slowmoCooldownTimerAnimation.Kill();
            slowmoCooldownTimerAnimation = null;
        }
        foreach (Tweener animator in animators) {
            animator.Complete();
        }
        animators.Clear();
        animators.Add(DOTween.To(() => currentSpeed,
            x => currentSpeed = x,
            worldSlowmoSpeed, slowmoAnimationTime));
        animators.Add(DOTween.To(() => _spawner.speed,
            x => _spawner.speed = x,
            _spawner.slowmoSpeed, slowmoAnimationTime));

        var timer = slowmoTimer * (currentSlowmoTimer / slowmoTimer);
        slowmoTimerAnimation = DOTween.To(() => currentSlowmoTimer,
            x => currentSlowmoTimer = x,
            0, timer).OnComplete(() => StopSlowmo());
        PlayerController.shared.StartSlowmo();
    }

    public void StopSlowmo() {
        if (slowmoCooldownTimerAnimation != null) {
            return;
        }
        if (slowmoTimerAnimation != null) {
            slowmoTimerAnimation.Kill();
            slowmoTimerAnimation = null;
        }
        slowmoCooldownTimerAnimation = null;
        foreach (Tweener animator in animators) {
            animator.Complete();
        }
        animators.Clear();
        animators.Add(DOTween.To(() => currentSpeed,
            x => currentSpeed = x,
            worldSpeed, stopSlowmoAnimationTime));
        animators.Add(DOTween.To(() => _spawner.speed,
            x => _spawner.speed = x,
            _spawner.initialSpeed, stopSlowmoAnimationTime));

        var timer = cooldownSlowmoTimer * (1 - (currentSlowmoTimer / slowmoTimer));
        slowmoCooldownTimerAnimation = DOTween.To(() => currentSlowmoTimer,
            x => currentSlowmoTimer = x,
            slowmoTimer, timer);
        PlayerController.shared.StopSlowmo();
    }

    public bool CanStartSlowmo() {
        return !IsInSlowmo();
    }

    public bool IsInSlowmo() {
        return slowmoTimerAnimation != null;
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
