using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureExplodeOnDeath : FractureCustom, IDeathActivated
{
    

    public void OnDeath(DamageInfo damage)
    {
        FractureObject();
    }


}
