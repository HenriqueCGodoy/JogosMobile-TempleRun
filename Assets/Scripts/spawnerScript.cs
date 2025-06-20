using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private List<Vector3> spawnPositions;
    private Dictionary<int, int> occupiedPositions;
    private int spawnPositionsLength;
    private int obstaclePrefabsLength;

    [SerializeField] private float spawnDelayTime;
    [SerializeField] private float beginDelayTime;

    void Start()
    {
        spawnPositionsLength = spawnPositions.Count;
        obstaclePrefabsLength = obstaclePrefabs.Count;
        if (spawnPositionsLength != 0 && obstaclePrefabsLength != 0)
            StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        yield return new WaitForSeconds(beginDelayTime);
        InvokeRepeating("SpawnObstacles", 0, spawnDelayTime);
    }

    private void SpawnObstacles()
    {
        RandomizeObstaclesAndPositions();
        foreach (KeyValuePair<int, int> pair in occupiedPositions)
        {
            Instantiate(obstaclePrefabs[pair.Value], spawnPositions[pair.Key], Quaternion.identity);
        }
    }

    private void RandomizeObstaclesAndPositions()
    {
        int numOfObstacles = RandomizeElement(spawnPositionsLength) + 1;
        for (int i = 1; i <= numOfObstacles; i++)
        {
            int randomizedPositionIndex;
            int randomizedPrefabIndex = RandomizeElement(obstaclePrefabsLength);
            do
            {
                randomizedPositionIndex = RandomizeElement(spawnPositionsLength);
            } while (occupiedPositions.ContainsKey(randomizedPositionIndex));
            occupiedPositions.Add(randomizedPositionIndex, randomizedPrefabIndex);
        }
    }

    private int RandomizeElement(int quantityOfElements)
    {
        return Random.Range(0, quantityOfElements - 1);
    }
}
