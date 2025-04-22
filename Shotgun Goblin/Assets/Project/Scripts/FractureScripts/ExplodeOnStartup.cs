using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnStartup : MonobehaviorScript_ToggleLog
{
    [SerializeField] float explotionVelocity;
    [SerializeField] float angularVelocityMult;


    protected Rigidbody rb;

    private void Start()
    {

        
        rb = GetComponent<Rigidbody>();



        Explode();
        
    }

    protected void Explode()
    {
        Vector3 direction = DirectionFromCenter();

        rb.velocity = direction * explotionVelocity;
        //rb.AddExplosionForce(explotionForceMultiplyer, origin, Radius, 0, ForceMode.VelocityChange);
        rb.angularVelocity = rb.velocity * angularVelocityMult;

        DebugLog("exploded, velocity: " + rb.velocity + ", direction: " + direction);
    }

    protected Vector3 DirectionFromCenter()
    {
        

        Vector3 position = rb.worldCenterOfMass;

        return rb.centerOfMass.normalized;
    }


}
