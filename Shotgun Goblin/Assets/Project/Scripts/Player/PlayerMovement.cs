using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour, IMover
{
    

    [Header ("Camera Tilt")]
   
    [SerializeField] private float tiltAmount = 10f;
    [SerializeField] private float tiltSpeed = 0.8f;
    private float currentTilt = 0f;
    private float targetTilt;


    [SerializeField] private Transform orientation;
    private Rigidbody characterRB;

    

    [Header ("Movement")]
    private Vector3 movementInput;
    private Vector3 targetMovementInput;
    private Vector3 movementVector;
    private Vector3 movementAcceleration;

    [SerializeField] private float movementSpeed;
    [SerializeField] private bool doMaxSpeed = true;
    [SerializeField] private float maxSpeed = 10;
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

 

    void Start()
    {
        characterRB = GetComponent<Rigidbody>();

        readyToJump = true;

      
        
    }

    private void Update()
    {
        characterRB.drag = groundDrag;

        //SpeedControl();

      

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
        CalculateAcceleration();
        if (targetMovementInput != Vector3.zero)
        {

            

            characterRB.AddRelativeForce(movementAcceleration, ForceMode.Acceleration);




        }

        CameraTiltValue();

       



    }

    protected void CalculateAcceleration()
    {
        movementVector = movementInput;/* movementInput.x * orientation.right + orientation.forward * movementInput.z;*/

        movementAcceleration = movementVector.normalized * movementSpeed;

        if (!grounded)
        {
            movementAcceleration *= airMultiplier;
        }

        if (doMaxSpeed)
        {
            movementAcceleration = LimitAcceleration(movementAcceleration, characterRB.velocity, maxSpeed);
        }


    }

    protected Vector3 LimitAcceleration(Vector3 acceleration, Vector3 velocity, float maxSpeed)
    {
        if(Vector3.Dot(acceleration, velocity) < 0)
        {
            return acceleration;
        }

        Vector3 forwardVelocity = Vector3.Project(velocity, acceleration);

        if(forwardVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            return Vector3.ClampMagnitude(acceleration, maxSpeed);
        }

        return acceleration;
    }

    public Vector3 GetInputDirection()
    {
        return targetMovementInput;
    }

    //gradualy turns movementInput to the trargent input
    protected void SmotheInput(float lerpValue)
    {
        
        movementInput = movementInput * (1 - lerpValue) + targetMovementInput * lerpValue;
    }

   

    
    

    private void OnMovement(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();

        targetMovementInput = new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);

        


        //Send horizontal movement to camera for tilting
        
        CameraTilt(input.x);

    }

    private void OnMovementStop(InputValue inputValue)
    {
   
        targetMovementInput = Vector3.zero;

      


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
        targetTilt = -horizontalInput;
    }

}

public interface IMover
{
    public Vector3 GetInputDirection();
}
