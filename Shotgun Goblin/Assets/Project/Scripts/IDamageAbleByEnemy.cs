using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAbleByEnemy
{
    void TakeDamage(DamageInfo damage);

    Transform GetTransform();
}
