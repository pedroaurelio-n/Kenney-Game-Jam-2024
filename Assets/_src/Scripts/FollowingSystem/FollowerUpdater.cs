using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerUpdater : MonoBehaviour
{
    [SerializeField] float followerDelay;
    
    List<RecordedTransform> recordedTransforms = new();
    WaitForSeconds routineWait;
    WaitForSeconds indexWait;

    public void SetupFollower (FollowerRecorder recorder, int index, float updateInterval, float routineInterval)
    {
        recorder.OnNewRecordCreated += HandleNewRecordCreated;
        
        transform.position = recorder.transform.position;
        transform.rotation = recorder.transform.rotation;

        routineWait = new WaitForSeconds(routineInterval);
        indexWait = new WaitForSeconds(index * followerDelay);
        StartCoroutine(UpdateRoutine(updateInterval, routineInterval));
    }

    void HandleNewRecordCreated (RecordedTransform recordedTransform) => recordedTransforms.Add(recordedTransform);

    IEnumerator UpdateRoutine (float updateInterval, float routineInterval)
    {
        yield return indexWait;
        
        float timer = 0f;
        int transformIndex = 0;

        while (true)
        {
            timer += routineInterval;
            if (timer <= updateInterval)
            {
                if (routineInterval > 0)
                    yield return routineWait;
                else
                    yield return null;
            }

            transform.position = recordedTransforms[transformIndex].Position;
            transform.rotation = recordedTransforms[transformIndex].Rotation;
            timer = 0f;
            transformIndex++;
            yield return null;
        }
    }
}