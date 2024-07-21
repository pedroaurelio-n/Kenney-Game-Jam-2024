using System;
using PedroAurelio.AudioSystem;
using UnityEngine;

public class PlayerDeathbox : MonoBehaviour
{
    public event Action<bool> OnPlayerDeath;

    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] MeshRenderer bodyMesh;
    [SerializeField] Material deathMaterial;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] PlayAudioEvent deathAudio;
    
    PlayerInput playerInput;
    FollowerRecorder followerRecorder;
    CarController carController;

    bool dead;

    void Awake ()
    {
        followerRecorder = GetComponentInParent<FollowerRecorder>();
        playerInput = GetComponentInParent<PlayerInput>();
        carController = GetComponentInParent<CarController>();
    }

    void OnTriggerEnter (Collider other) => KillPlayer(true);

    public void KillPlayer (bool collision)
    {
        if (dead)
            return;

        dead = true;
        OnPlayerDeath?.Invoke(collision);
        
        rigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);
        bodyMesh.material = deathMaterial;
        deathParticle.Play();
        deathAudio.PlayAudio();
        
        playerInput.SetInput(false);
        followerRecorder.StopRecording();
        carController.StopMovement();
    }
}
