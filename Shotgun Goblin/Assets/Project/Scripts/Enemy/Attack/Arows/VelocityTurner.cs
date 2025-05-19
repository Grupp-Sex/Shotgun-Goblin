using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTurner : MonoBehaviour
{
    public Rigidbody Rigidbody;

    
    void Awake()
    {
        if(Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        TurnRigidbody();
    }

    protected void TurnRigidbody()
    {
        if (Rigidbody != null)
        {
            Vector3 direction = Rigidbody.velocity;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Rigidbody.MoveRotation(rotation);
        }

    }
}
