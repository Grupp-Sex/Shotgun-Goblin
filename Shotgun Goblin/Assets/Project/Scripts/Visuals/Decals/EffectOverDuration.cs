using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class EffectOverDuration<T> : MonobehaviorScript_ToggleLog
{
    [SerializeField] float EffectDuration;

    [SerializeField] AnimationCurve EffectCurve;

    

    protected T currentValue;

    protected T maxValue;

    protected T minValue;

    protected virtual void SetValues(T max, T min)
    {
        maxValue = max;
        currentValue = max;

        minValue = min;
    }

    
    protected virtual void StartEffect()
    {
        StartCoroutine(RunEffectOverDuration(EffectDuration));
    }

    protected IEnumerator RunEffectOverDuration(float effectTimer)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        long effectTimerMS = (long)(effectTimer * 1000f);

        //DebugLog(fadeTimerMS.ToString());


        for (long i = 0; i < effectTimerMS; i = stopwatch.ElapsedMilliseconds)
        {


            float interpolationValue = (float)i / (float)effectTimerMS;

            InterploateEffect(interpolationValue);

            yield return new WaitForFixedUpdate();
        }


    }

    

    protected virtual void InterploateEffect(float interpolationValue)
    {
        float lerpValue = EffectCurve.Evaluate(1 - interpolationValue);


        currentValue = Lerp(maxValue, minValue, lerpValue);

        DebugLog("interpolation: " + lerpValue + ", current value: " + currentValue);

        
        ApplyEffect(currentValue);
        
        if (lerpValue <= 0.001f)
        {

            ApplyEffect(minValue);
            EndEffect();

            StopAllCoroutines();
        }

    }

    protected abstract void ApplyEffect(T value);

    protected abstract void EndEffect();

    protected abstract T Lerp(T a, T b, float t);
}
