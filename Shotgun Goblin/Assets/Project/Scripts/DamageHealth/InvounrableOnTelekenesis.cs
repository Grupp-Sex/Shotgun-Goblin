using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvounrableOnTelekenesis : MonoBehaviour, IOnTelekenesisEnter, IOnTelekenesisLeave
{
    [SerializeField] bool Timer;
    [SerializeField] float Duration;

    protected HealthManager healthManager;

    protected IEnumerator invincibleTimer;


    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    private void OnDisable()
    {
        StopInvincibleTimer();
    }

    public void OnTelekenesisEnter()
    {
        EnableInvincibility();
        StartInvincibleTimer();
    }

    public void OnTelekenesisLeave()
    {
        DisableInvincibility();
        StopInvincibleTimer();
    }

    protected void EnableInvincibility()
    {
        healthManager.invincible = true;
    }

    protected void DisableInvincibility()
    {
        healthManager.invincible = false;
    }

    protected void StopInvincibleTimer()
    {
        if (invincibleTimer != null)
        {
            StopCoroutine(invincibleTimer);
        }
    }

    protected void StartInvincibleTimer()
    {
        if (this.Timer && invincibleTimer == null)
        {
            invincibleTimer = InvincibleTimer(Duration);

            StartCoroutine(invincibleTimer);
        }
    }

    protected IEnumerator InvincibleTimer(float Duration)
    {
        yield return new WaitForSeconds(Duration);

        DisableInvincibility();

        invincibleTimer = null;
    }

    

}
