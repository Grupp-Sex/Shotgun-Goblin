using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAnimations : MonoBehaviour, IShotActivated
{

    public Animator animator;

    [SerializeField] private string trigger = "RECOIL";

    private void Awake()
    {
        if (animator == null)
        animator = GetComponent<Animator>();
    }


    public void RunShootLogic()
    {
        //Animation trigger when firing gun
        animator.SetTrigger(trigger);
    }

    public void RunProjectileLogic(ProjectileInfo projectile)
    {

    }
}
