using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDespawner : MonobehaviorScript_ToggleLog
{
    [SerializeField] bool CanDespawn = true;

    [SerializeField] DespawnType despawnType;

    [SerializeField] bool isDespawning;


    private void OnDisable()
    {
        AbortDespawnTimer();
    }

    private void OnValidate()
    {
        if(Application.isPlaying && isActiveAndEnabled)
        {
            if (!CanDespawn)
            {
                AbortDespawnTimer();
            }
        }
    }

    public bool DespawnTimerFree()
    {
        return !isDespawning && CanDespawn;
    }


    public void Despawn()
    {

        StopAllCoroutines();

        DebugLog("Object Despawned");

        switch (despawnType)
        {
            case DespawnType.Dissable:

                gameObject.SetActive(false);

                break;
        }
    }

    public void StartDespawnTimer(float timer)
    {
        if (!isDespawning)
        {

            StartCoroutine(DespawnTimer(timer));
            
        }
    }

    protected IEnumerator DespawnTimer(float time)
    {
        float timeInSeconds = time * 60;

        
        OnTimerStart(timeInSeconds);
        yield return new WaitForSeconds(timeInSeconds);

        OnTimerEnd();
        Despawn();
        
        
            
    }

    protected virtual void OnTimerStart(float time)
    {
        isDespawning = true;
        DebugLog("DespanwTimer Started, Length " + time);
    }

    protected void OnTimerEnd()
    {
        isDespawning = false;
        DebugLog("DespanwTimer Ended");

    }

    public void AbortDespawnTimer()
    {
        if (isDespawning)
        {
            DebugLog("Abort Despawn");
            StopAllCoroutines();
        
            OnTimerEnd();
        }
    }

    

    public enum DespawnType
    {
        Dissable
    }
    
}
