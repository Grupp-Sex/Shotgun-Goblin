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
        healthManager.Damage(projectile.damage, projectile.hitPos);
    }
}
