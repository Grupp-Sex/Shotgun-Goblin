using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    

    private Rigidbody characterRB;

    public Transform orientation;

    [Header("Movement")]
    private Vector3 movementInput;
    private Vector3 movementVector;

    [SerializeField] private float movementSpeed;
   [SerializeField] private float groundDrag;


    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header ("Ground Check")]
    public float playerHeight;
    [SerializeField] private float isGroundedOffset;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Wheel")]
    [SerializeField] WheelCollider wheel;
    [SerializeField] float BreakingTorque;

    void Start()
    {
        characterRB = GetComponent<Rigidbody>();

        readyToJump = true;

        
        
    }

    private void Update()
    {
        characterRB.drag = groundDrag;

        SpeedControl();

      

        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f + isGroundedOffset, whatIsGround);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 1.1f);


        //Handle Drag
        if (grounded)
        {
            characterRB.drag = groundDrag;
        }
        else
        {
            characterRB.drag = 0;
        }

       
        
    }

    
    void FixedUpdate()
    {
        if (movementInput != Vector3.zero)
        {
            
            movementVector = movementInput;/* movementInput.x * orientation.right + orientation.forward * movementInput.z;*/

            if (grounded)
            {
                characterRB.AddRelativeForce(movementVector.normalized * movementSpeed, ForceMode.Force);
            }
            else if (!grounded)
            {
                characterRB.AddRelativeForce(movementVector.normalized * movementSpeed * airMultiplier, ForceMode.Force);
            }


            //Debug.Log("___________ " + movementVector);


            TurnWheel(movementInput.normalized);
        }


       



    }

    protected void TurnWheel(Vector3 turnDirection)
    {
        //converts the turnDirection vectior into an angle between -180 and 180
        float turnAngle = Vector3.SignedAngle(turnDirection, new Vector3(0, 0, 1), new Vector3(0, -1, 0));

        wheel.steerAngle = turnAngle;

    }

    protected void WheelBreaksOn()
    {
        wheel.brakeTorque = BreakingTorque;

        wheel.motorTorque = 0;
    }

    protected void WheelBreaksOff()
    {
        wheel.brakeTorque = 0;

        // disables the wheels built in "handbrake mode"
        wheel.motorTorque = 0.00001f; 
    }

    private void OnMovement(InputValue inputValue)
    {

        movementInput = new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);

        //Debug.Log(movementInput);

        WheelBreaksOff();
       
        
    }

    private void OnMovementStop(InputValue inputValue)
    {
        movementInput = Vector3.zero; //new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);

        //Debug.Log("stopped movement " + movementInput);

        WheelBreaksOn();
    }

    private void OnJumpStart()
    {
        
        if (readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void Jump()
    {

        
        //reset Y velocity
        characterRB.velocity = new Vector3(characterRB.velocity.x, 0f, characterRB.velocity.z);

        characterRB.AddRelativeForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(characterRB.velocity.x, 0f, characterRB.velocity.z);

        //Limit velocity if needed
        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            characterRB.velocity = new Vector3(limitedVelocity.x, characterRB.velocity.y, limitedVelocity.z);
        }
    }

   

   
}
