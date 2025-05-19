using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AfixerOnArrowColition : MonoBehaviour
{
    public Rigidbody fixPoint;

    public GameObject collisionDetection;
    protected ICollision collision1;

    public bool deleatOnColition = true;


    // Start is called before the first frame update
    void Awake()
    {
        if (fixPoint == null)
        {
            fixPoint = GetComponent<Rigidbody>();

        }

        if(collisionDetection == null)
        {
            collisionDetection = gameObject;

        }

        collision1 = collisionDetection.GetComponent<ICollision>();

        
        //joint.breakForce = 0;
    }

    private void OnEnable()
    {
        collision1.Event_Collision.Subscribe(Event_Collision);

    }

    private void OnDisable()
    {
        collision1.Event_Collision.UnSubscribe(Event_Collision);
    }

    private void Event_Collision( object sender, Collision collision)
    {

        
        Rigidbody rb = collision.rigidbody;
        ArticulationBody ab = collision.articulationBody;
        if (rb != null || ab != null)
        {
            FixedJoint joint = fixPoint.AddComponent<FixedJoint>();

            joint.breakForce = float.NaN;

            if(rb != null)
            {
                joint.connectedBody = rb;
            }
            else
            {
                joint.connectedArticulationBody = ab;
            }

            //joint.breakForce = 10000;
            //joint.breakTorque = 10000;
            //joint.enableCollision = true;
        }
        else
        {
            fixPoint.isKinematic = true;
            
        }

        fixPoint.mass = 0.000001f;

        


        if (deleatOnColition)
        {
            gameObject.SetActive(false);
        }
    }
}

    
