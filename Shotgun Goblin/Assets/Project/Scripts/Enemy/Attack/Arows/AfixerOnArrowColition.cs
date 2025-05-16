using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AfixerOnArrowColition : MonoBehaviour, IArrowDamage
{
    public Rigidbody fixPoint;

    public bool deleatOnColition = true;

    public EventPusher<Collision> Event_ArrowCollision { get; protected set; }  = new EventPusher<Collision>();

    // Start is called before the first frame update
    void Awake()
    {
        if (fixPoint == null)
        {
            fixPoint = GetComponent<Rigidbody>();

        }

        //joint.breakForce = 0;
    }

    private void OnCollisionEnter(Collision collision)
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

        Event_ArrowCollision.Invoke(this, collision);


        if (deleatOnColition)
        {
            gameObject.SetActive(false);
        }
    }
}

    
