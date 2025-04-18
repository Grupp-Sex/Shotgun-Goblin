using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ThrowAimer : MonobehaviorScript_ToggleLog
{
    [SerializeField] Transform Vertical_RotationObject;
    [SerializeField] Transform Horizontal_RotationObject;
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

    public bool FoundTrejectory {  get; private set; }
    




    public void Aim(Vector3 TargetPos, float StartVelocity)
    {
        Vector3 direction = GetDirection(OriginObject.position, TargetPos);

        Vector2 TargetPosComposants = ToComposantsInDirection(direction, upDirection, out Vector3 groundDirection);

        float angleV = CaculateAngle(StartVelocity, TargetPosComposants, typeOfThrow);

        angleV = ToDegrees(angleV);


        float angleH = Vector3.SignedAngle(Vector3.ProjectOnPlane(transform.forward, upDirection), groundDirection, upDirection);

        

        ExecuteAim(angleV, angleH);
    }

    protected void ExecuteAim(float verticalAngle, float horizontalAngle)
    {

        float vAngle = Mathf.Clamp(verticalAngle, MinVerticalAngle, MaxVerticalAngle);
        float hAngle = Mathf.Clamp(horizontalAngle, MinHorizontalAngle, MaxHorizontalAngle);


        vAngle *= -1;


        Horizontal_RotationObject.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);

        Horizontal_RotationObject.transform.Rotate(new Vector3(0, 1, 0), hAngle);

        Vertical_RotationObject.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);
        Vertical_RotationObject.transform.Rotate(new Vector3(1,0,0), vAngle);

        

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

        if (IsNegativeX(hight, upDirection)) hightMagnitude *= -1;
        if(IsNegativeX(groundDistance, Horizontal_RotationObject.forward)) groundDistanceMagnitude *= -1;

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

        if (float.IsNormal(angle))
        {
            FoundTrejectory = true;
        }
        else
        {
            FoundTrejectory = false;
        }

        return angle;
    }

    protected float ToDegrees(float radians)
    {
        return radians * 180 / math.PI;
    }

}
