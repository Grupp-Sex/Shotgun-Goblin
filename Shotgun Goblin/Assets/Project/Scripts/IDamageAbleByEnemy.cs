using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAbleByEnemy
{
    void TakeDamage(float Damage);

    Transform GetTransform();
}
