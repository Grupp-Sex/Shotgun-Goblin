using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AfixerOnArrowColition : MonoBehaviour
{
    public Rigidbody fixPoint;

    public bool deleatOnColition = true;

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

            Debug.Log("b");

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

    
