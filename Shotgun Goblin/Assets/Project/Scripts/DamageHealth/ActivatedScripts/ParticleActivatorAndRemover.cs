using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActivatorAndRemover : MonoBehaviour, IDeathActivated
{
    [SerializeField] List<GameObject> particleHolders;

    public void OnDeath(DamageInfo damage)
    {
        for (int i = 0; i < particleHolders.Count; i++)
        {
            ActiveteParticleHolder(particleHolders[i]);
        }
    }

    protected void ActiveteParticleHolder(GameObject holder)
    {
        holder.transform.parent = transform.parent;
        holder.SetActive(true);
    }
}
