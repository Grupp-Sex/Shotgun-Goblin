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

    [SerializeField] bool IsKinematic;

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
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

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

    private void OnDisable()
    {
        StopAllCoroutines();
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

        

        if (agent != null)
        {
            agent.updatePosition = true;
            agent.updateUpAxis = true;
            agent.updateRotation = true;
        }

        LoadRB(rbData, rb);
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
        DebugLog("is kinematick = " + rb.isKinematic);
        IsKinematic = rb.isKinematic;
        rbData.isKinematic = rb.isKinematic;
        rbData.constraints = rb.constraints;
    }

    protected void LoadRB(RigidBodyData rbData, Rigidbody rb)
    {
        rb.isKinematic = rbData.isKinematic;
        rb.constraints = rbData.constraints;

        rb.isKinematic = IsKinematic;

        DebugLog("is kinematick = " + rb.isKinematic);
    }


    protected struct RigidBodyData
    {
        [SerializeField] public bool isKinematic;
        [SerializeField] public RigidbodyConstraints constraints;
    }
}

public interface IRagdollActivated
{
    public void EnterRagdoll();

    public void ExitRagdoll();
}
