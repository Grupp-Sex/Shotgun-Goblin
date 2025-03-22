using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody characterRB;

    public Transform orientation;

    private Vector3 movementInput;
    private Vector3 movementVector;
    [SerializeField] private float movementSpeed;
   [SerializeField] private float Drag;

    float jumpUpSpeed;

    bool canJump;
    bool jump;
    bool grounded;

    Collider ground;

   
    // Start is called before the first frame update
    void Start()
    {
        characterRB = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        characterRB.drag = Drag;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementInput != Vector3.zero)
        {
            //Vector3 movementVector = new Vector3(movementInput.x, 0.0f, movementInput.y);
            movementVector = movementInput;/* movementInput.x * orientation.right + orientation.forward * movementInput.z;*/

            characterRB.AddRelativeForce(movementVector.normalized * movementSpeed , ForceMode.Force);

            //Debug.Log("___________ " + movementVector);

        }

       


    }


    private void OnMovement(InputValue inputValue)
    {

        movementInput = new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);

        //Debug.Log(movementInput);


       
        
    }

    private void OnMovementStop(InputValue inputValue)
    {
        movementInput = Vector3.zero; //new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);

        //Debug.Log("stopped movement " + movementInput);
    }
}
