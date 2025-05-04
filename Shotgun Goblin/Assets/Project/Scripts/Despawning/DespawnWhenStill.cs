using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DespawnWhenStill : MonobehaviorScript_ToggleLog, IFrozenOnFractionFreeze
{
    public bool IsFrozen { get; set; }

    public ObjectDespawner Despawner;
    public Rigidbody Rigidbody;
    [SerializeField] bool PauseOnFreeze = true;
    [SerializeField] float DespawnTimerMinutes; 
    [SerializeField] float DespawnCheckIntervall;

    [SerializeField] bool isChecking;

    // Start is called before the first frame update
    void Start()
    {
        if(Despawner == null)
            Despawner = GetComponent<ObjectDespawner>();
        
        if(Rigidbody == null)
            Rigidbody = GetComponent<Rigidbody>();

        if (!PauseOnFreeze)
            StartChecks();
    }

    



    public void Freze()
    {
        IsFrozen = true;
    }

    public void Thaw()
    {
        IsFrozen = false;
        if (PauseOnFreeze && !isChecking)
            StartChecks();


    }

    protected bool CheckIfThawed()
    {
        return !IsFrozen || !PauseOnFreeze;
    }
    

    protected void StartDespawn()
    {
        Despawner?.StartDespawnTimer(this, DespawnTimerMinutes);
    }

    protected void EndDespawn()
    {
        Despawner?.AbortDespawnTimer();
    }

    protected void StartChecks()
    {
        if (Application.isPlaying && !isChecking)
        {
            DebugLog("Despawn Checking Started");
            StartCoroutine(WaitUntillSleeping(DespawnCheckIntervall));
        }
    }

    protected void EndChecks()
    {
        DebugLog("Despawn Checking Ended");
        isChecking = false;
        StopAllCoroutines();
    }

    protected IEnumerator WaitUntillSleeping(float interval)
    {
        isChecking = true;
        while (isChecking)
        {
            DebugLog("Despawn While Still, Wait for " + interval);
            yield return new WaitForSeconds(interval);

            DebugLog("Despawn While Still, Wait for if thawed");
            yield return new WaitUntil(CheckIfThawed);

            DebugLog("Despawn While Still, Wait for is still");
            yield return new WaitUntil(Rigidbody.IsSleeping);

            DebugLog("Despawn While Still, Wait for if despawn queue");
            yield return new WaitUntil(Despawner.DespawnTimerFree);


            if (CheckIfThawed() && Rigidbody.IsSleeping())
            {
                StartDespawn();

                DebugLog("Despawn While Still, Wait for if not still");
                yield return new WaitUntil(CheckNotIfSleeping);

                
                EndDespawn();
                
            }
            
        }
    }
    
    

    protected bool CheckNotIfSleeping()
    {
        return !Rigidbody.IsSleeping();
    }

    

}
