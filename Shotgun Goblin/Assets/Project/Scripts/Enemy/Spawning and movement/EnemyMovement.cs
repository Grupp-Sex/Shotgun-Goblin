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

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void StartChase()
    {
        if (Follow == null)
        {
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
            Agent.SetDestination(Target.transform.position);

            yield return Wait;
        }

    }
}
