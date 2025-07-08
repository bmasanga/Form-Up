using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public float spawnDuration;
    public float spawnRate;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float initialSpawnDelay = 5f; // Time before first enemy spawn
    [SerializeField] private List<EnemySpawnData> enemySpawnDataList = new List<EnemySpawnData>(); // List of different enemy prefabs and their spawn settings
    [SerializeField] private bool canSpawn = true;

    private int currentEnemyIndex = 0;

    void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        // Initial delay before starting the spawn
        yield return new WaitForSeconds(initialSpawnDelay);

        while (canSpawn)
        {
            // Get the current enemy spawn data
            EnemySpawnData currentEnemyData = enemySpawnDataList[currentEnemyIndex];

            // Spawn enemies for the duration specified for the current enemy type
            float spawnEndTime = Time.time + currentEnemyData.spawnDuration;

            while (Time.time < spawnEndTime)
            {
                Instantiate(currentEnemyData.enemyPrefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(currentEnemyData.spawnRate);
            }

            // Move to the next enemy type
            currentEnemyIndex = (currentEnemyIndex + 1) % enemySpawnDataList.Count;
        }
    }
}
