using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    [SerializeField] Transform PlayerVerticalLooker;
    [SerializeField] float MinDistance = 2;
    [SerializeField] float MaxDistance = 100;

    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    protected void Aim()
    {
        if(Physics.Raycast(new Ray(PlayerVerticalLooker.position, PlayerVerticalLooker.forward), out RaycastHit hit, MaxDistance) && hit.distance > MinDistance)
        {
            ExecuteAim(hit.point);
        }
        else
        {

        }
    }

    protected void ExecuteAim(Vector3 target)
    {
        transform.LookAt(target);
    }

    protected void ResetAim()
    {
        transform.localRotation = Quaternion.identity;
    }
}
