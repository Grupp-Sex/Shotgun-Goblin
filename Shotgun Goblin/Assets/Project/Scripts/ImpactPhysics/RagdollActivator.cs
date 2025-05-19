using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RagdollActivator : MonobehaviorScript_ToggleLog, IImpactThreshold
{
    [SerializeField] float RagdollTime = 1;

    [SerializeField] Vector3 angularTripSpeed;

    public RagdollManager RagdollManager;

    // Start is called before the first frame update
    void Awake()
    {
       if(RagdollManager == null)
        {
            RagdollManager = GetComponent<RagdollManager>();
        }
    }

    

    public void SoftImpact(CollisionData collision)
    {
        RagdollManager?.EnterRagdoll(RagdollTime, angularTripSpeed);
    }

    public void HardImpact(CollisionData collision)
    {

    }


    
}


