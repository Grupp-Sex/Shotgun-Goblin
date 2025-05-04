using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDespawner : MonobehaviorScript_ToggleLog
{
    [SerializeField] bool CanDespawn = true;

    [SerializeField] DespawnType despawnType;

    [SerializeField] bool isDespawning;

    public EventPusher<object> Event_Despawn = new EventPusher<object>();

    

    private void OnDisable()
    {
        AbortDespawnTimer();
    }

    private void OnDestroy()
    {
        Event_Despawn.UnSubscribeAll();
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


    public void Despawn(object sender, DespawnType type)
    {

        StopAllCoroutines();

        DebugLog("Object Despawned");

        switch (type)
        {
            case DespawnType.Dissable:

                gameObject.SetActive(false);

                break;


            case DespawnType.Event_Only or DespawnType.Dissable:

                Event_Despawn.Invoke(this, sender);

                break;

            case DespawnType.Destroy:

                Destroy(gameObject);

                break;
        }
    }

    public void StartDespawnTimer(object sender, float timer)
    {
        if (!isDespawning)
        {

            StartCoroutine(DespawnTimer(sender,timer));
            
        }
    }

    protected IEnumerator DespawnTimer(object sender, float time)
    {
        float timeInSeconds = time * 60;

        
        OnTimerStart(timeInSeconds);
        yield return new WaitForSeconds(timeInSeconds);

        OnTimerEnd();
        Despawn(sender, despawnType);
        
        
            
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
        Dissable,
        Event_Only,
        Destroy
    }
    
}
