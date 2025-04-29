using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionControledDash))]
public class ShotgunJump : MonobehaviorScript_ToggleLog, IShotActivated, IUserReference
{
    [SerializeField] DirectionControledDash DirectionControledDash;
    [SerializeField] float maximumAngle;

    protected PlayerCam cam;
    protected PlayerMovement movement;
    public void SetUser(GameObject user)
    {
        DirectionControledDash.SetMovement(user);
        DirectionControledDash.SetRigidbody(user.GetComponent<Rigidbody>());
        cam = user.GetComponent<PlayerCam>();
        movement = user.GetComponent<PlayerMovement>();
        
    }

    public void RunShootLogic()
    {
        DebugLog("Shotgun Jump: angle = " + -cam.xRotation);
        if (-cam.xRotation < maximumAngle && movement.grounded)
        {
            DirectionControledDash.Dash();
            
        }
    }

    public void RunProjectileLogic(ProjectileInfo projectile)
    {

    }
}
