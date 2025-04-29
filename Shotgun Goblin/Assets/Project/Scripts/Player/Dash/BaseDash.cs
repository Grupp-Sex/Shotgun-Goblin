using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDash : MonoBehaviour
{
    
    public Rigidbody rb;
    public bool RigidbodyFromThisObject;

    private void Start()
    {
        if (RigidbodyFromThisObject)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public virtual void Dash(Vector3 direction, float velocity)
    {
        Dash(direction, velocity, rb);
    }
    
    public virtual void Dash(Vector3 direction, float velocity, Rigidbody rb)
    {

    }
}
