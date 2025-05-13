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

        rbData = SaveRB(rbData,rb);

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
            agent.enabled = false;
            agent.updatePosition = false;
            agent.updateUpAxis = false;
            agent.updateRotation = false;
        }

        rb.constraints = new RigidbodyConstraints();
        rb.isKinematic = false;
        rb.drag = 0;
        rb.angularDrag = 0;

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
            agent.enabled = true;
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


    protected RigidBodyData SaveRB(RigidBodyData rbData, Rigidbody rb)
    {
        DebugLog("is kinematick = " + rb.isKinematic);
        IsKinematic = rb.isKinematic;
        rbData.isKinematic = rb.isKinematic;
        rbData.constraints = rb.constraints;

        rbData.drag = rb.drag;
        rbData.angularDrag = rb.angularDrag;

        return rbData;
        
    }

    protected void LoadRB(RigidBodyData rbData, Rigidbody rb)
    {
        rb.isKinematic = rbData.isKinematic;
        rb.constraints = rbData.constraints;
       
        

        rb.isKinematic = IsKinematic;

        rb.drag = rbData.drag;
        rb.angularDrag = rbData.angularDrag;
        

        DebugLog("is kinematick = " + rb.isKinematic);
    }


    protected struct RigidBodyData
    {
        public bool isKinematic;
        public RigidbodyConstraints constraints;
        public float drag;
        public float angularDrag;
    }
}

public interface IRagdollActivated
{
    public void EnterRagdoll();

    public void ExitRagdoll();
}
