using UnityEngine;

public class BoostCollectable : BaseCollectable
{
    [SerializeField] float speedMultiplier;
    [SerializeField] float duration;
    
    CarController playerCar;
    
    protected override void ActivateEffect (GameObject player)
    {
        if (playerCar == null)
            playerCar = player.GetComponent<CarController>();

        playerCar.SetAccelerationMultiplier(speedMultiplier, duration);
    }
}