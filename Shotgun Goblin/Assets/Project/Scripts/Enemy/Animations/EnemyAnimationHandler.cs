using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Written by Mikael
public class EnemyAnimationHandler : MonoBehaviour
{

    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void SetRunning(bool isRunning)
    {
        Debug.Log("SetRunning Called: " +  isRunning);
        animator.SetBool("IsRunning", isRunning);
    }

    public void PlayAttack()
    {
        Debug.Log("PlayAttack Triggered");
        animator.SetTrigger("Attack");
    }
    
   
}
