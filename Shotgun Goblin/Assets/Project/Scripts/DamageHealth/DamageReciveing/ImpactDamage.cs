using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactDamage : MonobehaviorScript_ToggleLog, IImpactThreshold
{
    [SerializeField] float LowImpactDamage;
    [SerializeField] float HighImpactDamage;

    private HealthManager healthManager;

    private void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    public void SoftImpact(CollisionData collision)
    {
        healthManager.Damage(LowImpactDamage, collision.position);
    }

    public void HardImpact(CollisionData collision)
    {
        healthManager.Damage(HighImpactDamage, collision.position);
    }
}
