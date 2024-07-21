using System.Collections;
using Cinemachine;
using PedroAurelio.AudioSystem;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float forwardSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float steeringSpeed;
    [SerializeField] float steerVelocityWeight;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    [Header("Grounded Settings")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 castOffset;
    [SerializeField] float castDistance;
    [SerializeField] float castRadius;

    [Header("Camera Settings")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float boostFov;
    [SerializeField] float fovChangeSpeed;
    
    [Header("Effects")]
    [SerializeField] ParticleSystem[] driftParticles;
    [SerializeField] float driftThreshold;
    [SerializeField] float speedThreshold;
    [SerializeField] ParticleSystem boostParticle;

    [Header("Audio")]
    [SerializeField] PlayAudioEvent driftSfx;
    [SerializeField] PlayAudioEvent boostSfx;

    bool dead;

    float steeringInput;
    float accelerationInput;
    bool isGrounded;

    float movementMultiplier = 1;
    WaitForSeconds multiplierDuration;
    Coroutine multiplierRoutine;

    float startFov;

    Rigidbody _rigidbody;

    void Awake ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startFov = virtualCamera.m_Lens.FieldOfView;
    }

    void FixedUpdate ()
    {
        if (dead)
            return;
        
        CheckGround();
        Accelerate();
        Turn();
        EvaluateDrift();

        if (movementMultiplier != 1)
            virtualCamera.m_Lens.FieldOfView =
                Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, boostFov, fovChangeSpeed);
        else
            virtualCamera.m_Lens.FieldOfView =
                Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, startFov, fovChangeSpeed * 1.5f);
    }

    public void StopMovement ()
    {
        dead = true;
        driftParticles[0].Stop();
        driftParticles[1].Stop();
        driftSfx.StopAudio();
        _rigidbody.velocity = Vector3.zero;
    }

    public void SetAccelerationInput (float value) => accelerationInput = value;
    
    public void SetSteeringInput (float value) => steeringInput = value;
    
    public void SetMovementMultiplier (float multiplier, float duration)
    {
        if (multiplierRoutine != null)
        {
            StopCoroutine(multiplierRoutine);
            boostSfx.StopAudio();
        }
        
        multiplierDuration ??= new WaitForSeconds(duration);
        movementMultiplier = multiplier;
        boostParticle.Play();
        boostSfx.PlayAudio();
        
        multiplierRoutine = StartCoroutine(ResetMultiplier());
    }

    void CheckGround ()
    {
        isGrounded = false;
        _rigidbody.drag = airDrag;
        
        if (Physics.SphereCast(transform.position + castOffset, castRadius, Vector3.down, out RaycastHit hit, castDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            _rigidbody.drag = groundDrag;

            Quaternion rotationDirection = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed);
            transform.rotation = newRotation;
        }
        else
        {
            driftParticles[0].Stop();
            driftParticles[1].Stop();
        }
    }

    void Accelerate()
    {
        if (!isGrounded)
            return;
        
        Vector3 targetSpeed = accelerationInput * forwardSpeed * movementMultiplier * transform.forward;
        _rigidbody.AddForce(targetSpeed, ForceMode.Acceleration);
    }

    void Turn()
    {
        if (!isGrounded)
            return;
        
        float targetTurn = steeringSpeed * steeringInput;
        targetTurn *= movementMultiplier * steerVelocityWeight * _rigidbody.velocity.sqrMagnitude;
        targetTurn = Mathf.Clamp(targetTurn, -(steeringSpeed * Mathf.Abs(steeringInput)), steeringSpeed * Mathf.Abs(steeringInput)); 
        
        transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, targetTurn, 0f));
    }

    void EvaluateDrift ()
    {
        float dotProduct = Vector3.Dot(_rigidbody.velocity.normalized, transform.forward.normalized);
        if (dotProduct <= driftThreshold && _rigidbody.velocity.magnitude >= speedThreshold)
        {
            driftParticles[0].Play();
            driftParticles[1].Play();
            driftSfx.PlayAudio(true);
        }
        else
        {
            driftParticles[0].Stop();
            driftParticles[1].Stop();
            driftSfx.StopAudio();
        }
    }
    
    IEnumerator ResetMultiplier ()
    {
        yield return multiplierDuration;
        movementMultiplier = 1f;
        boostParticle.Stop();
        boostSfx.StopAudio();
        multiplierRoutine = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + castOffset, Vector3.down * castDistance);
    }
}
