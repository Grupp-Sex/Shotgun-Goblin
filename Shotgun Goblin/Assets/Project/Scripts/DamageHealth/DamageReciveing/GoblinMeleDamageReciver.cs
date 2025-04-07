using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMeleDamageReciver : MonobehaviorScript_ToggleLog, IDamageAbleByEnemy
{
    protected HealthManager healthManager;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }


    public void TakeDamage(float damage)
    {
        healthManager.Damage(damage);
    }

    public Transform GetTransform()
    {
        return transform;
    }
    
}
