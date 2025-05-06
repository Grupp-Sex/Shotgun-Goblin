using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalFaderOnDespawn : DecalEffectOverTime<float>
{
    

    [SerializeField] bool DespawnAfterFade;
    [SerializeField] float EndMult;

    public ObjectDespawner Despawner;


    

    protected override void Initialize()
    {
        if (Despawner == null)
        {
            Despawner = GetComponent<ObjectDespawner>();
        }

        Despawner.Event_Despawn.Subscribe(Event_Despawn);
    }


    protected void Event_Despawn(object sender, object args)
    {
        StartEffect();
        
    }

    

    protected override void ApplyEffect(DecalHolder decal, float interpolationValue)
    {
        decal.decal.fadeFactor = Lerp(decal.startOppacity, decal.startOppacity * EndMult, interpolationValue);

        
        DebugLog("after: " + decal.decal.fadeFactor.ToString());
    }

    protected override void EndEffect()
    {
        if (DespawnAfterFade)
        {
            Destroy(gameObject);
        }
    }

    protected override float Lerp(float a, float b, float t) 
    {
        return a * t + b * (1-t);
    }
}
