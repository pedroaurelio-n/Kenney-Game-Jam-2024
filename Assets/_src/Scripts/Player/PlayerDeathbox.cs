using System;
using UnityEngine;

public class PlayerDeathbox : MonoBehaviour
{
    public event Action OnPlayerDeath;
    
    PlayerInput playerInput;
    FollowerRecorder followerRecorder;
    CarController carController;

    void Awake ()
    {
        followerRecorder = GetComponentInParent<FollowerRecorder>();
        playerInput = GetComponentInParent<PlayerInput>();
        carController = GetComponentInParent<CarController>();
    }

    void OnTriggerEnter (Collider other)
    {
        OnPlayerDeath?.Invoke();
        playerInput.CanInput = false;
        followerRecorder.StopRecording();
        carController.StopMovement();
    }
}
