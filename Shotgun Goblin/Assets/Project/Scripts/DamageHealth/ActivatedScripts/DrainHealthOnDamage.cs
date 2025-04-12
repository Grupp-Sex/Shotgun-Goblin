using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainHealthOnDamage : MonobehaviorScript_ToggleLog, IDamageActivated, IDeathActivated
{
    [SerializeField] float intervalTime;
    [SerializeField] float damage;
    [SerializeField] float duration;

    protected HealthManager healthManager;

    public static string DamageTag = "DrainHealthOverTime";

    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void OnDamage(DamageInfo damageInfo)
    {
        if (isActiveAndEnabled && damageInfo.DamageTag != DamageTag)
        {
            StartCoroutine(DamageOverTime(intervalTime, damage, duration, damageInfo));
        }
    }

    public void OnDeath(DamageInfo damageInfo)
    {
        StopAllCoroutines();
    }

    protected IEnumerator DamageOverTime(float interval, float damage, float duration, DamageInfo originalDamage)
    {
        IEnumerator dotLoop = DamageIntervalLoop(interval, damage, originalDamage);

        StartCoroutine(dotLoop);

        yield return new WaitForSeconds(duration);

        StopCoroutine(dotLoop);
    }

    protected IEnumerator DamageIntervalLoop(float interval, float damage, DamageInfo originalDamage)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            DoDamage(damage, originalDamage);
        }
    }

   

    protected void DoDamage(float damage, DamageInfo originalDamage)
    {
        healthManager.Damage(new DamageInfo 
        {
            damage = damage,
            position = originalDamage.position,
            hasDirection = false,
            direction = originalDamage.direction,
            DamageTag = DamageTag
        });
    }
}
