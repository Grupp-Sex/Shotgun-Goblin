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
    
    // Start is called before the first frame update
    void Start()
    {
        if(Despawner == null)
            Despawner = GetComponent<ObjectDespawner>();
        
        if(Rigidbody == null)
            Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartChecks();
    }

    private void OnDisable()
    {
        EndChecks();
    }

    public void Freze()
    {
        IsFrozen = true;
    }

    public void Thaw()
    {
        IsFrozen = false;
    }

    protected bool CheckIfThawed()
    {
        return !IsFrozen;
    }
    

    protected void StartDespawn()
    {
        Despawner?.StartDespawnTimer(DespawnTimerMinutes);
    }

    protected void EndDespawn()
    {
        Despawner?.AbortDespawnTimer();
    }

    protected void StartChecks()
    {
        if (Application.isPlaying)
        {
            StartCoroutine(WaitUntillSleeping(DespawnCheckIntervall));
        }
    }

    protected void EndChecks()
    {
        StopAllCoroutines();
    }

    protected IEnumerator WaitUntillSleeping(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return new WaitUntil(CheckIfThawed);
            yield return new WaitUntil(Rigidbody.IsSleeping);
            yield return new WaitUntil(Despawner.DespawnTimerFree);


            if (CheckIfThawed() && Rigidbody.IsSleeping())
            {
                StartDespawn();

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
