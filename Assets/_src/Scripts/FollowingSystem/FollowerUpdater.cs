using System;
using System.Collections;
using System.Collections.Generic;
using PedroAurelio.AudioSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowerUpdater : MonoBehaviour
{
    [SerializeField] float followerDelay;
    [SerializeField] BoxCollider _collider;

    [SerializeField] MeshRenderer bodyMesh;
    [SerializeField] Material[] materialList;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] Material deathMaterial;
    [SerializeField] PlayAudioEvent deathAudio;

    List<RecordedTransform> recordedTransforms = new();
    WaitForSeconds indexWait;
    WaitForSeconds indexMultipliedWait;
    Coroutine delayRoutine;
    
    int transformIndex;
    bool canUpdate;

    void FixedUpdate ()
    {
        if (!canUpdate)
            return;
        
        transform.position = recordedTransforms[transformIndex].Position;
        transform.rotation = recordedTransforms[transformIndex].Rotation;
        transformIndex++;
    }

    public void SetupFollower (FollowerRecorder recorder, int index, PlayerDeathbox playerDeathbox)
    {
        recorder.OnNewRecordCreated += HandleNewRecordCreated;
        playerDeathbox.OnPlayerDeath += HandlePlayerDeath;

        bodyMesh.material = materialList[Random.Range(0, materialList.Length)];
        
        transform.position = recorder.transform.position;
        transform.rotation = recorder.transform.rotation;

        indexWait = new WaitForSeconds(index * followerDelay);
        indexMultipliedWait = new WaitForSeconds(index * followerDelay * 3);
        delayRoutine = StartCoroutine(UpdateRoutine());
    }

    void HandleNewRecordCreated (RecordedTransform recordedTransform) => recordedTransforms.Add(recordedTransform);

    void HandlePlayerDeath (bool _)
    {
        canUpdate = false;
        
        if (delayRoutine != null)
            StopCoroutine(delayRoutine);

        StartCoroutine(DeathRoutine());
    }

    IEnumerator UpdateRoutine ()
    {
        yield return indexWait;
        _collider.enabled = true;
        canUpdate = true;
    }

    IEnumerator DeathRoutine ()
    {
        yield return indexMultipliedWait;
        bodyMesh.material = deathMaterial;
        deathParticle.Play();;
        deathAudio.PlayAudio();
    }
}