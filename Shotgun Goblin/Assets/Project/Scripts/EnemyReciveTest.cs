using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReciveTest : MonoBehaviour, IDamageAbleByEnemy
{
    private float Health = 300;

    public Transform GetTransform()
    {
        return transform;
    }

    public void TakeDamage(DamageInfo Damage)
    {
        Debug.Log(Damage.damage);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
