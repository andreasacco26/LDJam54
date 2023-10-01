using System.Collections.Generic;
using UnityEngine;

public class KnightBusEffects : MonoBehaviour
{
    // 1. Exahust fire bang
    // 2. Death explosion? 
    // 3. Sound effects?
    [Tooltip("The speed at which the parts of the car will fly off when exploding.")]
    public float explosionForce = 30.0f;
    [Tooltip("The angular speed at which the parts of the car will fly off when exploding.")]
    public float explosionTorque = 30.0f;
    [Tooltip("Object that contains the car parts that will fly off when exploding.")]
    public GameObject knightBusPartsContainer;

    private List<Rigidbody> knightBusRigidbodies;

    public bool HasExploded { get; private set; }

    void Start()
    {
        HasExploded = false;
        knightBusRigidbodies = new(knightBusPartsContainer.GetComponentsInChildren<Rigidbody>());
        Debug.Log("Knight Bus Rigidbodies: " + knightBusRigidbodies.Count);
        knightBusPartsContainer.SetActive(false);

        Explode();
    }

    public void Explode() {
        HasExploded = true;

        // knightBusPartsContainer.transform.SetPositionAndRotation(
        //    PlayerController.shared.transform.position,
        //    PlayerController.shared.transform.rotation);
        // PlayerController.shared.gameObject.SetActive(false);
        
        knightBusPartsContainer.SetActive(true);

        Debug.Log("Iterating Rigidbodies");
        foreach (Rigidbody body in knightBusRigidbodies) {
            Vector3 forceVector = Random.onUnitSphere;
            forceVector.y = Mathf.Abs(forceVector.y) * 2;
            forceVector.Normalize();
            body.AddForce(forceVector * explosionForce, ForceMode.Impulse);
            body.AddTorque(Random.onUnitSphere * explosionTorque);
        }
    }

    public void Restore() {
        knightBusPartsContainer.SetActive(false);

        PlayerController.shared.gameObject.SetActive(true); 

        HasExploded = false;
    }
}
