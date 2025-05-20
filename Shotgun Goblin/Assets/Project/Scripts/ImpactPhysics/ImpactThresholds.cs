using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactThresholds : MonobehaviorScript_ToggleLog, IImapctManagerNotify
{   
    public enum TypeOfImpactValue
    {
        Relative_KineticEneryg,
        Relative_Momentum,
        Collider_KineticEneryg,
        Collider_Momentum,

    }

    [SerializeField] TypeOfImpactValue SoftImpactType;
    [SerializeField] float SoftImpactThreshold;

    [SerializeField] TypeOfImpactValue HardImpactType;
    [SerializeField] float HardImpactThreshold;

    protected IImpactThreshold[] impactThresholdScripts;

    public void Start()
    {
        impactThresholdScripts = GetComponents<IImpactThreshold>();
    }

    protected virtual void SoftImpact(CollisionData collision)
    {
        DebugLog("Soft Impact, Kinetic Energy;" + collision.colider_kineticEnergy + ", Momentum: " + collision.colider_momentum);
        for (int i = 0; i < impactThresholdScripts.Length; i++)
        {
            impactThresholdScripts[i].SoftImpact(collision);
        }
    }

    protected virtual void HardImpact(CollisionData collision)
    {
        DebugLog("Hard Impact, Kinetic Energy;" + collision.colider_kineticEnergy + ", Momentum: " + collision.colider_momentum);
        for(int i = 0; i < impactThresholdScripts.Length; i++)
        {
            impactThresholdScripts[i].HardImpact(collision);
        }
    }

    public void OnNotifyedCollision(CollisionData collision)
    {

        if ((collision.kineticEnergy > HardImpactThreshold && HardImpactType == TypeOfImpactValue.Relative_KineticEneryg) ||
            (collision.momentum > HardImpactThreshold && HardImpactType == TypeOfImpactValue.Relative_Momentum) ||
            (collision.colider_kineticEnergy > HardImpactThreshold && HardImpactType == TypeOfImpactValue.Collider_KineticEneryg) ||
            (collision.colider_momentum > HardImpactThreshold && HardImpactType == TypeOfImpactValue.Collider_Momentum))
        {
            HardImpact(collision);
        }

        if ((collision.kineticEnergy > SoftImpactThreshold && SoftImpactType == TypeOfImpactValue.Relative_KineticEneryg) ||
            (collision.momentum > SoftImpactThreshold && SoftImpactType == TypeOfImpactValue.Relative_Momentum) ||
            (collision.colider_kineticEnergy > SoftImpactThreshold && SoftImpactType == TypeOfImpactValue.Collider_KineticEneryg) ||
            (collision.colider_momentum > SoftImpactThreshold && SoftImpactType == TypeOfImpactValue.Collider_Momentum))
        {
            SoftImpact(collision);
        }
        
    }



}

public interface IImpactThreshold
{
    public void SoftImpact(CollisionData collition);
    public void HardImpact(CollisionData collition);

}
