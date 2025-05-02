using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnDisableParticles : MonoBehaviour
{
    [SerializeField] GameObject endParticles;

    [SerializeField] bool DoOnDisable = true;

    [SerializeField] MeshRenderer objectShapeMesh;

    protected bool hasBeenEnabled;
    
    void Start()
    {
        objectShapeMesh = GetComponent<MeshRenderer>(); 
        hasBeenEnabled = true;
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
        if (Application.isPlaying && hasBeenEnabled)
        {
            objectShapeMesh = GetComponent<MeshRenderer>();

            

            if (endParticles != null)
            {
                GameObject particleGameObject = Instantiate(endParticles);
                particleGameObject.transform.position = transform.position;

                //particleGameObject.transform.localScale = transform.lossyScale;

                ParticleSystem ps = particleGameObject.GetComponent<ParticleSystem>();
                ParticleSystemRenderer ps_renderer = ps.GetComponent<ParticleSystemRenderer>();
                var shape = ps.shape;

                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                shape.meshRenderer = objectShapeMesh;

                //shape.texture = objectShapeMesh.

                ps_renderer.SetMaterials(objectShapeMesh.materials.ToList());
                
            }
        }
    }


}
