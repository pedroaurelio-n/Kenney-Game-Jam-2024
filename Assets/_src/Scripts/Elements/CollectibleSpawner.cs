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

    [Header("Boost")]
    [SerializeField] BoostCollectable boostPrefab;
    [SerializeField] Transform[] boostSpawnPositions;
    [SerializeField] Vector2 boostSpawnInterval;

    PointCollectable pointObject;
    PointCollectable bonusObject;
    PointCollectable currentPoint;
    int spawnIndex;

    BoostCollectable boostObject1;
    BoostCollectable boostObject2;

    void Start ()
    {
        pointObject = Instantiate(pointPrefab, transform);
        pointObject.OnPointCollected += HandlePointCollected;
        
        bonusObject = Instantiate(bonusPrefab, transform);
        bonusObject.OnPointCollected += HandlePointCollected;

        boostObject1 = Instantiate(boostPrefab, transform);
        boostObject2 = Instantiate(boostPrefab, transform);
        boostObject1.gameObject.SetActive(false);
        boostObject2.gameObject.SetActive(false);

        SpawnPoint();
        StartCoroutine(SpawnBoostRoutine());
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
    
    void SpawnBoost (BoostCollectable boostObject)
    {
        Transform spawnPoint = boostSpawnPositions[Random.Range(0, boostSpawnPositions.Length)];
        boostObject.transform.position = spawnPoint.position;
        boostObject.gameObject.SetActive(true);
    }

    void HandlePointCollected (int point, float timeToAdd)
    {
        currentPoint.gameObject.SetActive(false);
        directionArrow.UpdateTarget(null);
        uiManager.UpdatePoints(point);
        timeManager.AddTime(timeToAdd);
        StartCoroutine(WaitForCollectibleSpawn(SpawnType.Point));
    }

    IEnumerator WaitForCollectibleSpawn (SpawnType type)
    {
        yield return new WaitForSeconds(Random.Range(pointSpawnInterval.x, pointSpawnInterval.y));

        switch (type)
        {
            case SpawnType.Point:
                SpawnPoint();
                break;
            case SpawnType.Boost:
                break;
        }
    }

    IEnumerator SpawnBoostRoutine ()
    {
        while (true)
        {
            Reset:
            yield return new WaitForSeconds(Random.Range(boostSpawnInterval.x, boostSpawnInterval.y));
            
            if (!boostObject1.gameObject.activeInHierarchy)
            {
                SpawnBoost(boostObject1);
                goto Reset;
            }
            
            if (!boostObject2.gameObject.activeInHierarchy)
                SpawnBoost(boostObject2);
        }
    }

    enum SpawnType
    {
        Point,
        Boost
    }
}