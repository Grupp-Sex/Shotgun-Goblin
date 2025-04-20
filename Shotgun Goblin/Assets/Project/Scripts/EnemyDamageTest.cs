using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTest : MonobehaviorScript_ToggleLog, IOnEnemyHit
{
    public void OnAttackHit(IDamageAbleByEnemy hitObject, float damage)
    {
        DebugLog("Damage Delt: " + damage);
        hitObject.TakeDamage(GetDamageInfo(hitObject, damage));
    }

    protected DamageInfo GetDamageInfo(IDamageAbleByEnemy hitObject, float damage)
    {
        Vector3 position = hitObject.GetTransform().GetComponent<Collider>().ClosestPoint(transform.position);

        return new DamageInfo { damage = damage, position = position };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
