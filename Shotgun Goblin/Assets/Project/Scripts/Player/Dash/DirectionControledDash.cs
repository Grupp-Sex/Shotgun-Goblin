using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionControledDash : BaseDash
{
    public Rigidbody rigidbody;
    public GameObject movement;
    
    [SerializeField] float DirectionInterpolation;
    [SerializeField] float TruncationInterpolation;
    [SerializeField] DashData DashData;


    protected IMover movement_mover;

    private void OnValidate()
    {
        Validate();
    }

    protected void Validate()
    {
        if (movement != null)
        {
            movement_mover = movement.GetComponent<IMover>();
        }
    }

    public void SetMovement(GameObject movement)
    {
        this.movement = movement;
        Validate();
    }

    public void SetRigidbody(Rigidbody rigidbody)
    {
        this.rigidbody = rigidbody;
    }

    protected Vector3 GetMovement()
    {
        if(movement_mover != null)
        {
            return movement_mover.GetInputDirection();
        }
        else
        {
            return Vector3.zero;
        }
    }

    
    public void Dash()
    {
        base.Dash(DashData, rigidbody);

    }


    protected override DashData AlterDashData(DashData dashData)
    {
        dashData = base.AlterDashData(dashData);

        dashData.unalterd_direction = CalulateDashDirection(dashData.direction, GetMovement(), DirectionInterpolation);

        dashData.truncateDirection = CalulateDashDirection(dashData.truncateDirection, GetMovement(), TruncationInterpolation);

        return dashData;
    }

    protected virtual Vector3 CalulateDashDirection(Vector3 direction, Vector3 movement, float interpolation)
    {
        if(movement == Vector3.zero) return direction;

        Vector3 newDirection = Vector3.Slerp(direction,movement, interpolation);

        return newDirection;
    }

   
}
