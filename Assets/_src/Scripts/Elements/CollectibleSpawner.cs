using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] DirectionArrow directionArrow;

    [SerializeField] UIManager uiManager;
    
    [Header("Point")]
    [SerializeField] PointCollectable pointPrefab;
    [SerializeField] Transform[] pointSpawnPositions;
    [SerializeField] Vector2 pointSpawnInterval;

    PointCollectable pointObject;

    void Start ()
    {
        pointObject = Instantiate(pointPrefab, transform);
        pointObject.OnPointCollected += HandlePointCollected;
        
        SpawnPoint();
    }

    void SpawnPoint ()
    {
        Transform spawnPoint = pointSpawnPositions[Random.Range(0, pointSpawnPositions.Length)];
        pointObject.transform.position = spawnPoint.position;
        pointObject.gameObject.SetActive(true);
        directionArrow.UpdateTarget(pointObject.transform);
    }

    void HandlePointCollected (int point)
    {
        pointObject.gameObject.SetActive(false);
        directionArrow.UpdateTarget(null);
        uiManager.UpdatePoints(point);
        StartCoroutine(WaitForCollectibleSpawn(SpawnType.Point, pointSpawnInterval));
    }

    IEnumerator WaitForCollectibleSpawn (SpawnType type, Vector2 interval)
    {
        yield return new WaitForSeconds(Random.Range(interval.x, interval.y));

        switch (type)
        {
            case SpawnType.Point:
                SpawnPoint();
                break;
            case SpawnType.Boost:
                break;
        }
    }

    enum SpawnType
    {
        Point,
        Boost
    }
}