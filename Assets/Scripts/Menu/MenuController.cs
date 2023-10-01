using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class MenuController: MonoBehaviour {

    public CinemachineVirtualCamera menuCamera;
    public Vector3 lightInitialEulerAngles;
    public GameObject lights;
    private Vector3 lightsGameplayEulerAngles;

    private void Start() {
        lightsGameplayEulerAngles = lights.transform.eulerAngles;
        lights.transform.eulerAngles = lightInitialEulerAngles;
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {
            menuCamera.Priority = 1;
            LevelMaker.shared.StartGameplay();
            lights.transform.DORotate(lightsGameplayEulerAngles, 3f);
            enabled = false;
            Destroy(gameObject);
        }
    }
}
