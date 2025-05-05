using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalSpawnExpantion : EffectOverDuration<Vector3>
{
    public DecalProjector Decal;

    [SerializeField] float startMult;

    private void Start()
    {
        if(Decal == null)
        {
            Decal = GetComponent<DecalProjector>();
        }

        SetValues(Decal.size * startMult, Decal.size);

        StartEffect();
    }



    protected override void ApplyEffect(Vector3 value)
    {
        Decal.size = value;
    }

    protected override void EndEffect()
    {
        
    }

    protected override Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a * t + b * (1 - t);
    }

}
