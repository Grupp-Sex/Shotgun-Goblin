using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDamageReciever : MonoBehaviour, IShootAble
{
    protected HealthManager healthManager;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    public void GotShotLogic(ProjectileInfo projectile)
    {
        healthManager.Damage(new DamageInfo 
        {
            damage = projectile.damage, position = projectile.hitPos, 
            hasDirection = true,  direction = projectile.direction
        });
    }
}
