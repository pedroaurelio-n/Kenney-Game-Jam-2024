using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] DirectionArrow directionArrow;
    [SerializeField] UIManager uiManager;
    [SerializeField] TimeManager timeManager;
    
    [Header("Point")]
    [SerializeField] PointCollectable pointPrefab;
    [SerializeField] PointCollectable bonusPrefab;
    [SerializeField] Transform[] pointSpawnPositions;
    [SerializeField] Vector2 pointSpawnInterval;
    [SerializeField] int bonusSpawnInterval;

    PointCollectable pointObject;
    PointCollectable bonusObject;
    PointCollectable currentPoint;
    int spawnIndex;

    void Start ()
    {
        pointObject = Instantiate(pointPrefab, transform);
        pointObject.OnPointCollected += HandlePointCollected;
        
        bonusObject = Instantiate(bonusPrefab, transform);
        bonusObject.OnPointCollected += HandlePointCollected;
        
        SpawnPoint();
    }

    void SpawnPoint ()
    {
        bool spawnBonus = spawnIndex > 0 && spawnIndex % bonusSpawnInterval == 0;
        currentPoint = spawnBonus ? bonusObject : pointObject;
        
        Transform spawnPoint = pointSpawnPositions[Random.Range(0, pointSpawnPositions.Length)];
        currentPoint.transform.position = spawnPoint.position;
        currentPoint.gameObject.SetActive(true);
        directionArrow.UpdateTarget(currentPoint.transform);
        spawnIndex++;
    }

    void HandlePointCollected (int point, float timeToAdd)
    {
        currentPoint.gameObject.SetActive(false);
        directionArrow.UpdateTarget(null);
        uiManager.UpdatePoints(point);
        timeManager.AddTime(timeToAdd);
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