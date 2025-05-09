using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemoveNavObsticleOnTelekenesis : MonoBehaviour, IOnTelekenesisEnter, IOnTelekenesisLeave
{
    public NavMeshObstacle NavMeshObstacle;
    [SerializeField] bool ReEnable = false;

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

    public void OnTelekenesisLeave()
    {
        if (ReEnable)
        {
            NavMeshObstacle.enabled = true;
        }
    }
}
