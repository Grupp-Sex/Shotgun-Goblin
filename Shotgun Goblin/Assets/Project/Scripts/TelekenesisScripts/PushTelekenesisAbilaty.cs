using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTelekenesisAbilaty : BaseTelekenesisAbilaty
{
    [SerializeField] float Force;


    //between 0 and 1
    [SerializeField] float Spread;


    public override void OnThrowTelekenesisObject(TelekenesisPhysicsObject thrownObject)
    {
        base.OnThrowTelekenesisObject(thrownObject);

        PushObject(thrownObject);
    }


    protected void PushObject(TelekenesisPhysicsObject obj)
    {
        Vector3 direction = NormalDirection(obj.transform.position, LerpBetweenOrignAndPlayer(Spread));

        obj.Rigidbody.AddForce(direction * Force, ForceMode.VelocityChange);
        DebugLog("Pushed Object in direction: " + direction + " at a velocity change of " + Force);
    }


    protected Vector3 LerpBetweenOrignAndPlayer(float lervValue)
    {
        return transform.position * (1  - lervValue) + pickuppOriginPoint.position * lervValue;
    }

}
