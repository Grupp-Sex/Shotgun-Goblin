/*
 * PlayerCam.cs
 * 
 * Handles player camera movement using mouse input via Unity's Input System.
 * Rotates the player (y-axis) and the camera (x-axis) independently for first-person.
 * Locks the cursor and applies input sensitivity scaling to ensure consistent behavior across varying frame rates.
 * 
 * Features:
 * - Horizontal and vertical rotation using Rigidbody
 * - Clamping vertical rotation to prevent over-rotation
 * - Cursor locking and hiding for immersive gameplay
 * - Input system integration for look controls
 * 
 * Dependencies:
 * - Requires Rigidbody references for xTransform (camera) and yTransform (player body or orientation root)
 * - Must be paired with Unity Input System with "Look" action mapped
 * 
 * Author:
 * - Written by Mikael
 */




using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

//Written by Mikael
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
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            //Scaling mouse movement med delta time (Tid mellan varje frame) och sensitivity för att inte skapa inconsintency
            mouseX *= 1 / 60f * sensitivity;
            mouseY *= 1 / 60f * sensitivity;

            yRotation += mouseX;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -85f, 85f); // Begränsar spelar från att titta mer än 90 grader upp och ner



            //rotera orientation (vilket är spelaren) på y-axeln endast 

            yTransform.MoveRotation(Quaternion.Euler(yTransform.rotation.x, yRotation, yTransform.rotation.z));

            xTransform.MoveRotation(Quaternion.Euler(xRotation, yRotation, xTransform.rotation.z));


            
        }
       
    }

    
    //OnLook tar player inputs från musen och förvarar dem i mouseX och mouseY beroende på om player tittar horizontelt (X) eller vertikalt (Y)
    private void OnLook(InputValue input)
    {
        lookInput = input.Get<Vector2>();
        mouseX = lookInput.x;
        mouseY = lookInput.y;
        
    }

  

}
