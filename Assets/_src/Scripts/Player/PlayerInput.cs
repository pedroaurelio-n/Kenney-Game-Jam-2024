using PedroAurelio.AudioSystem;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayAudioEvent engineAudio;

    CarController controller;
    FollowerRecorder followerRecorder;

    bool canInput;

    void Awake ()
    {
        controller = GetComponent<CarController>();
        followerRecorder = GetComponent<FollowerRecorder>();
    }

    void Update ()
    {
        if (!canInput)
            return;
        
        float acceleration = 1f;
        float steering = Input.GetAxis("Horizontal");
        controller.SetAccelerationInput(acceleration);
        controller.SetSteeringInput(steering);
        
        if (Input.GetKeyDown(KeyCode.Return))
            followerRecorder.CreateFollower();
    }

    public void SetInput (bool active)
    {
        canInput = active;
        engineAudio.gameObject.SetActive(active);
    }
}
