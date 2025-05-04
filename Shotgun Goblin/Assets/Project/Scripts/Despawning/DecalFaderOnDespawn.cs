using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalFaderOnDespawn : EffectOverDuration<float>
{
    

    [SerializeField] bool DespawnAfterFade;

   

    public DecalProjector Decal;

    public ObjectDespawner Despawner;


    private void Start()
    {
        if(Decal == null)
        {
            Decal = GetComponent<DecalProjector>();
        }

        SetValues(Decal.fadeFactor, 0);

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

    

    protected override void ApplyEffect(float interpolationValue)
    {

        Decal.fadeFactor = interpolationValue;

    }

    protected override void EndEffect()
    {
        if (DespawnAfterFade)
        {
            Destroy(Decal.gameObject);
        }
    }

    protected override float Lerp(float a, float b, float t) 
    {
        return a * t + b * (1-t);
    }
}
