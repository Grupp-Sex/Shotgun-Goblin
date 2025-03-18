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
    [SerializeField] float SizeForceMod;
    [SerializeField] float Upmod;
    [SerializeField] Vector3 ParentMovement => newParentPosition - oldParentPosition;

    protected Vector3 oldParentPosition;
    protected Vector3 newParentPosition;

    void Start()
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        UpdateShape();
        RunFuncOnAllHeldObjects(HoldTelekenesis);

    }

    public void FixedUpdate()
    {
        //FixedUpdateActions();
    }

    protected virtual void FixedUpdateActions()
    {
        UpdateParentMovement();

        RunFuncOnAllHeldObjects(AddParentMovementToHeld);
    }

    protected void AddParentMovementToHeld(TelekenesisPhysicsObject obj)
    {
        obj.Rigidbody.MovePosition(obj.Rigidbody.position + parentRB.velocity / 60f);
    }

    

    protected void UpdateParentMovement()
    {
        oldParentPosition = newParentPosition;
        newParentPosition = pickuppOriginPoint.position;

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
        obj.SetNewParrent(pickuppOriginPoint);

        obj.Rigidbody.angularDrag = 30;
        obj.Rigidbody.drag = 10;
        obj.Rigidbody.useGravity = false;

        
        
    }

    protected Vector3 GetShapePoint(TelekenesisPhysicsObject obj)
    {
        return Shape.ClosestPoint(obj.transform.position);
    }


    protected void HoldTelekenesis(TelekenesisPhysicsObject obj)
    {
        //AddHolderVelocity(obj);
        PullObjectToEpicenter(obj);
        PushObjectsApart(obj);
    }

    protected void PullObjectToEpicenter(TelekenesisPhysicsObject obj)
    {
        obj.Rigidbody.AddExplosionForce(-ImplotionForce , GetShapePoint(obj), Radius, -Upmod);
        obj.Rigidbody.AddExplosionForce(-ImplotionForce * ( ObjectBoundSizeMod(obj) * SizeForceMod * EpicenterForceMod * 0.5f), pickuppOriginPoint.position, Radius, -Upmod);
        
        
    }

   

    protected float ObjectBoundSizeMod(TelekenesisPhysicsObject obj)
    {
        return obj.BoundSize * obj.BoundSize * obj.BoundSize;
    }

    
    protected void LerpPosToEpicenter(TelekenesisPhysicsObject obj)
    {
        // unused
        // bug: telleports objects when they are picked upp

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


