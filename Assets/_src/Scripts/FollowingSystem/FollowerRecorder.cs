using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerRecorder : MonoBehaviour
{
    public event Action<RecordedTransform> OnNewRecordCreated;
    
    [SerializeField] FollowerUpdater followerPrefab;
    [SerializeField] Transform followerContainer;

    int followerCount = 0;
    bool canRecord;
    
    PlayerDeathbox playerDeathbox;

    void Awake ()
    {
        playerDeathbox = GetComponentInChildren<PlayerDeathbox>();
    }

    void FixedUpdate ()
    {
        if (!canRecord)
            return;
        
        RecordedTransform recordedTransform = new(transform.position, transform.rotation);
        OnNewRecordCreated?.Invoke(recordedTransform);
    }

    public void StartRecording () => canRecord = true;

    public void StopRecording ()
    {
        Debug.Log($"stop recording");
        canRecord = false;
    }

    public void CreateFollower ()
    {
        followerCount++;
        FollowerUpdater newFollower = Instantiate(followerPrefab, followerContainer);
        newFollower.SetupFollower(this, followerCount, playerDeathbox);
    }
}