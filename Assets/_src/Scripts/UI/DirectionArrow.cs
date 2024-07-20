using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform cameraContainer;
    [SerializeField] Transform player;

    Transform target;

    void FixedUpdate ()
    {
        transform.position = cameraContainer.position;
        
        if (target == null)
            return;

        Vector3 directionToTarget = target.position - player.position;
        directionToTarget.y = 0;

        if (directionToTarget != Vector3.zero)
        {
            Quaternion rotationDirection = Quaternion.LookRotation(directionToTarget);
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, rotationSpeed);
            transform.rotation = newRotation;
        }
    }

    public void UpdateTarget (Transform target) => this.target = target;
}
