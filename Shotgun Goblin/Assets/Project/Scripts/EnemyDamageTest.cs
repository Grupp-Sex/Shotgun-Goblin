using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTest : MonoBehaviour, IOnEnemyHit
{
    public void OnAttackHit(IDamageAbleByEnemy hitObject, float damage)
    {
        hitObject.TakeDamage(damage);
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
