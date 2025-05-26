using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointToParrent : MonoBehaviour
{
    protected ConfigurableJoint characterJoint;

    private void Awake()
    {
        characterJoint = GetComponent<ConfigurableJoint>();
    }
    private void Start()
    {

        Rigidbody rb = transform.parent.GetComponent<Rigidbody>();
        if (rb != null)
        {
            characterJoint.connectedBody = rb;
        }
        
    }
}
