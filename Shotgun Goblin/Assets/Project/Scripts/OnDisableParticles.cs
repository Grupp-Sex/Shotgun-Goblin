using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisableParticles : MonoBehaviour
{
    [SerializeField] GameObject endParticles;

    [SerializeField] bool DoOnDisable = true;

    [SerializeField] MeshRenderer objectShapeMesh;
    
    void Start()
    {
        objectShapeMesh = GetComponent<MeshRenderer>(); 
    }

    private void OnDisable()
    {
        if (DoOnDisable)
        {
            
            SpawnEndParticles();
        }

    }

    private void OnApplicationQuit()
    {
        DoOnDisable = false;
    }


    protected void SpawnEndParticles()
    {
        if (Application.isPlaying)
        {
            objectShapeMesh = GetComponent<MeshRenderer>();

            if (endParticles != null)
            {
                GameObject particleGameObject = Instantiate(endParticles);
                particleGameObject.transform.position = transform.position;

                particleGameObject.transform.localScale = transform.lossyScale;

                ParticleSystem ps = particleGameObject.GetComponent<ParticleSystem>();

                var shape = ps.shape;

                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                shape.meshRenderer = objectShapeMesh;
                
                
            }
        }
    }


}
