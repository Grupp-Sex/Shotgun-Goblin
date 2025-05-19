using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class InstansiateObjectOnParticleCollision : MonoBehaviour
{
   

    [SerializeField] List<GameObject> objects = new List<GameObject>();

    [SerializeField] int max_Objects_Spawned = 1;

    [SerializeField] bool deleatOnCompleation;

    public GameObject spawnDestination;

    protected int spawnCounter;


    public ParticleSystem particles;


    

    // Start is called before the first frame update
    void Start()
    {
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (spawnCounter < max_Objects_Spawned)
        {
            List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
            particles.GetCollisionEvents(other, events);

            SpawnObjects(objects, events.Last().intersection);
        }

        
    }


    protected void SpawnObjects(List<GameObject> objects, Vector3 position)
    {
        spawnCounter++;
        for(int i = 0; i < objects.Count; i++)
        {
            SpawnObject(objects[i], position);
            
        }

        CheckObjectCount();
    }

    protected void SpawnObject(GameObject obj, Vector3 position)
    {

        GameObject newObject = Instantiate(obj);

        newObject.transform.SetPositionAndRotation(position, obj.transform.rotation);

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
