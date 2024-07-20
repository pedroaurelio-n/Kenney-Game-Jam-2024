using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    CarController controller;

    void Awake()
    {
        controller = GetComponent<CarController>();
    }

    void Update ()
    {
        float steering = Input.GetAxis("Horizontal");
        controller.SetSteeringInput(steering);
    }
}
