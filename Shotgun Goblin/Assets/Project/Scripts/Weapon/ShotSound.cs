using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSound : GenralSoundPlayer, IShotActivated
{
   
    public void RunShootLogic()
    {
        PlaySounds(sounds);
    }

    
    public void RunReloadLogic()
    {

    }

    public void RunProjectileLogic(ProjectileInfo projectile)
    {

    }
}
