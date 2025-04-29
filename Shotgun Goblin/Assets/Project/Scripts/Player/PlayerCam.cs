using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    
   private PlayerMovement playerMovement;

    public Rigidbody yTransform;
    public Rigidbody xTransform;

    


    [SerializeField] private float sensitivity = 5f;

    float mouseX;
    float mouseY;
   

    private Vector2 lookInput;

    
    public float xRotation { get; protected set; }
    public float yRotation { get; protected set; }





    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   
    void LateUpdate()
    {
        //Scaling mouse movement med delta time (Tid mellan varje frame) och sensitivity för att inte skapa inconsintency
        mouseX *= Time.deltaTime * sensitivity;
        mouseY *= Time.deltaTime * sensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -85f, 85f); // Begränsar spelar från att titta mer än 90 grader upp och ner

       
       
        //rotera orientation (vilket är spelaren) på y-axeln endast 

        yTransform.MoveRotation(Quaternion.Euler(yTransform.rotation.x, yRotation, yTransform.rotation.z));

        xTransform.MoveRotation(Quaternion.Euler(xRotation, yRotation, xTransform.rotation.z));


        //currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
       
    }

    
    //OnLook tar player inputs från musen och förvarar dem i mouseX och mouseY beroende på om player tittar horizontelt (X) eller vertikalt (Y)
    private void OnLook(InputValue input)
    {
        lookInput = input.Get<Vector2>();
        mouseX = lookInput.x;
        mouseY = lookInput.y;
        
    }

  

}
