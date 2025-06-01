using System.Collections.Generic;
using UnityEngine;

public class OnDisableParticles : MonoBehaviour
{
    [SerializeField] List<VolumeParticleSpawningData> particles;

    [SerializeField] ObjectVolumeParticels volumeSpawner;

    [SerializeField] bool DoOnDisable = true;

   
    protected bool hasBeenEnabled;
    
    void Start()
    {
        if( volumeSpawner == null)
        {
            volumeSpawner = GetComponent<ObjectVolumeParticels>();
        }
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
        if (hasBeenEnabled)
        {
            volumeSpawner.SpawnVolumeParticles(particles);
        }
    }

}
