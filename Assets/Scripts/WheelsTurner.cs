using UnityEngine;

public class WheelsTurner : MonoBehaviour
{
    public GameObject leftTire, rightTire;
    public float maxRotation = 45;

    void RotateWheel(GameObject wheel, float angle) {
        wheel.transform.Rotate(0, angle, 0, Space.Self);
    }

    public void RotateWheels(float angle) {
        RotateWheel(leftTire, angle);
        RotateWheel(rightTire, angle);
    }
}
