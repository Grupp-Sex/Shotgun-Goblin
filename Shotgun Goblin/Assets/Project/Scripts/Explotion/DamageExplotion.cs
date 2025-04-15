using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageExplotion : MonoBehaviour, IOnExplotionInRadius
{
    [SerializeField] float DPS;
    public void OnExplode(Collider target, float effect)
    {
        HealthManager targetHP = target.GetComponent<HealthManager>();

        if (targetHP != null)
        {

            targetHP.Damage( new DamageInfo 
                    { 
                        damage = DPS * effect * Time.deltaTime,
                        position = target.ClosestPoint(transform.position),
                        NoEffects = true,

                    });
        }
    }
}
