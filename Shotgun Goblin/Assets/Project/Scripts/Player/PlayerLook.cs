using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
   [SerializeField] private int mouseSensitivity; 

    public Transform orientation; //reference to the player's transform so we can rotate it


     float xRotation, yRotation; //Variables to store rotation values
     float mouseX, mouseY; //Variables to store mouse X & Y Axis movement


    // Start is called before the first frame update
    void Start()
    {
        //Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        //Scaling mouse movement by deltatime and sensitivity
        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Makes the camera unable to look more than 90 degrees up and down

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }

    private void OnLook(InputValue input)
    {
        //Getting mouse input values
        mouseX = input.Get<Vector2>().x;
        mouseY = input.Get<Vector2>().y;
    }
}
