using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour, IDeathActivated
{

    protected Explosive explosice;
    public void Start()
    {
        explosice = GetComponent<Explosive>();

    }

    public void OnDeath(float remainingHealth)
    {
        explosice.Explode();
    }
    
}
