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
    [SerializeField] private bool alwaysSpawnMaxObstacles = false;
    private bool canStart = false;
    [SerializeField] private float destroyZPosition;

    //Timers
    [SerializeField] private float spawnDelayTime;
    private float spawnDelayTimeAccumulator = 0f;

    void Start()
    {
        spawnPositionsLength = spawnPositions.Count;
        obstaclePrefabsLength = obstaclePrefabs.Count;
        if (spawnPositionsLength != 0 && obstaclePrefabsLength != 0)
            canStart = true;
    }

    void Update()
    {
        if (canStart)
        {
            //Spawn delay timer
            spawnDelayTimeAccumulator += Time.deltaTime;
            if (spawnDelayTimeAccumulator >= spawnDelayTime)
            {
                SpawnObstacles();
                spawnDelayTimeAccumulator = 0f;
            }

        }
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
            GameObject instantiatedObstacle = Instantiate(obstaclePrefabs[pair.Value], correctPos, Quaternion.identity);
            instantiatedObstacle.AddComponent(typeof(ObstacleMove));
            instantiatedObstacle.AddComponent(typeof(destroyObstacle));
            instantiatedObstacle.GetComponent<destroyObstacle>().SetDestroyPosition(destroyZPosition);
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
