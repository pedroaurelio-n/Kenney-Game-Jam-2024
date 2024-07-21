using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    [SerializeField] protected ParticleSystem collectedParticle;
    
    protected abstract void ActivateEffect (GameObject player);

    void OnTriggerEnter (Collider other)
    {
        if (!other.TryGetComponent(out PlayerHitbox playerHitbox))
            return;
        
        ActivateEffect(playerHitbox.transform.parent.gameObject);
        collectedParticle.transform.parent = null;
        collectedParticle.transform.position = transform.position;
        collectedParticle.Play();
        gameObject.SetActive(false);
    }
}
