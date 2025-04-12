using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpreadDamage : MonobehaviorScript_ToggleLog, IDamageActivated
{
    [SerializeField] List<string> TargetTags;
    [SerializeField] float DamageSpreadRadius;

    [SerializeField] float EpicenterDamageMult;
    [SerializeField] float EdgeDamageMult;

    public void OnDamage(DamageInfo damage)
    {
        if (!damage.IsSpread)
        {
            DamageNeighbours(damage, DamageSpreadRadius);
        }
    }

    protected void DamageNeighbours(DamageInfo damage, float radius)
    {


        Collider[] colidersInside = Physics.OverlapSphere(damage.position, radius);
        int counter = 0;
        for (int i = 0; i < colidersInside.Length; i++)
        {
            if (TargetTags.Contains(colidersInside[i].tag))
            {
                if (colidersInside[i].gameObject != gameObject) {
                    DebugLog(colidersInside[i].gameObject.name);

                    HealthManager fragment = colidersInside[i].gameObject.GetComponent<HealthManager>();

                    if (fragment != null)
                    {
                        counter++;
                        DebugLog("neighbour found");

                        fragment.Damage(SpreadDamageInfo(damage, colidersInside[i], radius));
                    }
                }
            }
        }
    }

    protected DamageInfo SpreadDamageInfo(DamageInfo inputDamage, Collider hitObject, float radius)
    {
        Vector3 closestPoint = hitObject.ClosestPoint(inputDamage.position);


        float distance = Vector3.Distance(inputDamage.position, closestPoint);

        return new DamageInfo
        {
            position = closestPoint,
            damage = CalculateDamage(inputDamage.damage, distance, radius),
            IsSpread = true
        };
    }

    protected float CalculateDamage(float baseDamage, float distance, float radius)
    {
        return baseDamage * Lerp(EpicenterDamageMult, EdgeDamageMult, distance / radius);
    }

    protected float Lerp(float a, float b, float t)
    {
        return a * t + b * (1 - t);
    }
}
