using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollManager : MonobehaviorScript_ToggleLog, IBlockable
{
    

    protected bool isRagdolled;

    protected RigidBodyData rbData = new RigidBodyData();

    [SerializeField] bool IsKinematic;

    protected IEnumerator ActiveRagdollTimer;

    protected IRagdollActivated[] ragdollScripts;

    protected Rigidbody rb;
    protected NavMeshAgent agent;

    protected EnableQueue canExitRagdolQueue = new EnableQueue();

    public EventPusher<object> Event_RagdollStart = new EventPusher<object>();
    public EventPusher<object> Event_RagdollEnd = new EventPusher<object>();

    

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rbData = SaveRB(rbData, rb);

        agent = GetComponent<NavMeshAgent>();

        ragdollScripts = GetComponents<IRagdollActivated>();
    }


    

    public void AddBlocker(object key)
    {
        canExitRagdolQueue.AddBlocker(key);
    }

    public void RemoveBlocker(object key)
    {
        canExitRagdolQueue.RemoveBlocker(key);
    }

    public bool CanExitRagdoll()
    {
        return canExitRagdolQueue.IsUnBlocked();
    }

    public void EnterRagdoll(float duration, Vector3 trippSpeed)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (isRagdolled)
        {
            StopCoroutine(ActiveRagdollTimer);
        }

        ActiveRagdollTimer = RagdollTimer(duration);

        RagdollActivate(trippSpeed);
        StartCoroutine(ActiveRagdollTimer);


    }

    protected IEnumerator RagdollTimer(float time)
    {
        yield return new WaitForSeconds(time);

        yield return new WaitUntil(CanExitRagdoll);

        RagdollDeactivate();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected void RagdollActivate(Vector3 trippSpeed)
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

        rb.angularVelocity += trippSpeed;

        isRagdolled = true;

        Event_RagdollStart.Invoke(this, null);

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

        Event_RagdollEnd.Invoke(this, null);

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

