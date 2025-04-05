using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RagdollActivator : MonobehaviorScript_ToggleLog, IImpactThreshold
{
    [SerializeField] float RagdollTime = 1;

    [SerializeField] Vector3 angularTripSpeed;

    protected bool isRagdolled;

    protected RigidBodyData rbData = new RigidBodyData();

    protected IEnumerator ActiveRagdollTimer;

    protected IRagdollActivated[] ragdollScripts;

    protected Rigidbody rb;
    protected NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SaveRB(rbData,rb);

        agent = GetComponent<NavMeshAgent>();

        ragdollScripts = GetComponents<IRagdollActivated>();
    }

    protected void EnterRagdoll()
    {
        if (isRagdolled)
        {
            StopCoroutine(ActiveRagdollTimer);
        }

        ActiveRagdollTimer = RagdollTimer();

        RagdollActivate();
        StartCoroutine(ActiveRagdollTimer);
        
        
    }

    protected IEnumerator RagdollTimer()
    {
        yield return new WaitForSeconds(RagdollTime);

        RagdollDeactivate();
    }


    protected void RagdollActivate()
    {
        DebugLog("Ragdoll Engaged");

        if (agent != null)
        {
            agent.updatePosition = false;
            agent.updateUpAxis = false;
            agent.updateRotation = false;
        }

        SaveRB(rbData, rb);

        rb.constraints = new RigidbodyConstraints();
        rb.isKinematic = false;


        rb.angularVelocity += angularTripSpeed;

        isRagdolled = true;

        
    }

    protected void RagdollDeactivate()
    {
        DebugLog("Ragdoll Disengaged");

        Quaternion rotation = Quaternion.identity;

        rb.MoveRotation(rotation);
        isRagdolled = false;

        LoadRB(rbData, rb);

        if (agent != null)
        {
            agent.updatePosition = true;
            agent.updateUpAxis = true;
            agent.updateRotation = true;
        }
    }

    


    public void SoftImpact(CollisionData collision)
    {
        EnterRagdoll();
    }

    public void HardImpact(CollisionData collision)
    {

    }


    protected void SaveRB(RigidBodyData rbData, Rigidbody rb)
    {
        rbData.isKinematic = rb.isKinematic;
        rbData.constraints = rb.constraints;
    }

    protected void LoadRB(RigidBodyData rbData, Rigidbody rb)
    {
        rb.isKinematic = rbData.isKinematic;
        rb.constraints = rbData.constraints;
    }


    protected struct RigidBodyData
    {
        public bool isKinematic;
        public RigidbodyConstraints constraints;
    }
}

public interface IRagdollActivated
{
    public void EnterRagdoll();

    public void ExitRagdoll();
}
