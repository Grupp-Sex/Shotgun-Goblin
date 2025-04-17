using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAimer : MonoBehaviour
{
    [SerializeField] GameObject RotationObject;
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

    protected Vector3 upDirection => Physics.gravity.normalized;

    

    public void Aim(Vector3 Pos, float StartVelocity)
    {
        
        
    }

    protected Vector3 GetDirection(Vector3 ThisPos, Vector3 TargetPos)
    {
        return TargetPos - ThisPos;
    }
    

    protected Vector2 ToComposantsInDirection(Vector3 Direction, Vector3 up)
    {

        Vector3 groundDistance = Vector3.ProjectOnPlane(Direction, up);
        Vector3 hight = Vector3.Project(Direction,up);

        return new Vector2(0, 0);
    }
    

}
