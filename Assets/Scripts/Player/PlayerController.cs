using UnityEngine;
using DG.Tweening;

public class PlayerController: MonoBehaviour {

    public float streetWidth = 4;
    public float numberOfLanes = 4;
    public float zMovement = 1;
    public float speed = 1;
    public float slowmoSpeed = 1;

    private float initialSpeed;
    private Vector3 movement = Vector3.zero;
    private Vector3 initialPosition;
    private const float slowmoScale = 0.2f;

    void Start() {
        initialPosition = transform.position;
        initialSpeed = speed;
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            movement.x = -1;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            movement.x = 1;
        } else {
            movement.x = 0;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            movement.z = -1;
        } else if (Input.GetKey(KeyCode.UpArrow)) {
            movement.z = 1;
        } else {
            movement.z = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (LevelMaker.shared.IsInSlowmo()) {
                LevelMaker.shared.StopSlowmo();
                StopSlowmo();
            } else {
                LevelMaker.shared.StartSlowmo();
                StartSlowmo();
            }
        }
        movement *= Time.deltaTime * speed;
        ClampMovement();
        transform.Translate(movement);
    }

    void ClampMovement() {
        var position = transform.position + movement;
        var xLimit = streetWidth / 2;
        var xOffset = streetWidth / numberOfLanes * 0.5f;
        if (position.x < -xLimit + xOffset ||
            position.x > xLimit - xOffset) {
            movement.x = 0;
        }
        if (position.z < initialPosition.z - zMovement ||
            position.z > initialPosition.z + zMovement) {
            movement.z = 0;
        }
    }

    void StartSlowmo() {
        var duration = LevelMaker.stopSlowmoAnimationTime;
        transform.DOScaleX(slowmoScale, duration);
        DOTween.To(() => speed, x => speed = x, slowmoSpeed, duration);
    }

    void StopSlowmo() {
        var duration = LevelMaker.stopSlowmoAnimationTime;
        transform.DOScaleX(1f, duration);
        DOTween.To(() => speed, x => speed = x, initialSpeed, duration);
    }
}
