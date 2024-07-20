using System;
using PedroAurelio.AudioSystem;
using UnityEngine;

public class PointCollectable : BaseCollectable
{
    public event Action<int, float> OnPointCollected;
    
    [SerializeField] int value;
    [SerializeField] float timeToAdd;
    [SerializeField] PlayAudioEvent collectedSfx;

    FollowerRecorder followerRecorder;
    
    protected override void ActivateEffect (GameObject player)
    {
        if (followerRecorder == null)
            followerRecorder = player.GetComponent<FollowerRecorder>();
        
        collectedSfx.PlayAudio();
        OnPointCollected?.Invoke(value, timeToAdd);
        followerRecorder.StartRecording();

        for (int i = 0; i < value; i++)
            followerRecorder.CreateFollower();
    }
}