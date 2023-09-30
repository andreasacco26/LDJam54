using UnityEngine;

public class VehicleMover: MonoBehaviour {
    const float minimumSpeed = -1f;
    const float maximumSpeed = 5f;
    public float speed;

    void Start() {
        speed = Random.Range(minimumSpeed, maximumSpeed);
    }
}
