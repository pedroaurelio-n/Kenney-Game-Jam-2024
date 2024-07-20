using System;
using UnityEngine;

public class PointCollectable : BaseCollectable
{
    public event Action OnPointCollected;

    FollowerRecorder followerRecorder;
    
    protected override void ActivateEffect (GameObject player)
    {
        if (followerRecorder == null)
            followerRecorder = player.GetComponent<FollowerRecorder>();
        
        OnPointCollected?.Invoke();
        followerRecorder.StartRecording();
        followerRecorder.CreateFollower();
    }
}