using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour, IMover
{
    

    [Header ("Camera Tilt")]
    private float currentTilt = 0f;
     private float targetTilt;
    [SerializeField] private float tiltAmount = 10f;
    [SerializeField] private float tiltSpeed = 0.8f;


   [SerializeField] private Transform orientation;
    private Rigidbody characterRB;

    

    [Header ("Movement")]
    private Vector3 movementInput;
    private Vector3 targetMovementInput;
    private Vector3 movementVector;

    [SerializeField] private float movementSpeed;
   [SerializeField] private float groundDrag;
    [SerializeField] private float turningSmotheness = 0.9f;


    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public bool isJumping;

    [Header ("Ground Check")]
    public float playerHeight;
    [SerializeField] private float isGroundedOffset;
    public LayerMask whatIsGround;
    public bool grounded {  get; private set; }

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

        SmotheInput(turningSmotheness);

        if (targetMovementInput != Vector3.zero)
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

        CameraTiltValue();

       



    }

    public Vector3 GetInputDirection()
    {
        return targetMovementInput.normalized;
    }

    //gradualy turns movementInput to the trargent input
    protected void SmotheInput(float lerpValue)
    {
        
        movementInput = movementInput * (1 - lerpValue) + targetMovementInput * lerpValue;
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
        Vector2 input = inputValue.Get<Vector2>();

        targetMovementInput = new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);

        //Debug.Log(movementInput);

        WheelBreaksOff();

        //Send horizontal movement to camera for tilting
        
        CameraTilt(input.x);

    }

    private void OnMovementStop(InputValue inputValue)
    {
        //movementInput = Vector3.zero; //new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);
        targetMovementInput = Vector3.zero;

        //Debug.Log("stopped movement " + movementInput);

        WheelBreaksOn();

       // Stop Camera tilt if not moving
       CameraTilt(0f);
        
    }

    private void OnJumpStart()
    {
        
        if (readyToJump)
        {
            isJumping = true;
            if (grounded)
            {


                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    private void OnJumpStop()
    {
        isJumping = false;
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

  
    private void CameraTiltValue()
    {
        float lerpValue = tiltSpeed;
        currentTilt = currentTilt * lerpValue + targetTilt * (1 - lerpValue);

        //Apply tilt to camera (z-axis)
        Quaternion rotation = Quaternion.Euler(0, 0, currentTilt * tiltAmount);
        orientation.SetLocalPositionAndRotation(orientation.localPosition, rotation);
    }

    private void CameraTilt(float horizontalInput)
    {
        
       

        //Debug.Log(targetTilt + " wweeew");

        targetTilt = -horizontalInput;
    }

}

public interface IMover
{
    public Vector3 GetInputDirection();
}
