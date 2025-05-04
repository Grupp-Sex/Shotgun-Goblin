using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class EffectOverDuration : MonobehaviorScript_ToggleLog
{
    [SerializeField] float EffectDuration;

    [SerializeField] AnimationCurve EffectCurve;

    

    protected float currentValue;

    protected float maxValue;

    protected abstract void SetMaxValue();
    
    


    protected IEnumerator RunEffectOverDuration(float effectTimer)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        long effectTimerMS = (long)(effectTimer * 1000f);

        //DebugLog(fadeTimerMS.ToString());


        for (long i = 0; i < effectTimerMS; i = stopwatch.ElapsedMilliseconds)
        {


            yield return new WaitForFixedUpdate();
            float interpolationValue = (float)i / (float)effectTimerMS;


            InterploateEffect(interpolationValue);
        }


    }

    

    protected virtual void InterploateEffect(float interpolationValue)
    {
        float lerpValue = EffectCurve.Evaluate(1 - interpolationValue);


        currentValue = Lerp(maxValue, 0, lerpValue);

        DebugLog("interpolation: " + interpolationValue + ", current value: " + currentValue);

        ApplyEffect(currentValue);

    }

    protected abstract void ApplyEffect(float value);

    protected float Lerp(float a, float b, float t)
    {
        return a * t + b * (1 - t);
    }
}
