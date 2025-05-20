using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionControledDash))]
public class ShotgunJump : MonobehaviorScript_ToggleLog, IShotActivated, IUserReference
{
    [SerializeField] DirectionControledDash DirectionControledDash;
    [SerializeField] float maximumAngle;
    [SerializeField] private int dubbleJumpCount;
    public int currentDubbleJumpCounter;

    protected PlayerCam cam;
    protected PlayerMovement movement;
    public void SetUser(GameObject user)
    {
        DirectionControledDash.SetMovement(user);
        DirectionControledDash.SetRigidbody(user.GetComponent<Rigidbody>());
        cam = user.GetComponent<PlayerCam>();
        movement = user.GetComponent<PlayerMovement>();
        
    }

    private void OnEnable()
    {
        StartCoroutine(GroundCeck());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected IEnumerator GroundCeck()
    {
        while (true)
        {
            yield return new WaitUntil(IsGrounded);
            ResetDubbleJump();
        }
    }

    protected void ResetDubbleJump()
    {
        DebugLog("ResetDubbleJump");
        currentDubbleJumpCounter = dubbleJumpCount;
    }

    protected bool IsGrounded()
    {
        
        return movement != null && movement.grounded;
    }

    protected bool CanDash()
    {
        return -cam.xRotation < maximumAngle && currentDubbleJumpCounter > 0;
    }

    

    public void RunShootLogic()
    {
        DebugLog("Shotgun Jump: angle = " + -cam.xRotation);
        if (CanDash())
        {
            DirectionControledDash.Dash();
            currentDubbleJumpCounter--;
        }
    }

    public void RunProjectileLogic(ProjectileInfo projectile)
    {

    }

    public void RunReloadLogic()
    {
       
    }
}
