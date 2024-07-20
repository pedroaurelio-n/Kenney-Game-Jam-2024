using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour
{
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
    }

    void HandlePointCollected ()
    {
        pointObject.gameObject.SetActive(false);
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