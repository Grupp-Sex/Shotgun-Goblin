using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    private RaycastHit groundHit;
    public bool grounded {  get; private set; }

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle = 80f;
    [SerializeField] private float minSlopeBoost = 1f;
    [SerializeField] private float maxSlopeBoost = 2f;
    private RaycastHit slopeHit;

 

    void Start()
    {
        characterRB = GetComponent<Rigidbody>();

        readyToJump = true;

      
        
    }

    private void Update()
    {
        

        //SpeedControl();



        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, out groundHit, playerHeight / 2 + 0.2f);
        
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 1.1f);

        //If we want something to not be grounded, apply this tag 
        //if (grounded && groundHit.collider.CompareTag("IgnoreGround"))
        //{

        //}


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
        // updated by ansgar

        movementVector = movementInput;/* movementInput.x * orientation.right + orientation.forward * movementInput.z;*/

        movementAcceleration = movementVector.normalized * movementSpeed;

        if (!grounded)
        {
            movementAcceleration *= airMultiplier;
            characterRB.useGravity = true;
        }
        else
        {
            if (StandingOnSlope())
            {
                // If standing on slope, add force in the calculated slope direction
                Vector3 slopeDirection = SlopeMoveDirection();
                movementAcceleration = slopeDirection * movementSpeed * GetSlopeForceMultiplier();

                //movementVector.y = -4.5f;

                //Turn off gravity while on slope so player don't glide off
                characterRB.useGravity = false;
            }
            else
            {
                characterRB.useGravity = true;
            }
        }

        if (doMaxSpeed)
        {
            movementAcceleration = LimitAcceleration(movementAcceleration, characterRB.velocity, maxSpeed);
        }





        // original code by mikey

        //if (grounded && !StandingOnSlope())
        //{
        //    characterRB.AddRelativeForce(movementVector.normalized * movementSpeed, ForceMode.Force);
        //    characterRB.useGravity = true;
        //    //Debug.Log("P� marken");
        //}
        //else if (grounded && StandingOnSlope())
        //{
        //    // If standing on slope, add force in the calculated slope direction
        //    Vector3 slopeDirection = SlopeMoveDirection();
        //    characterRB.AddRelativeForce(slopeDirection * movementSpeed * GetSlopeForceMultiplier(), ForceMode.Force);

        //    movementVector.y = -4.5f;

        //    //Turn off gravity while on slope so player don't glide off
        //    characterRB.useGravity = !StandingOnSlope();
        //    //Debug.Log("Slope");
        //}
        //else if (!grounded)
        //{
        //    characterRB.AddRelativeForce(movementVector.normalized * movementSpeed * airMultiplier, ForceMode.Force);
        //    characterRB.useGravity = true;
        //    //Debug.Log("Luften");
        //}



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

    private bool StandingOnSlope()
    {
        // Check under player and calculate slope and store in angle and if angle is smaller than maxSlopeAngle and not 0 return true, if raycast dont hit return false
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            Debug.DrawRay(slopeHit.point, slopeHit.normal, Color.cyan);

            Debug.DrawRay(transform.position, Vector3.down * (playerHeight / 2 + 0.5f), Color.yellow);
            Debug.Log($"Slope angle: {angle}");

            return angle < maxSlopeAngle && angle != 0;

        }
        return false;
    }

    private float GetSlopeForceMultiplier()
    {
        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

        float slopePercentage = Mathf.InverseLerp(0, maxSlopeAngle, angle);
       float result = Mathf.Lerp(minSlopeBoost, maxSlopeBoost, slopePercentage);
        Debug.Log($"Slope added force: {result}");

        return result;
    }

    // Project movement vector onto slope and remove the slope's normals from the vector, making the player vector move along the slope direction
    private Vector3 SlopeMoveDirection()
    {
        Vector3 projected = Vector3.ProjectOnPlane(transform.TransformDirection(movementVector), slopeHit.normal);

        Vector3 direction = projected.normalized * movementVector.magnitude;

        Debug.DrawRay(transform.position, direction, Color.green);

        direction = transform.InverseTransformDirection(direction);

        


        return direction;
    }

}

public interface IMover
{
    public Vector3 GetInputDirection();
}
