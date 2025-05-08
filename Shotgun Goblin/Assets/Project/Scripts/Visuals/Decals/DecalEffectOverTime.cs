using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecalEffectOverTime<T> : EffectOverDuration<T>
{
    public DecalManager Decals;

    private void Start()
    {
        if (Decals == null)
        {
            Decals = GetComponent<DecalManager>();
        }

        Initialize();
    }

    protected abstract void Initialize();
    


    protected override void ApplyEffect(float interploationValue)
    {
        DebugLog("interpolationValue: " + interploationValue);
        Decals.RunActionOnAllDecals(ApplyEffect, interploationValue);
    }

    protected abstract void ApplyEffect(DecalHolder decal, float interploationValue);
}
