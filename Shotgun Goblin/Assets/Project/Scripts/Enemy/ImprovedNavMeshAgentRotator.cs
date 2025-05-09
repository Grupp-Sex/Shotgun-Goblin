using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ImprovedNavMeshAgentRotator : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public NavMeshAgent Agent;

    public bool Active;

    // 0 = no effect
    // 1 = full effect (only ImprovedNavMeshAgentRotator can effect rotation)
    [SerializeField] float Interpolation;
    [SerializeField] float TargetInterpolation = 1;
    [SerializeField] float MaxRotationAngle;
    

    [SerializeField] bool CanRotate;
    [SerializeField] bool NotNull;

    [SerializeField] Quaternion currentRotation;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] Quaternion interploationRotation;
    [SerializeField] Quaternion differenceRotation;
    [SerializeField] Vector3 navmeshDirection;
    //[SerializeField] Quaternion differenceRotation;


    // Start is called before the first frame update
    void Start()
    {
        if (Agent == null)
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        if(Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
        //Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Active)
        {
            CheckIsEnabled();

            UpdateRotation();

            Interpolate(Interpolation);
        }
    }

    protected bool CheckCanRotate()
    {
        CanRotate = NotNull && navmeshDirection != Vector3.zero;

        return CanRotate;
    }

    protected bool CheckIsEnabled()
    {
        NotNull = Rigidbody != null && Agent != null && Agent.isActiveAndEnabled && Agent.updatePosition;

        return NotNull;
    }

    protected virtual void UpdateRotation()
    {
        navmeshDirection = Agent.desiredVelocity.normalized;

        if (!CheckCanRotate()) return;

        //if (Agent.desiredVelocity == Vector3.zero) return;
        //Vector3 navmeshDirection = Agent.desiredVelocity.normalized;


        Quaternion newtargetRotation = Quaternion.LookRotation(navmeshDirection);

        currentRotation = Rigidbody.rotation;

        Quaternion clampedTargetRotation = Quaternion.RotateTowards(currentRotation, newtargetRotation, MaxRotationAngle);


        targetRotation = Quaternion.Slerp(targetRotation, clampedTargetRotation, TargetInterpolation);


    }

    

    protected void Interpolate(float interpolation)
    {
        if (NotNull)
        {
            interploationRotation = Quaternion.Slerp(currentRotation, targetRotation, interpolation);

            //differenceRotation =  Quaternion.Inverse(targetRotation) * currentRotation ;

            if (interploationRotation != currentRotation)
            {
                Rigidbody.MoveRotation(interploationRotation);
                //Rigidbody.AddRelativeTorque(differenceRotation.eulerAngles * TorqueAcceleration, ForceMode.Acceleration);
                
            }
        }

    }

    protected Vector3 GetDirection()
    {
        if(Agent == null || Rigidbody == null) return Vector3.zero;

        return Vector3.Project((Agent.nextPosition - Rigidbody.position), Agent.nextPosition).normalized;
    }


}
