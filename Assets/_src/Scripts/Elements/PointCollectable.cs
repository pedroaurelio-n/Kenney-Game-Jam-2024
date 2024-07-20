using System;
using UnityEngine;

public class PointCollectable : BaseCollectable
{
    public event Action<int> OnPointCollected;
    
    [SerializeField] int value;

    FollowerRecorder followerRecorder;
    
    protected override void ActivateEffect (GameObject player)
    {
        if (followerRecorder == null)
            followerRecorder = player.GetComponent<FollowerRecorder>();
        
        OnPointCollected?.Invoke(value);
        followerRecorder.StartRecording();

        for (int i = 0; i < value; i++)
            followerRecorder.CreateFollower();
    }
}