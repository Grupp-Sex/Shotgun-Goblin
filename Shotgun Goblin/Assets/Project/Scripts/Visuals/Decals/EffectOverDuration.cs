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

        DoEndEffect();

    }


    protected virtual void InterploateEffect(float interpolationValue)
    {
        float lerpValue = EvaluateCurve(interpolationValue);

        DebugLog("interpolation: " + interpolationValue + ", current value: " + lerpValue);

        ApplyEffect(lerpValue);
        
    }



    protected virtual float EvaluateCurve(float interplation)
    {
        return EffectCurve.Evaluate(1 - interplation);
    }

    

    protected virtual void DoEndEffect()
    {
        ApplyEffect(EvaluateCurve(1));
        EndEffect();

        StopAllCoroutines();
    }
    
    protected abstract void ApplyEffect(float interploationValue);

    protected abstract void EndEffect();

    protected abstract T Lerp(T a, T b, float t);
}

