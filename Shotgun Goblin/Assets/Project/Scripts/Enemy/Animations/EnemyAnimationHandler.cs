/*
 * EnemyAnimationHandler.cs
 * 
 * Handles animation state changes for enemy characters.
 * Acts as an interface between enemy AI or logic and the Animator component.
 * 
 * Features:
 * - Sets "IsRunning" boolean in Animator to trigger running animation
 * - Triggers "Attack" animation using a trigger parameter
 * 
 * Usage:
 * - Call SetRunning(true/false) to toggle running animation based on movement logic
 * - Call PlayAttack() when the enemy initiates an attack (e.g., via AI behavior tree or script)
 * 
 * Dependencies:
 * - Requires an Animator component on the same GameObject
 * - Animator should have:
 *     - A bool parameter named "IsRunning"
 *     - A trigger parameter named "Attack"
 * 
 * Author:
 * - Written by Mikael
 */


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
        //Debug.Log("SetRunning Called: " +  isRunning);
        animator.SetBool("IsRunning", isRunning);
    }

    public void PlayAttack()
    {
        //Debug.Log("PlayAttack Triggered");
        animator.SetTrigger("Attack");
    }
    
   
}
