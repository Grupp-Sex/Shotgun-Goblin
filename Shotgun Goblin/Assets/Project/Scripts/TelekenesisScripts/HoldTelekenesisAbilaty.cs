using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class HoldTelekenesisAbilaty : BaseTelekenesisAbilaty
{
    [SerializeField] Collider Shape;
    [SerializeField] float ImplotionForce;
    [SerializeField] float EpicenterForceMod;
    [SerializeField] float Radius;
    [SerializeField] float Upmod;


    void Start()
    {

    }
    protected override void OnUpdate()
    {
        base.OnUpdate();

        UpdateShape();
        RunFuncOnAllHeldObjects(HoldTelekenesis);

    }

    protected void UpdateShape()
    {
        Shape.transform.position = pickuppOriginPoint.position;
    }

    public override void OnGrabTelekenesisObject(TelekenesisPhysicsObject grabedObjec)
    {
        base.OnGrabTelekenesisObject(grabedObjec);
        AplyZeroG(grabedObjec);
    }

    protected void AplyZeroG(TelekenesisPhysicsObject obj)
    {
        obj.Rigidbody.angularDrag = 20;
        obj.Rigidbody.drag = 10;
        obj.Rigidbody.useGravity = false;

        
    }

    protected Vector3 GetShapePoint(TelekenesisPhysicsObject obj)
    {
        return Shape.ClosestPoint(obj.transform.position);
    }


    protected void HoldTelekenesis(TelekenesisPhysicsObject obj)
    {
        PullObjectToEpicenter(obj);
        PushObjectsApart(obj);
    }

    protected void PullObjectToEpicenter(TelekenesisPhysicsObject obj)
    {
        //LerpPosToEpicenter(obj);
        obj.Rigidbody.AddExplosionForce(-ImplotionForce , GetShapePoint(obj), Radius, -Upmod);
        obj.Rigidbody.AddExplosionForce(-ImplotionForce * EpicenterForceMod, pickuppOriginPoint.position, Radius, -Upmod);
        //PushEdjgesOfObjectToFlatten(obj);


    }


    // not working
    protected void PushEdjgesOfObjectToFlatten(TelekenesisPhysicsObject obj)
    {
        Vector3 direction = NormalDirection(GetShapePoint(obj), obj.transform.position);// * TelekenesisManager.Vector3DistanceSquared(GetShapePoint(obj), obj.transform.position) / 0.4f;
        obj.Rigidbody.AddForceAtPosition(direction * ImplotionForce , obj.Rigidbody.ClosestPointOnBounds(transform.position));
    }

    protected void LerpPosToEpicenter(TelekenesisPhysicsObject obj)
    {
        float lerpV = 0.01f;

        obj.transform.position = obj.transform.position * lerpV + GetShapePoint(obj) * (1 - lerpV);
    }

    protected void PushObjectsApart(TelekenesisPhysicsObject obj)
    {
        for(int i = 0; i < heldObjects.Count; i++)
        {
            if(heldObjects[i] != obj)
            {
                obj.Rigidbody.AddExplosionForce(ImplotionForce * 0.2f, heldObjects[i].transform.position, 1f, 0);
            }
        }
    }



}


