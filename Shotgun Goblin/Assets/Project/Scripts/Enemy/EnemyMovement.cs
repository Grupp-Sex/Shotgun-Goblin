using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private float UpdateSpeed;

    private NavMeshAgent Agent;

    private Coroutine Follow;

    private EnemyAnimationHandler animationHandler;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        animationHandler = GetComponentInChildren<EnemyAnimationHandler>();
        Agent.enabled = false;
        
    }

    public void StartChase()
    {

        if (Follow == null)
        {
            Agent.enabled = true;

            StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("Enemy is already chasing.");
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateSpeed);

        while (enabled)
        {
            if (Agent.enabled && Target != null)
            {
                if (Agent.isOnNavMesh)
                {

                    //Agent.SetDestination(Target.transform.position);
                    SetDestination(Target.transform.position);

                    

                }
                else
                {
                    
                    Debug.LogError("Enemy is not on navmesh");
                }
            }

            yield return Wait;
        }
        animationHandler.SetRunning(false);


    }


    
    void Update()
    {

        // by Mikael (moved to update by Ansgar)
        if (Agent.velocity.magnitude > 0.05f)
        {
            //Putting animation triggers on enemy throughout the script / Mikey
            animationHandler?.SetRunning(true);
        }
        else
        {
            animationHandler?.SetRunning(false);
        }
    }


    // added by Ansgar
    protected void SetDestination(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        Agent.CalculatePath(position, path);
        Agent.SetPath(path);

    }
}
