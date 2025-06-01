using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeParticlesOnDeath : MonoBehaviour, IDeathActivated
{
    [SerializeField] List<VolumeParticleSpawningData> particles;
    [SerializeField] ObjectVolumeParticels volumeSpawner;

    private void Start()
    {
        if(volumeSpawner == null)
        {
            volumeSpawner = GetComponent<ObjectVolumeParticels>();
        }
    }

    public void OnDeath(DamageInfo lastHit)
    {
        volumeSpawner?.SpawnVolumeParticles(particles);        
    }
}
