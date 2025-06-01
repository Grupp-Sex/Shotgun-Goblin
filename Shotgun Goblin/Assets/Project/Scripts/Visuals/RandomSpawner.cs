using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomSpawner : MonoBehaviour
{
    protected System.Random rd = new System.Random();
    public MeshFilter meshFilter;

    [SerializeField] private int ObjectCount;
    [SerializeField] private Vector3 randomOffset;
    [SerializeField] private float randomScale;
    [SerializeField] private Transform targetLocation;

    [SerializeField] private GameObject template;
    [SerializeField] private List<GameObject> spawnedObjects;

    [SerializeField] int vertexCount;

    [SerializeField] bool spawnObjectButton;



    private AsyncInstantiateOperation<GameObject> result;

    private void Start()
    {
        SpawnObjects(ObjectCount);
    }

    private void OnDisable()
    {
        result.Cancel();
    }


    protected void SpawnObjects(int count)
    {
        AsyncSpawn(count, transform.position, transform.lossyScale, GetMeshPoints(meshFilter));
    }

    

    protected async void AsyncSpawn(int count, Vector3 centerPos, Vector3 scale, Vector3[] ponts)
    {
        Vector3[] spawnPoints = await Task.Run(() => RandomPositions(centerPos, scale, ponts, count));


        AsyncInstantiateOperation<GameObject> opperation = AsyncInstantiate(count, spawnPoints, new Quaternion[count]);

        StartCoroutine(GetResults(opperation));
        
    } 

    protected IEnumerator GetResults(AsyncInstantiateOperation<GameObject> opperation)
    {
        yield return new WaitUntil(() => opperation.isDone);

        GameObject[] gameObjects = opperation.Result;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            spawnedObjects.Add(gameObjects[i]);
        }

    }

    private AsyncInstantiateOperation<GameObject> AsyncInstantiate(int count, Vector3[] spawnPoints, Quaternion[] rotations)
    {
        result = InstantiateAsync(template, count, targetLocation, spawnPoints, new Quaternion[count]);

        return result;
    }

    

    protected Vector3[] RandomPositions(Vector3 centerPos, Vector3 scale, Vector3[] ponts, int count)
    {
        Vector3[] spawnPoints = new Vector3[count];

        for(int i = 0; i < count; i++)
        {
            spawnPoints[i] =  GetRandomPos(ponts, scale) + GetRandomOffset(randomOffset) * randomScale;
        }

        return spawnPoints;
    }


  

    
    protected Vector3 GetRandomOffset(Vector3 randomOffset)
    {
        System.Random rd = new System.Random();

        float x = rd.Next(-255, 255) / 255f * randomOffset.x; 
        float y = rd.Next(-255, 255) / 255f * randomOffset.y;
        float z = rd.Next(-255, 255) / 255f * randomOffset.z;

        Vector3 randomPos = new Vector3(x,y,z);
        
        return randomPos;
    }

    protected Vector3[] GetMeshPoints(MeshFilter meshFilter)
    {
        Vector3[] points = meshFilter.mesh.vertices;

        meshFilter.transform.TransformPoints(points);

        vertexCount = points.Length;

        return points;
    }

    protected Vector3 GetRandomPos(Vector3[] points, Vector3 scale)
    {
        
        Vector3 randomPoint = points[rd.Next(points.Length)];
        //randomPoint.Scale(scale);
        return randomPoint;
    }

    private void Update()
    {
        if (spawnObjectButton)
        {
            spawnObjectButton = false;
            SpawnObjects(1);

            
        }
    }


}
