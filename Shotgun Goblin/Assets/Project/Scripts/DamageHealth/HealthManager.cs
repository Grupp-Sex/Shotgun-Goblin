using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonobehaviorScript_ToggleLog
{
    [SerializeField] float maxHealth = 1;
    [SerializeField] float currentHealth;
    [SerializeField] public bool invincible;
    public float Health => currentHealth;
    public float MaxHealth => maxHealth;

    [SerializeField] public bool imortal;

    [SerializeField] bool kill;

    protected bool isDying;
    protected bool dead;
    protected IDeathActivated[] deathActivatedScripts;
    protected IDamageActivated[] damageActivatedScripts;

    public EventPusher<float> Event_HealthChanged = new EventPusher<float>();
    public EventPusher<DamageInfo> Event_Damage = new EventPusher<DamageInfo>();
    public EventPusher<DamageInfo> Event_Death = new EventPusher<DamageInfo>();


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
            //CheckHealth();

            if (kill)
            {
                kill = false;

                Death();
            }
        }
    }

    public virtual void Kill()
    {
        StartDeath();
    }

    public virtual void Heal(float health)
    {
        
        currentHealth += health;
        Event_HealthChanged.Invoke(this, health);


        CheckHealth();
    }

    public virtual void Damage(float damage, Vector3 position)
    {
        Damage(new DamageInfo { damage = damage, position = position } );   
    }

    public virtual void Damage(DamageInfo damageInfo)
    {
        if (dead || !isActiveAndEnabled || invincible) return;

        float damage = damageInfo.damage;

        lastHit = damageInfo;

        if (!damageInfo.NoEffects)
        {
            NotifyDamage(damageInfo);
        }

        currentHealth -= damage;
        Event_Damage.Invoke(this, damageInfo);
        Event_HealthChanged.Invoke(this,-damage);
        DebugLog("Damage Taken: " + damage + " health: " + currentHealth + "/" + maxHealth);
        CheckHealth();


    }

    
    protected virtual void CheckHealth()
    {
        if(currentHealth >= maxHealth) currentHealth = maxHealth;

        if (!imortal && currentHealth <= 0 && !isDying)
        {
            StartDeath();
        }
    }

    public virtual void StartDeath()
    {
        StartCoroutine(DeathTimer());

        
    }

    protected IEnumerator DeathTimer()
    {
        isDying = true;

        yield return new WaitForEndOfFrame();

        Death();
    }
    protected virtual void Death()
    {
        dead = true;

        DebugLog("object has perrished, remaining health: " + currentHealth);

        NotifyDeath(lastHit);
    }

    

    protected virtual void NotifyDeath(DamageInfo damage)
    {
        Event_Death.Invoke(this, damage);

        for (int i = 0; i < deathActivatedScripts?.Length; i++)
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

    public string DamageTag;

    public bool NoEffects;
}

public interface IDeathActivated
{
    public void OnDeath(DamageInfo damage);

}

public interface IDamageActivated
{
    public void OnDamage(DamageInfo damage);

}
