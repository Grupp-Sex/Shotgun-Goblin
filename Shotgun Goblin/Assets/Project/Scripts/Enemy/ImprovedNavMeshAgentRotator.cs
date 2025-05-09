using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ImprovedNavMeshAgentRotator : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public NavMeshAgent Agent;


    // 0 = no effect
    // 1 = full effect (only ImprovedNavMeshAgentRotator can effect rotation)
    [SerializeField] float Interpolation;

    [SerializeField] float MaxRotationAngle;


    [SerializeField] Quaternion currentRotation;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] Quaternion interploationRotation;
    [SerializeField] Quaternion differenceRotation;


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
        //Agent.updateUpAxis = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InterpolateRotation(Interpolation);
    }

    protected virtual void InterpolateRotation(float interpolation)
    {
        if (Rigidbody != null && Agent != null && Agent.pathStatus != NavMeshPathStatus.PathInvalid && Agent.updatePosition)
        {
            //if (Agent.desiredVelocity == Vector3.zero) return;
            //Vector3 navmeshDirection = Agent.desiredVelocity.normalized;

            Vector3 navmeshDirection = GetDirection();



            targetRotation = Quaternion.LookRotation(navmeshDirection);

            currentRotation = Rigidbody.rotation;

            Quaternion clampedTargetRotation = Quaternion.RotateTowards(currentRotation, targetRotation, MaxRotationAngle);

            interploationRotation = Quaternion.Lerp(currentRotation, targetRotation, interpolation);

            differenceRotation = currentRotation * Quaternion.Inverse(interploationRotation);

            if (interploationRotation != currentRotation)
            {
                Rigidbody.MoveRotation(interploationRotation);
            }
        }

    }

    protected Vector3 GetDirection()
    {
        return Agent.nextPosition - Rigidbody.position;
    }


}
