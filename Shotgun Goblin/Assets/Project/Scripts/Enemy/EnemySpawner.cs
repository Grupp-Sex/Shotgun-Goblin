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
    [SerializeField] private float spawnMaxDistance = 0.8f;
    [SerializeField] private Vector3 spawnOffset;

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

    // handles the delay between spawns and checks/uses choosen method of spawning
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

    // randomly picks one of the object(s) in the enemies list and spawns it
    private void SpawnRandomEnemy()
    {
        SpawnEnemy(Random.Range(0, enemies.Count));
    }


    // makes it so the object(s) in the enemies list can spawn an even amount between eachother 
    private void SpawnEvenEnemies(int numberOfSpawnedEnemies)
    {
        int spawnIndex = numberOfSpawnedEnemies % enemies.Count;
        SpawnEnemy(spawnIndex);
    }

    // makes it so the first object in the enemies list gets spawned at least a third of the time
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

    // makes it so objects spawn at a random point on a navmesh within a specified sphere
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, spawnMaxDistance, NavMesh.AllAreas))
            {
                result = hit.position + spawnOffset;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    // takes an object from poolOfEnemyObjects at a specified index, spawns it and activates the objects behavior
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
// by Wilmer