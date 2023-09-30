using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour {

    public Vector3 centerPosition;
    public float streetWidth = 4;
    public float numberOfLanes = 4;
    public float zMovement = 1;
    public float speed = 1;
    public float slowmoSpeed = 1;

    private float initialSpeed;

    private Vector3 movement = Vector3.zero;

    void Start() {
        transform.position = centerPosition;
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
        if (position.z < centerPosition.z - zMovement ||
            position.z > centerPosition.z + zMovement) {
            movement.z = 0;
        }
    }

    void StartSlowmo() {
        var scale = transform.localScale;
        scale.x = 0.2f;
        transform.localScale = scale;
        speed = slowmoSpeed;
    }

    void StopSlowmo() {
        var scale = transform.localScale;
        scale.x = 1f;
        transform.localScale = scale;
        speed = initialSpeed;
    }
}
