using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuController: MonoBehaviour {

    public CinemachineVirtualCamera menuCamera;
    public Vector3 lightInitialEulerAngles;
    public GameObject lights;
    public CanvasGroup menuUI;
    private Vector3 lightsGameplayEulerAngles;

    private void Start() {
        lightsGameplayEulerAngles = lights.transform.eulerAngles;
        lights.transform.eulerAngles = lightInitialEulerAngles;
        if (!LevelMaker.restartWithMenu) {
            menuCamera.Priority = 1;
            menuUI.alpha = 0;
            enabled = false;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        if (LevelMaker.restartWithMenu && Input.anyKeyDown) {
            menuCamera.Priority = 1;
            menuUI.DOFade(0, 0.5f).OnComplete(() => Destroy(menuUI.gameObject));
            LevelMaker.shared.StartGameplay();
            lights.transform.DORotate(lightsGameplayEulerAngles, 3f);
            GetComponent<AudioSource>().DOFade(0f, 1f).OnComplete(() => {
                Destroy(gameObject);
            });
            enabled = false;
        }
    }
}
