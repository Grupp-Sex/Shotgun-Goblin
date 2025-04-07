using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelMovementTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject wheelHolder;
    protected WheelCollider wheelCollider;
    protected PlayerMovement playerMovement;
    protected Vector3 movementInput;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        wheelCollider = wheelHolder.GetComponent<WheelCollider>();
        

    }
    private void OnMovement(InputValue inputValue)
    {

        movementInput = new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);
        movementInput = movementInput.normalized;
        //Debug.Log(movementInput);
        //wheelCollider.motorTorque = 1;
        //wheelCollider.rotationSpeed = 1000;

        wheelCollider.motorTorque = 0.00001f;

        wheelCollider.brakeTorque = 0;

        
    }

    private void OnMovementStop(InputValue inputValue)
    {
        movementInput = Vector3.zero; //new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);

        wheelCollider.brakeTorque = 100;
        

        //Debug.Log("stopped movement " + movementInput);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
                
        //wheelCollider.transform.LookAt(wheelCollider.transform.position + movementInput);

        wheelCollider.steerAngle = Vector3.SignedAngle(movementInput, new Vector3(0,0,1),new Vector3(0,-1,0));

    }

    
}
