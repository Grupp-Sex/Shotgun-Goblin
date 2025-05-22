using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float rangeOfSpawn;
    [SerializeField] private Transform player;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private float delayBetweenSpawn;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private MethodOfSpawning MethodOfSpawningEnemies;

    private Dictionary<int, PoolOfObjects> poolOfEnemyObjects = new Dictionary<int, PoolOfObjects>();

    public EventPusher<GameObject> Event_ObjectSpawned = new EventPusher<GameObject>();
    private void Awake()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            poolOfEnemyObjects.Add(i, PoolOfObjects.CreateInstance(enemies[i], numberOfEnemies));
        }
    }

    private void Start()
    {
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

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 0.8f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void SpawnEnemy(int indexOfSpawn)
    {
        PoolableObject poolableObject = poolOfEnemyObjects[indexOfSpawn].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            Vector3 point;
            if (RandomPoint(transform.position, rangeOfSpawn, out point))
            {
                enemy.agent.Warp(point);
                enemy.movement.Target = player;
                enemy.agent.enabled = true;
                Debug.Log(point);

                enemy.movement.StartChase();
            }

            Event_ObjectSpawned.Invoke(this, enemy.gameObject);
        }
    }

    private enum MethodOfSpawning
    {
        Random,
        Even,
        Third
    }
}
