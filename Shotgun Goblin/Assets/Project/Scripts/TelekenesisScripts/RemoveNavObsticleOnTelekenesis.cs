using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemoveNavObsticleOnTelekenesis : MonoBehaviour
{
    public TelekenesisPhysicsObject TelekenesisPhysicsObject;
    public NavMeshObstacle NavMeshObstacle;
    [SerializeField] bool ReEnable = false;

    private void Awake()
    {
        if(NavMeshObstacle == null)
        {
            NavMeshObstacle = GetComponent<NavMeshObstacle>();
        }

        if(TelekenesisPhysicsObject == null)
        {
            TelekenesisPhysicsObject = GetComponent<TelekenesisPhysicsObject>();
        }

        
    }

    private void OnEnable()
    {
        if (TelekenesisPhysicsObject != null)
        {
            TelekenesisPhysicsObject.Event_OnTelekenesisEnter.Subscribe(Event_TelekenesisEnter);
            TelekenesisPhysicsObject.Event_OnTelekenesisEnter.Subscribe(Event_TelekenesisLeave);
        }
    }

    private void OnDisable()
    {
        if (TelekenesisPhysicsObject != null)
        {
            TelekenesisPhysicsObject.Event_OnTelekenesisEnter.UnSubscribe(Event_TelekenesisEnter);
            TelekenesisPhysicsObject.Event_OnTelekenesisEnter.UnSubscribe(Event_TelekenesisLeave);
        }
    }

    public void Event_TelekenesisEnter(object sender, TelekenesisPhysicsObject args)
    {
        NavMeshObstacle.enabled = false;
    }

    public void Event_TelekenesisLeave(object sender, TelekenesisPhysicsObject args)
    {
        if (ReEnable)
        {
            NavMeshObstacle.enabled = true;
        }
    }
}
