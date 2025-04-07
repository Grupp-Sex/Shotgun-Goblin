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

    public virtual void Damage(float damage)
    {
        
        if(dead || !isActiveAndEnabled) return;
        
        NotifyDamage(damage);

        currentHealth -= damage;
        DebugLog("Damage Taken: " + damage + " health: " + currentHealth + "/" + maxHealth );
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

        NotifyDeath();
    }

    protected virtual void NotifyDeath()
    {
        for(int i = 0; i < deathActivatedScripts?.Length; i++)
        {
            deathActivatedScripts[i].OnDeath(currentHealth);
        }
    }

    protected virtual void NotifyDamage(float damage)
    {
        for (int i = 0; i < damageActivatedScripts?.Length; i++)
        {
            damageActivatedScripts[i].OnDamage(damage);
        }
    }
}

public interface IDeathActivated
{
    public void OnDeath(float negativeHealthRemaining);

}

public interface IDamageActivated
{
    public void OnDamage(float damage);

}
