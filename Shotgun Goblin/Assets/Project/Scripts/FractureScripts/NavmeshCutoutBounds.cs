using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class NavmeshCutoutBounds : MonoBehaviour
{

    [SerializeField] Vector3 sizeMult = new Vector3(1,1,1);
    [SerializeField] Vector3 boundSize;
    protected NavMeshObstacle navMeshObstacle;

    protected Bounds bounds;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        
        SetBounds();

    }
   

    protected void SetBounds()
    {

        bounds = GetComponent<MeshFilter>().sharedMesh.bounds;

        boundSize = bounds.size;

        Vector3 scaledBoundSize = Vector3.Scale(boundSize, sizeMult);
        
        navMeshObstacle.size = scaledBoundSize;

        navMeshObstacle.center = bounds.center;

    }

 

}
