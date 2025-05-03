using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnDisableParticles : MonoBehaviour
{
    [SerializeField] GameObject endParticles;

    [SerializeField] bool DoOnDisable = true;

    [SerializeField] EndParticleMaterialType OverideParticleMaterial;

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
                
                switch (OverideParticleMaterial)
                {
                    case EndParticleMaterialType.Particle or EndParticleMaterialType.Mesh:

                        ParticleSystem ps = PrepareParticles(objectShapeMesh);

                        if (OverideParticleMaterial == EndParticleMaterialType.Mesh)
                        {
                            SetParticleMaterial(ps, objectShapeMesh.materials.ToList());
                        }
                        
                        break;

                    case EndParticleMaterialType.OneForEachMesh:

                        Material[] materials = objectShapeMesh.materials;

                        for(int i = 0; i < materials.Length; i++)
                        {
                            ParticleSystem iteratedParticles = PrepareParticles(objectShapeMesh);

                            SetParticleMaterial(iteratedParticles, new List<Material> { materials[i]});

                        }

                        break;
                }

                //shape.texture = objectShapeMesh.
                
                
                
            }
        }
    }

    protected ParticleSystem PrepareParticles(MeshRenderer objectShapeMesh)
    {
        GameObject particleGameObject = Instantiate(endParticles);
        particleGameObject.transform.position = transform.position;

        //particleGameObject.transform.localScale = transform.lossyScale;

        ParticleSystem ps = particleGameObject.GetComponent<ParticleSystem>();
       
        var shape = ps.shape;

        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.MeshRenderer;
        shape.meshRenderer = objectShapeMesh;


        return ps;
        
    }

    protected void SetParticleMaterial(ParticleSystem ps, List<Material> materials)
    {
        ParticleSystemRenderer ps_renderer = ps.GetComponent<ParticleSystemRenderer>();

        ps_renderer.SetMaterials(materials);
    
    }

    


    public enum EndParticleMaterialType
    {
        Particle,
        Mesh,
        OneForEachMesh
    }


}
