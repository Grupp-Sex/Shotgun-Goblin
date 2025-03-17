using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTeleknesisAbilaty : BaseTelekenesisAbilaty
{
    [SerializeField] float Force;
    [SerializeField] float Radius;


    public override void OnGrabTelekenesisObject(TelekenesisPhysicsObject grabedObjec)
    {
        base.OnGrabTelekenesisObject(grabedObjec);

        GrabVortex(grabedObjec);
    }

    protected void GrabVortex(TelekenesisPhysicsObject obj)
    {
        obj.Rigidbody.AddExplosionForce(-Force, pickuppOriginPoint.position, Radius);

    }
}
