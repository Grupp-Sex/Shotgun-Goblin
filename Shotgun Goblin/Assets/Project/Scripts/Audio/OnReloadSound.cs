using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReloadSound : GenralSoundPlayer, IShotActivated
{
    [SerializeField] float OffsetTimer = 1f;
    public void RunShootLogic()
    {

    }   

    public void RunProjectileLogic(ProjectileInfo projectile)
    {

    }

    public void RunReloadLogic()
    {
        StartCoroutine(SoundsTimer(OffsetTimer));
    }

    protected IEnumerator SoundsTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySounds(sounds);
    }
}
