using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemoveNavObsticleOnTelekenesis : MonoBehaviour, IOnTelekenesisEnter
{
    public NavMeshObstacle NavMeshObstacle;

    private void Start()
    {
        if(NavMeshObstacle == null)
        {
            NavMeshObstacle = GetComponent<NavMeshObstacle>();
        }
    }
    public void OnTelekenesisEnter()
    {
        NavMeshObstacle.enabled = false;
    }
}
