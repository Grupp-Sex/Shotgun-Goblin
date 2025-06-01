using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonZone : MonoBehaviour
{
    public MoveCamera PlayerCamera;

    public Transform tempPosition;
    protected Transform lastTarget;

    protected bool inThridPerson;


    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            EnterThirdPerson();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            ExitThirdPerson();
        }
    }

    protected bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player");
    }

    protected void EnterThirdPerson()
    {
        if (!inThridPerson)
        {
            inThridPerson = true;


            lastTarget = PlayerCamera.playerTransform;
            PlayerCamera.playerTransform = tempPosition;
        }
    }

    protected void ExitThirdPerson()
    {
        if (inThridPerson)
        {
            inThridPerson = false;

            PlayerCamera.playerTransform = lastTarget;
            
        }
    }

}
