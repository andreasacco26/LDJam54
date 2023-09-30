using UnityEngine;
using DG.Tweening;

public class WheelsTurner: MonoBehaviour {
    public GameObject leftTire, rightTire;
    public float rotationAngle = 30f;
    public float bodyRotationAngle = 10f;
    public float rotationTime = 0.2f;

    void RotateTires(float rotationAngle) {
        leftTire.transform.DOLocalRotate(new Vector3(0, rotationAngle, 0), rotationTime, RotateMode.Fast);
        rightTire.transform.DOLocalRotate(new Vector3(0, rotationAngle + 180f, 0), rotationTime, RotateMode.Fast);
    }

    void Update() {
        var movement = PlayerController.shared.movement;
        var parent = transform.parent;
        if (movement.x < 0 && parent.eulerAngles.y >= 0) {
            RotateTires(-rotationAngle);
            parent.DOLocalRotate(new Vector3(0, -bodyRotationAngle, 0), rotationTime);
        } else if (movement.x > 0) {
            RotateTires(rotationAngle);
            parent.DOLocalRotate(new Vector3(0, bodyRotationAngle, 0), rotationTime);
        } else {
            if (parent.eulerAngles != Vector3.zero &&
                Mathf.Abs(parent.eulerAngles.y) != 10) {
                RotateTires(0);
                parent.DOLocalRotate(Vector3.zero, rotationTime);
            }
        }
    }
}
