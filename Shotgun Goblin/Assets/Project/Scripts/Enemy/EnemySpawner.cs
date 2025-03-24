using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private float delayBetweenSpawn;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private MethodOfSpawning MethodOfSpawningEnemies;

    private NavMeshTriangulation triangulation;
    private Dictionary<int, PoolOfObjects> poolOfEnemyObjects = new Dictionary<int, PoolOfObjects>();

    private void Awake()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            poolOfEnemyObjects.Add(i, PoolOfObjects.CreateInstance(enemies[i], numberOfEnemies));
        }
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(delayBetweenSpawn);

        int numberOfSpawnedEnemies = 0;

        while ( numberOfSpawnedEnemies < numberOfEnemies)
        {
            if (MethodOfSpawningEnemies == MethodOfSpawning.Third)
            {
                SpawnThirdIsEnemy(numberOfSpawnedEnemies);
            }
            else if (MethodOfSpawningEnemies == MethodOfSpawning.Even)
            {
                SpawnEvenEnemies(numberOfSpawnedEnemies);
            }
            else
            {
                SpawnRandomEnemy();     
            }

            numberOfSpawnedEnemies++;
            yield return wait;
        }
    }

    private void SpawnRandomEnemy()
    {
        SpawnEnemy(Random.Range(0, enemies.Count));
    }

    private void SpawnEvenEnemies(int numberOfSpawnedEnemies)
    {
        int spawnIndex = numberOfSpawnedEnemies % enemies.Count;
        SpawnEnemy(spawnIndex);
    }

    private void SpawnThirdIsEnemy(int numberOfSpawnedEnemies)
    {
        int spawnIndex;
        if (numberOfSpawnedEnemies < numberOfEnemies/3)
        {
            spawnIndex = 0;
        }
        else
        {
            spawnIndex = numberOfSpawnedEnemies % enemies.Count;
        }

        SpawnEnemy(spawnIndex);
    }

    private void SpawnEnemy(int indexOfSpawn)
    {
        PoolableObject poolableObject = poolOfEnemyObjects[indexOfSpawn].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            int indexOfVertex = Random.Range(0, triangulation.vertices.Length);

            if (NavMesh.SamplePosition(triangulation.vertices[indexOfVertex], out NavMeshHit hit, 2f, 1))
            {
                enemy.agent.Warp(hit.position);
                enemy.movement.Target = player;
                enemy.agent.enabled = true;
                enemy.movement.StartChase();
            }
        }
    }

    private enum MethodOfSpawning
    {
        Random,
        Even,
        Third
    }
}
