using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private float delayBetweenSpawn;
    [SerializeField] private List<Enemy> enemies;

    private NavMeshTriangulation triangulation;
    private Dictionary<int, PoolOfObjects> poolsOfEnemyObjects;

    private void Awake()
    {
        enemies = new List<Enemy>();
        poolsOfEnemyObjects = new Dictionary<int, PoolOfObjects>();

        for (int i = 0; i < enemies.Count; i++)
        {
            poolsOfEnemyObjects.Add(i, PoolOfObjects.CreateInstance(enemies[i], numberOfEnemies));
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
            SpawnRandomEnemy();
            numberOfSpawnedEnemies++;
        }

        yield return wait;
    }

    private void SpawnRandomEnemy()
    {
        SpawnEnemy(Random.Range(0, enemies.Count));
    }

    private void SpawnEnemy(int indexOfSpawn)
    {
        PoolableObject poolableObject = poolsOfEnemyObjects[indexOfSpawn].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            int indexOfVertex = Random.Range(0, triangulation.vertices.Length);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(triangulation.vertices[indexOfVertex], out hit, 2f, 1))
            {
                enemy.agent.Warp(hit.position);
                enemy.movement.Target = player;
                enemy.agent.enabled = true;
                enemy.movement.StartChase();
            }
        }
    }
}
