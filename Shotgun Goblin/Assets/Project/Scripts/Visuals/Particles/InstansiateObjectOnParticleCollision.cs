using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class InstansiateObjectOnParticleCollision : MonoBehaviour
{
   

    [SerializeField] List<GameObject> objects = new List<GameObject>();

    [SerializeField] int max_Objects_Spawned = 1;
    [SerializeField] private int spawnCounter;


    [SerializeField] bool deleatOnCompleation;
    [SerializeField] bool prewarm = true;

    public GameObject spawnDestination;



    public ParticleSystem particles;

    protected Queue<GameObject[]> prewarmQueue = new Queue<GameObject[]>();

    

    // Start is called before the first frame update
    void Awake()
    {
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }

        var main = particles.main;
        
        main.maxParticles = max_Objects_Spawned;

        
    }

    private void Start()
    {
        if (prewarm)
        {
            PreawrmObjects(max_Objects_Spawned);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
        particles.GetCollisionEvents(other, events);

        ParticleCollision(other, events);
    }

    protected void ParticleCollision(GameObject other, List<ParticleCollisionEvent> events)
    {
        if (spawnCounter < max_Objects_Spawned)
        {
            

            SpawnObjects(events.Last().intersection);
        }
    }

    protected void SpawnObjects(Vector3 position)
    {
        if (prewarm)
        {
            SpawnPrewarmObjects(position);
        }
        else
        {
            SpawnObjects(objects, position);
        }

        CheckObjectCount();
    }

    protected void PreawrmObjects(int count)
    {
        GameObject queueHolder = new GameObject("PrewarmQueue");
        queueHolder.transform.parent = transform;
        queueHolder.SetActive(false);

        for (int i = 0; i < count; i++)
        {
            prewarmQueue.Enqueue(InstantiateList(objects, queueHolder.transform));
        }
    }

    protected void SpawnPrewarmObjects(Vector3 position)
    {
        if (prewarmQueue.TryDequeue(out GameObject[] dequee))
        {
            for(int i = 0; i < dequee.Length; i++)
            {
                InitializeObject(dequee[i], position);
            }
            spawnCounter++;
        }
        else
        {
            spawnCounter = max_Objects_Spawned;
        }
    }

    protected void SpawnObjects(List<GameObject> objects, Vector3 position)
    {
        spawnCounter++;
        GameObject[] gameObjects = InstantiateList(objects);
        InitialzieArray(gameObjects, position);
    }

    protected GameObject[] InstantiateList(List<GameObject> objects)
    {
        GameObject[] gameObjects = new GameObject[objects.Count];

        for (int i = 0; i < objects.Count; i++)
        {
            gameObjects[i] = Instantiate(objects[i]);

        }

        return gameObjects;
    }

    protected GameObject[] InstantiateList(List<GameObject> objects, Transform parrent)
    {
        GameObject[] gameObjects = new GameObject[objects.Count];

        for (int i = 0; i < objects.Count; i++)
        {
            gameObjects[i] = Instantiate(objects[i], parrent);

        }

        return gameObjects;
    }


    protected void InitialzieArray(GameObject[] objects, Vector3 position)
    {
        for(int i = 0; i < objects.Length; i++)
        {
            InitializeObject(objects[i], position);
        }
    }

    protected void InitializeObject(GameObject newObject, Vector3 position)
    {
        newObject.transform.SetPositionAndRotation(position, newObject.transform.rotation);

        if (spawnDestination != null)
        {
            newObject.transform.SetParent(spawnDestination.transform);
        }
    }

    

    protected void CheckObjectCount()
    {
        if (spawnCounter >= max_Objects_Spawned)
        {
            MaxObjectsSpawned();
        }
    }

    protected virtual void MaxObjectsSpawned()
    {
        if (deleatOnCompleation)
        {
            Destroy(gameObject);
        }
    }
}
