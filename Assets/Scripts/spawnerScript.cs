using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private List<Vector3> spawnPositions;
    private List<int> obstacles = new List<int>();
    private int spawnPositionsLength;
    private int obstaclePrefabsLength;
    private bool canStart = false;
    [SerializeField] private float destroyZPosition;

    //Timers
    public float initialSpawnDelayTime;
    public float spawnDelayTime;
    private float spawnDelayTimeAccumulator = 0f;

    void Start()
    {
        spawnPositionsLength = spawnPositions.Count;
        obstaclePrefabsLength = obstaclePrefabs.Count;
        if (spawnPositionsLength != 0 && obstaclePrefabsLength != 0)
            canStart = true;
        initialSpawnDelayTime = spawnDelayTime;
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
        RandomizeObstacles();
        int positionIndex = 0;
        foreach (int obstacleIndex in obstacles)
        {
            GameObject chosenObstacle = obstaclePrefabs[obstacleIndex];
            float correctXPos = spawnPositions[positionIndex].x;
            float correctYPos = CalculateYPosition(chosenObstacle);
            float correctZPos = spawnPositions[positionIndex].z;
            Vector3 correctPos = new Vector3(correctXPos, correctYPos, correctZPos);
            GameObject instantiatedObstacle = Instantiate(chosenObstacle, correctPos, Quaternion.identity);
            instantiatedObstacle.AddComponent(typeof(ObstacleMove));
            instantiatedObstacle.AddComponent(typeof(destroyObstacle));
            instantiatedObstacle.GetComponent<destroyObstacle>().SetDestroyPosition(destroyZPosition);
            positionIndex++;
        }
        
        obstacles.Clear();
    }

    private void RandomizeObstacles()
    {
        for (int i = 0; i < spawnPositionsLength; i++)
        {
            obstacles.Add(RandomizeElement(obstaclePrefabsLength));
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
