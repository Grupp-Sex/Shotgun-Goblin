using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ActivateExplosiveOnEnemyAttack : MonoBehaviour, IOnEnemyHit
{
    [SerializeField] HealthManager Explosive;
    [SerializeField] float DamageToExplosive;
    protected Unity.Mathematics.Random RD;

    void Start()
    {
        RD.InitState();
    }

    public void OnAttackHit(IDamageAbleByEnemy target, float damage)
    {

        
        Vector3 position = transform.position + (Vector3)RD.NextFloat3();

        Vector3 direction = Explosive.transform.position - position;

        Explosive.Damage(new DamageInfo() { damage = DamageToExplosive, position = position, direction = direction });
    }
}
