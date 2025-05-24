using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class NavmeshCutoutBounds : MonoBehaviour
{

    public Vector3 sizeMult = new Vector3(1,1,1);
    [SerializeField] Vector3 boundSize;
    [SerializeField] bool carve;
    protected NavMeshObstacle navMeshObstacle;

    protected MeshFilter meshFilter;

    protected Bounds bounds;

    

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        StartCoroutine(SetBoundsOnCondition());

        //SetBounds();

    }
   

    protected void SetBounds()
    {

        bounds = meshFilter.sharedMesh.bounds;

        boundSize = bounds.size;

        Vector3 scaledBoundSize = Vector3.Scale(boundSize, sizeMult);
        
        navMeshObstacle.size = scaledBoundSize;

        navMeshObstacle.center = bounds.center;
        navMeshObstacle.carving = carve;

    }

    protected IEnumerator SetBoundsOnCondition()
    {
        yield return new WaitUntil(() => meshFilter != null && meshFilter.sharedMesh != null && navMeshObstacle != null);

        SetBounds();
    }


 

}
