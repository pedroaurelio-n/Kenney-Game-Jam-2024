using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float forwardSpeed;
    [SerializeField] float backwardsSpeed;
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

    float steeringInput;
    bool isGrounded;

    Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CheckGround();
        Accelerate();
        Turn();
    }
    
    public void SetSteeringInput(float value) => steeringInput = value;

    void CheckGround()
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
        
        Vector3 targetSpeed = forwardSpeed * transform.forward;
        _rigidbody.AddForce(targetSpeed, ForceMode.Acceleration);
    }

    void Turn()
    {
        if (!isGrounded)
            return;
        
        float targetTurn = steeringSpeed * steeringInput;
        targetTurn *= _rigidbody.velocity.sqrMagnitude * steerVelocityWeight;
        targetTurn = Mathf.Clamp(targetTurn, -(steeringSpeed * Mathf.Abs(steeringInput)), steeringSpeed * Mathf.Abs(steeringInput)); 
        
        transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, targetTurn, 0f));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + castOffset, Vector3.down * castDistance);
    }
}
