using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerRecorder : MonoBehaviour
{
    public event Action<RecordedTransform> OnNewRecordCreated;
    
    [SerializeField] float recordingInterval;
    [SerializeField] float routineInterval;
    [SerializeField] FollowerUpdater followerPrefab;
    [SerializeField] Transform followerContainer;

    int followerCount = 0;
    Coroutine recordingCoroutine;
    WaitForSeconds routineWait;

    public void StartRecording ()
    {
        if (recordingCoroutine != null)
            return;
        
        routineWait = new WaitForSeconds(routineInterval);
        recordingCoroutine = StartCoroutine(WriteRecordRoutine());
    }

    public void StopRecording ()
    {
        if (recordingCoroutine == null)
            return;
        
        StopCoroutine(recordingCoroutine);
    }

    public void CreateFollower ()
    {
        followerCount++;
        FollowerUpdater newFollower = Instantiate(followerPrefab, followerContainer);
        newFollower.SetupFollower(this, followerCount, recordingInterval, routineInterval);
    }

    IEnumerator WriteRecordRoutine ()
    {
        float timer = 0f;

        while (true)
        {
            if (timer <= recordingInterval)
            {
                timer += routineInterval;

                if (routineInterval > 0)
                    yield return routineWait;
                else
                    yield return null;
            }

            RecordedTransform recordedTransform = new(transform.position, transform.rotation);
            OnNewRecordCreated?.Invoke(recordedTransform);
            timer = 0f;
            yield return null;
        }
    }
}