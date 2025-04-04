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

    public void TakeDamage(float Damage)
    {
        Debug.Log(Damage);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
