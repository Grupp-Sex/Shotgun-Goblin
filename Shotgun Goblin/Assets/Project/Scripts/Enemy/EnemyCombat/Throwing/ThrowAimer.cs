using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ThrowAimer : MonobehaviorScript_ToggleLog
{
    [SerializeField] GameObject RotationObject;
    [SerializeField] public Transform OriginObject;
    protected enum TypeOfThrow
    {
        High,
        Low
    }

    [SerializeField] TypeOfThrow typeOfThrow;
    [SerializeField] float MaxVerticalAngle = 90;
    [SerializeField] float MinVerticalAngle = -90;

    [SerializeField] float MaxHorizontalAngle = 90;
    [SerializeField] float MinHorizontalAngle = -90;

    protected Vector3 upDirection => new Vector3(0,1,0);

    

    public void Aim(Vector3 TargetPos, float StartVelocity)
    {
        Vector3 direction = GetDirection(OriginObject.position, TargetPos);

        Vector2 TargetPosComposants = ToComposantsInDirection(direction, upDirection, out Vector3 groundDirection);

        float angleV = CaculateAngle(StartVelocity, TargetPosComposants, typeOfThrow);

        angleV = ToDegrees(angleV);


        float angleH = Vector3.Angle(Vector3.ProjectOnPlane(transform.forward, upDirection), groundDirection);



        ExecuteAim(-angleV);
    }

    protected void ExecuteAim(float verticalAngle)
    {
        
        RotationObject.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);
        RotationObject.transform.Rotate(new Vector3(1,0,0), verticalAngle);

        

    }

    protected Vector3 GetDirection(Vector3 ThisPos, Vector3 TargetPos)
    {
        return TargetPos - ThisPos;
    }
    

    protected Vector2 ToComposantsInDirection(Vector3 Direction, Vector3 up)
    {

        return ToComposantsInDirection(Direction, up, out Vector3 groundDirection);
    }

    

    protected Vector2 ToComposantsInDirection(Vector3 Direction, Vector3 up, out Vector3 groundDistance)
    {

        groundDistance = Vector3.ProjectOnPlane(Direction, up);
        Vector3 hight = Vector3.Project(Direction, up);

        float groundDistanceMagnitude = groundDistance.magnitude;
        float hightMagnitude = hight.magnitude;

        if (IsNegativeX(hight, transform.up)) hightMagnitude *= -1;
        if(IsNegativeX(groundDistance, transform.forward)) groundDistanceMagnitude *= -1;

        return new Vector2(groundDistanceMagnitude, hightMagnitude);
    }

    protected bool IsNegativeX(Vector3 ThisPos, Vector3 Z)
    {
        float dotP = Vector3.Dot(ThisPos, Z);

        //DebugLog(dotP.ToString());

        return dotP < 0;
    }

    protected float CaculateAngle(float velocity, Vector2 target, TypeOfThrow type)
    {
        float G = Physics.gravity.magnitude;
        float V = velocity;

        float X = target.x;
        float Y = target.y;

        float typeChanger = 1;

        if(type == TypeOfThrow.High)
        {
            typeChanger = -1;
        }


        float Eq = math.pow(V,4) - G * G * X * X - 2 * G * Y * V * V;

        Eq = -V * V + typeChanger * math.sqrt(Eq);

        Eq = Eq / (G * X);

        float angle = -math.atan(Eq);

        return angle;
    }

    protected float ToDegrees(float radians)
    {
        return radians * 180 / math.PI;
    }

}
