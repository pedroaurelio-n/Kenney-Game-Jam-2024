using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    protected abstract void ActivateEffect (GameObject player);

    void OnTriggerEnter (Collider other)
    {
        if (!other.TryGetComponent(out PlayerHitbox playerHitbox))
            return;
        
        ActivateEffect(playerHitbox.transform.parent.gameObject);
        gameObject.SetActive(false);
    }
}
