using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Door Objects")]
    [Space]
    [SerializeField] Transform northDoor;
    [SerializeField] Transform southDoor;
    [SerializeField] Transform eastDoor;
    [SerializeField] Transform westDoor;

    [Space]
    [Header("Wall Objects")]
    [SerializeField] Transform northWall;
    [SerializeField] Transform southWall;
    [SerializeField] Transform eastWall;
    [SerializeField] Transform westWall;

    [Space]
    [Header("Values")]

    [SerializeField] int insideWidht;
    [SerializeField] int insideHeight;

    [Space]
    [Header("Prefabs")]

    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject healthPrefab;
    [SerializeField] GameObject keyPrefab;
    [SerializeField] GameObject exitDoorPrefab;

    private List<Vector3> usedPositions = new List<Vector3>();

    public void GenerateInterior()
    {
        //Do we Spawn enemies?
        if(Random.value < Generation.Instance.enemySpawnChance)
        {
            SpawnPrefab(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], 1, Generation.Instance.maxEnemiesPerRoom + 1);
        }

        //Do we spawn coins?
        if (Random.value < Generation.Instance.coinSpawnChance)
        {
            SpawnPrefab(coinPrefab, 1, Generation.Instance.maxEnemiesPerRoom + 1);
        }

        //Do we spawn Health?
        if (Random.value < Generation.Instance.healthSpawnChance)
        {
            SpawnPrefab(healthPrefab, 1, Generation.Instance.maxEnemiesPerRoom + 1);
        }

        
    }

    public void SpawnPrefab(GameObject prefab, int min = 0, int max = 0)
    {
        int num = 1;

        if (min != 0 || max != 0) { num = Random.Range(min, max); }

        for (int x = 0; x < num; x++)
        {
            GameObject obj = Instantiate(prefab);
            Vector3 pos = transform.position + new Vector3(Random.Range(-insideWidht/2, insideWidht / 2 + 1), Random.Range(-insideHeight /2, insideHeight / 2 + 1), 0);

            while (usedPositions.Contains(pos))
            {
                pos = transform.position + new Vector3(Random.Range(-insideWidht / 2, insideWidht / 2 + 1), Random.Range(-insideHeight / 2, insideHeight / 2 + 1), 0);
            }

            obj.transform.position = pos;
            usedPositions.Add(pos);

            for (int y = 0; y < enemyPrefabs.Length; y++)
            {
                if (prefab == enemyPrefabs[y]) { EnemyManager.Instance.enemies.Add(obj.GetComponent<Enemy>()); }


            }
            
        }
    }

    public Transform GetNorthDoor()
    {
        return northDoor;
    }

    public Transform GetSouthDoor()
    {
        return southDoor;
    }

    public Transform GetEastDoor()
    {
        return eastDoor;
    }

    public Transform GetWestDoor()
    {
        return westDoor;
    }

    public Transform GetNorthWall()
    {
        return northWall;
    }

    public Transform GetSoutWall()
    {
        return southWall;
    }

    public Transform GetEastWall()
    {
        return eastWall;
    }

    public Transform GetWestWall()
    {
        return westWall;
    }

    public GameObject KeyPrefabInstantiate()
    {
        return keyPrefab;
    }

    public GameObject ExitPrefabInstantiate()
    {
        return exitDoorPrefab;
    }

}
