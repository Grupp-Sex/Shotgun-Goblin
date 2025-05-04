using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalFaderOnDespawn : MonobehaviorScript_ToggleLog
{
    [SerializeField] float FadeTime;

    [SerializeField] bool DespawnAfterFade;

    [SerializeField] AnimationCurve FadeCurve; 

    public DecalProjector Decal;

    public ObjectDespawner Despawner;

    protected float oppacity;

    protected float maxOppacity;

    

    private void Start()
    {
        if(Decal == null)
        {
            Decal = GetComponent<DecalProjector>();
        }

        maxOppacity = Decal.fadeFactor;
        oppacity = maxOppacity;

        if(Despawner == null)
        {
            Despawner = GetComponent<ObjectDespawner>();
        }

        Despawner.Event_Despawn.Subscribe(Event_Despawn);
    }


    protected void Event_Despawn(object sender, object args)
    {
        StartCoroutine(FadeDecalInterval(FadeTime));
        
    }

    protected IEnumerator FadeDecalInterval(float fadeTimer)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        long fadeTimerMS = (long)(fadeTimer * 1000f);

        //DebugLog(fadeTimerMS.ToString());


        for (long i = 0; i < fadeTimerMS; i = stopwatch.ElapsedMilliseconds)
        {
            

            yield return new WaitForFixedUpdate();
            float interpolationValue = (float)i / (float)fadeTimerMS;

           
            FadeDecal(interpolationValue);
        }


    }

    protected void FadeDecal(float interpolationValue)
    {
        float lerpValue = FadeCurve.Evaluate(1 - interpolationValue);
        
        

        oppacity = Lerp(maxOppacity, 0, lerpValue);

        DebugLog("Decal Fade lerpvalue: " + interpolationValue + ", oppacity: " + oppacity);

        if (oppacity > 0.001f)
        {
            Decal.fadeFactor = lerpValue;
        }
        else
        {
            if (DespawnAfterFade)
            {

                Despawner.Despawn(this, ObjectDespawner.DespawnType.Destroy);
            }

            StopAllCoroutines();
        }

    }

    protected float Lerp(float a, float b, float t)
    {
        return a * t + b * (1- t);
    }

}
