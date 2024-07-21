using System.Collections;
using Cinemachine;
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
        CheckGround();
        Accelerate();
        Turn();

        if (movementMultiplier != 1)
            virtualCamera.m_Lens.FieldOfView =
                Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, boostFov, fovChangeSpeed);
        else
            virtualCamera.m_Lens.FieldOfView =
                Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, startFov, fovChangeSpeed * 1.5f);
    }

    public void StopMovement ()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void SetAccelerationInput (float value) => accelerationInput = value;
    
    public void SetSteeringInput (float value) => steeringInput = value;
    
    public void SetMovementMultiplier (float multiplier, float duration)
    {
        if (multiplierRoutine != null)
            StopCoroutine(multiplierRoutine);
        
        multiplierDuration ??= new WaitForSeconds(duration);
        movementMultiplier = multiplier;
        
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
    
    IEnumerator ResetMultiplier ()
    {
        yield return multiplierDuration;
        movementMultiplier = 1f;
        multiplierRoutine = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + castOffset, Vector3.down * castDistance);
    }
}
