using System;
using UnityEngine;

public class PlayerDeathbox : MonoBehaviour
{
    public event Action<bool> OnPlayerDeath;
    
    PlayerInput playerInput;
    FollowerRecorder followerRecorder;
    CarController carController;

    void Awake ()
    {
        followerRecorder = GetComponentInParent<FollowerRecorder>();
        playerInput = GetComponentInParent<PlayerInput>();
        carController = GetComponentInParent<CarController>();
    }

    void OnTriggerEnter (Collider other) => KillPlayer(true);

    public void KillPlayer (bool collision)
    {
        OnPlayerDeath?.Invoke(collision);
        playerInput.SetInput(false);
        followerRecorder.StopRecording();
        carController.StopMovement();
    }
}
