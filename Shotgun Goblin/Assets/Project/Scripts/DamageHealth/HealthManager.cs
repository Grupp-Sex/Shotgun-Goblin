using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class HealthManager : MonobehaviorScript_ToggleLog
{
    [SerializeField] float maxHealth = 1;
    [SerializeField] float currentHealth;

    [SerializeField] bool imortal;

    protected bool dead;
    protected IDeathActivated[] deathActivatedScripts;
    protected IDamageActivated[] damageActivatedScripts;

    private bool gameIsOn;

    protected DamageInfo lastHit;

    // Start is called before the first frame update
    void Start()
    {
        
        gameIsOn = true;
        currentHealth = maxHealth;
        CheckHealth();

        deathActivatedScripts = GetComponents<IDeathActivated>();
        damageActivatedScripts = GetComponents<IDamageActivated>();
    }

    private void OnValidate()
    {
        if (isActiveAndEnabled && gameIsOn)
        {
            CheckHealth();
        }
    }

    
    public virtual void Damage(float damage, Vector3 position)
    {
        Damage(new DamageInfo { damage = damage, position = position } );
        
    }

    public virtual void Damage(DamageInfo damageInfo)
    {
        if (dead || !isActiveAndEnabled) return;

        float damage = damageInfo.damage;

        lastHit = damageInfo;

        NotifyDamage(damageInfo);

        currentHealth -= damage;
        DebugLog("Damage Taken: " + damage + " health: " + currentHealth + "/" + maxHealth);
        CheckHealth();


    }

    
    protected virtual void CheckHealth()
    {
        if (!imortal && currentHealth <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        dead = true;

        DebugLog("object has perrished, remaining health: " + currentHealth);

        NotifyDeath(lastHit);
    }

    protected virtual void NotifyDeath(DamageInfo damage)
    {
        for(int i = 0; i < deathActivatedScripts?.Length; i++)
        {
            deathActivatedScripts[i].OnDeath(damage);
        }
    }

    protected virtual void NotifyDamage(DamageInfo damage)
    {
        for (int i = 0; i < damageActivatedScripts?.Length; i++)
        {
            damageActivatedScripts[i].OnDamage(damage);
        }
    }
}

public struct DamageInfo
{
    public float damage;
    public Vector3 position;

    public bool hasDirection;
    public Vector3 direction;

    public bool IsSpread;

}

public interface IDeathActivated
{
    public void OnDeath(DamageInfo damage);

}

public interface IDamageActivated
{
    public void OnDamage(DamageInfo damage);

}
