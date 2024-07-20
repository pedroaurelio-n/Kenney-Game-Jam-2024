using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    CarController controller;
    FollowerRecorder followerRecorder;

    void Awake ()
    {
        controller = GetComponent<CarController>();
        followerRecorder = GetComponent<FollowerRecorder>();
    }

    void Update ()
    {
        float acceleration = 1f;
        float steering = Input.GetAxis("Horizontal");
        controller.SetAccelerationInput(acceleration);
        controller.SetSteeringInput(steering);
        
        if (Input.GetKeyDown(KeyCode.Return))
            followerRecorder.CreateFollower();
    }
}
