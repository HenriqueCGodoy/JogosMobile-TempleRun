using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private List<Vector3> spawnPositions;
    private Dictionary<int, int> occupiedPositions = new Dictionary<int, int>();
    private int spawnPositionsLength;
    private int obstaclePrefabsLength;

    [SerializeField] private float spawnDelayTime;
    [SerializeField] private float beginDelayTime;
    [SerializeField] private float obstacleLifetime = 5f;

    [SerializeField] private bool alwaysSpawnMaxObstacles = false;

    private bool beginSpawning = false;
    private bool canSpawnObstacle = true;

    void Start()
    {
        spawnPositionsLength = spawnPositions.Count;
        obstaclePrefabsLength = obstaclePrefabs.Count;
        if (spawnPositionsLength != 0 && obstaclePrefabsLength != 0)
            StartCoroutine(Begin());
    }

    void Update()
    {
        if (beginSpawning && canSpawnObstacle)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Begin()
    {
        beginSpawning = false;
        yield return new WaitForSeconds(beginDelayTime);
        beginSpawning = true;
    }

    IEnumerator Spawn()
    {
        canSpawnObstacle = false;
        SpawnObstacles();
        yield return new WaitForSeconds(spawnDelayTime);
        canSpawnObstacle = true;
    }

    private void SpawnObstacles()
    {
        RandomizeObstaclesAndPositions();
        foreach (KeyValuePair<int, int> pair in occupiedPositions)
        {
            float correctXPos = spawnPositions[pair.Key].x;
            float correctYPos = CalculateYPosition(obstaclePrefabs[pair.Value]);
            float correctZPos = spawnPositions[pair.Key].z;
            Vector3 correctPos = new Vector3(correctXPos, correctYPos, correctZPos);
            GameObject newObstacle = Instantiate(obstaclePrefabs[pair.Value], correctPos, Quaternion.identity);
            Destroy(newObstacle, obstacleLifetime);

        }
        occupiedPositions.Clear();
    }

    private void RandomizeObstaclesAndPositions()
    {
        int numOfObstacles;
        if (alwaysSpawnMaxObstacles)
            numOfObstacles = spawnPositionsLength;
        else
            numOfObstacles = RandomizeElement(spawnPositionsLength) + 1;
        Debug.Log("numObstacles: " + numOfObstacles);
        for (int i = 1; i <= numOfObstacles; i++)
        {
            int randomizedPositionIndex = RandomizeElement(spawnPositionsLength);
            int randomizedPrefabIndex = RandomizeElement(obstaclePrefabsLength);
            if (i != 1)
            {
                while (occupiedPositions.ContainsKey(randomizedPositionIndex))
                {
                    randomizedPositionIndex = RandomizeElement(spawnPositionsLength);
                }
            }
            occupiedPositions.Add(randomizedPositionIndex, randomizedPrefabIndex);
        }
    }

    private int RandomizeElement(int quantityOfElements)
    {
        return Random.Range(0, quantityOfElements);
    }

    private float CalculateYPosition(GameObject obj)
    {
        return obj.transform.localScale.y / 2;
    }

}
