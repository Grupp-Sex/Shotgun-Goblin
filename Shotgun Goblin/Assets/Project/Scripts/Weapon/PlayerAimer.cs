using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    [SerializeField] Transform PlayerVerticalLooker;
    [SerializeField] float MinDistance = 2;
    [SerializeField] float MaxDistance = 100;
    [SerializeField] float MoveBackMult = 1;
    [SerializeField] float DirectionLerpValue = 0.5f;
    [SerializeField] float OffsetLerpValue = 0.5f;

    protected Vector3 TargetDirection;

    protected float CurrentOffset;
    protected float TargetOffset;
    

    protected Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        InterpolateAim(DirectionLerpValue);
        InterpolateOffset(OffsetLerpValue);

    }

    protected void Aim()
    {
        Vector3 direction = PlayerVerticalLooker.forward;

        if (Physics.Raycast(new Ray(PlayerVerticalLooker.position, direction), out RaycastHit hit, MaxDistance))
        {

            ExecuteMove(hit.distance);


            ExecuteAim(hit.point);
        }
        else
        {
            ExecuteMove(MaxDistance);

            TargetDirection = direction;
        }
        
    }

    protected void InterpolateAim(float T)
    {
        Vector3 currentRotaion = transform.forward * T + TargetDirection * (1 - T);
        
        transform.rotation = Quaternion.LookRotation(currentRotaion);
        
    }

    protected void InterpolateOffset(float T)
    {
        CurrentOffset = CurrentOffset * T + TargetOffset * (1 - T);

        transform.localPosition = startPos + new Vector3(0, 0, 1) * CurrentOffset;
    }

    protected void ExecuteAim(Vector3 target)
    {
        TargetDirection = target - transform.position;
    }

    protected void ExecuteMove(float distance)
    {
        if (distance < MinDistance)
        {
            TargetOffset = (MinDistance - distance) * MoveBackMult;
        }
        else
        {
            TargetOffset = 0;
        }
    }

    
}
