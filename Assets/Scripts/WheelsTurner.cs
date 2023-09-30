using UnityEngine;
using DG.Tweening;

public class WheelsTurner : MonoBehaviour
{
    public GameObject leftTire, rightTire;
    public float rotationAngle = 30f;
    public float rotationTime = 0.2f;

    void RotateTires(float rotationAngle) {
        leftTire.transform.DOLocalRotate(new Vector3(0, rotationAngle, 0), rotationTime, RotateMode.Fast);
        rightTire.transform.DOLocalRotate(new Vector3(0, rotationAngle + 180f, 0), rotationTime, RotateMode.Fast);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            RotateTires(rotationAngle);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            RotateTires(-rotationAngle);
        }
    }
}
