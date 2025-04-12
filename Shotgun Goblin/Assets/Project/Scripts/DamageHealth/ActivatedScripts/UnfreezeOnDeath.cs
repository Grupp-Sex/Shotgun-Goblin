using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfreezeOnDeath : MonoBehaviour, IDeathActivated
{
    
    public void OnDeath(DamageInfo damage)
    {
        FragmentFreeze fragment = GetComponent<FragmentFreeze>();

        if(fragment != null && fragment.IsFrozen)
        {
            fragment.Activate();
        }

    }
}
