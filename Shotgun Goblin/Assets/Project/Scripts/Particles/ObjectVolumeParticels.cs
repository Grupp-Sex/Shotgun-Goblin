using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectVolumeParticels : MonoBehaviour
{


    [SerializeField] MeshRenderer objectShapeMesh;

    

    void Start()
    {
        objectShapeMesh = GetComponent<MeshRenderer>();
        
    }

    public void SpawnVolumeParticles(List<VolumeParticleSpawningData> volumeParticleList)
    {
        for(int i = 0; i < volumeParticleList.Count; i++)
        {
            SpawnVolumeParticles(volumeParticleList[i]);
        }
    }

    public void SpawnVolumeParticles(VolumeParticleSpawningData spawningData)
    {
        if (Application.isPlaying)
        {
            objectShapeMesh = GetComponent<MeshRenderer>();

            GameObject volumeParticles = spawningData.particleEmitterHolder;

            VolumeParticleMaterialType overideParticleMaterial = spawningData.OverideParticleMaterial;

            if (volumeParticles != null)
            {

                switch (overideParticleMaterial)
                {
                    case VolumeParticleMaterialType.Particle or VolumeParticleMaterialType.Mesh:

                        ParticleSystem ps = PrepareParticles(volumeParticles, objectShapeMesh);

                        if (overideParticleMaterial == VolumeParticleMaterialType.Mesh)
                        {
                            SetParticleMaterial(ps, objectShapeMesh.materials.ToList());
                        }

                        break;

                    case VolumeParticleMaterialType.OneForEachMesh:

                        Material[] materials = objectShapeMesh.materials;

                        for (int i = 0; i < materials.Length; i++)
                        {
                            ParticleSystem iteratedParticles = PrepareParticles(volumeParticles, objectShapeMesh);

                            SetParticleMaterial(iteratedParticles, new List<Material> { materials[i] });

                        }

                        break;
                }

                //shape.texture = objectShapeMesh.



            }
        }
    }

    protected ParticleSystem PrepareParticles(GameObject volumeParticles, MeshRenderer objectShapeMesh)
    {
        GameObject particleGameObject = Instantiate(volumeParticles);
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




    

}

public enum VolumeParticleMaterialType
{
    Particle,
    Mesh,
    OneForEachMesh
}

[System.Serializable]
public struct VolumeParticleSpawningData
{
    public GameObject particleEmitterHolder;
    public VolumeParticleMaterialType OverideParticleMaterial;

}
