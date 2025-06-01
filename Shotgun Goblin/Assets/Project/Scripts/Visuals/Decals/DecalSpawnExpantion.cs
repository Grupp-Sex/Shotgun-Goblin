using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalSpawnExpantion : DecalEffectOverTime<Vector3>
{
    

    [SerializeField] float startMult;

    protected override void Initialize()
    {
        StartEffect();
    }


    protected override void ApplyEffect(DecalHolder decal, float lerpValue)
    {
        decal.decal.size = Lerp(decal.startSize * startMult, decal.startSize, lerpValue);
        
    }

    protected override void EndEffect()
    {
        
    }

    protected override Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a * t + b * (1 - t);
    }

}
