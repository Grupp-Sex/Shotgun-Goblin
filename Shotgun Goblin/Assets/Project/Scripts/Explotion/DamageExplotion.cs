using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageExplotion : MonoBehaviour, IOnExplotionInRadius
{
    [SerializeField] float DPS;
    public void OnExplode(Collider target, float effect)
    {
        HealthManager targetHP = target.GetComponent<HealthManager>();

        targetHP.Damage(DPS * effect * Time.deltaTime, target.ClosestPoint(transform.position));
    }
}
