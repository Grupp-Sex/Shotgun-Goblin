/*
 * ShotgunAnimationHandler.cs
 * 
 * Handles animation logic for the shotgun weapon by responding to shooting and reloading events.
 * Implements the IShotActivated interface, allowing it to be notified when the gun is fired or reloaded.
 * 
 * Features:
 * - Triggers recoil and reload animations using Animator parameters.
 * 
 * Integration:
 * - The Animator component should be assigned or found on the same GameObject.
 * - Requires appropriate trigger parameters (default: "RECOIL", "RELOAD") to be set in the Animator.
 * - This script is automatically notified by the BaseGun system through the IShotActivated interface.
 * 
 * Author:
 * - Written by Mikael
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAnimationHandler : MonoBehaviour, IShotActivated
{

    public Animator animator;
    private BaseGun baseGun;

    [SerializeField] private string shootTrigger = "RECOIL";
    [SerializeField] private string reloadTrigger = "RELOAD";

    private void Awake()
    {
        if (animator == null)
        animator = GetComponent<Animator>();

        BaseGun baseGun = GetComponent<BaseGun>();
    }


    public void RunShootLogic()
    {
        //Animation trigger when firing gun
        animator.SetTrigger(shootTrigger);
    }

    public void RunReloadLogic()
    {
        //Animation trigger reload
        animator.SetTrigger(reloadTrigger);
    }

    public void RunProjectileLogic(ProjectileInfo projectile)
    {

    }





    /*When Reload() is called in base BaseGun, NotifyReloadActivated() runs.That calls RunReloadLogic() on all scripts that implement the interface IShotActivated(which means ShotgunAnimator).
    The animator receives the "RELOAD" trigger and plays the reload animation.This is at least how I understand it /Mikey */


}
