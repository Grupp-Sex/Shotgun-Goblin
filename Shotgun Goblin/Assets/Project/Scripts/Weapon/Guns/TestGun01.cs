using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun01 : BaseGun
{

    protected override void Shoot()
    {
        base.Shoot();

        ShootOneTime(transform.position, transform.up,100);
    }

}
