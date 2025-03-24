using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun01 : BaseGun
{
    [SerializeField] int pelletsPerShot;
    [SerializeField] float randomScale;
    
    protected override void Shoot()
    {

        ShootOneTime(transform.position, ToForward(new Vector3(0, 1, 0)), 100);
        for (int i = 0; i < pelletsPerShot; i++)
        {
            ShootOneTime(transform.position, GetRandomDirection(randomScale), 100);
        }
    }

    protected Vector3 GetRandomDirection(float scale)
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * scale;

        Vector3 localDirection = new Vector3(offset.x, 1, offset.y);

        return ToForward(localDirection);
        
    }

    protected Vector3 ToForward(Vector3 direction)
    {
        return transform.TransformVector(direction);
    }
}
