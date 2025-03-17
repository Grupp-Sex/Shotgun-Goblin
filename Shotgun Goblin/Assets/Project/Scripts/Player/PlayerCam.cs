using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public Transform orientation;

    public float sensitivity = 5f;

    float mouseX;
    float mouseY;

    private Vector2 lookInput;
    

    float xRotation;
    float yRotation;

    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   
    void Update()
    {
        //Scaling mouse movement med delta time (Tid mellan varje frame) och sensitivity för att inte skapa inconsintency
        mouseX *= Time.deltaTime * sensitivity;
        mouseY *= Time.deltaTime * sensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Begränsar spelar från att titta mer än 90 grader upp och ner

        //rotera kamera åt bägge axis (x och y)
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //rotera orientation (vilket är spelaren) på y-axeln endast 
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        //Debug för att se ifall kameran tar emot mouse inputs
        Debug.Log(lookInput);

    }

    //OnLook tar player inputs från musen och förvarar dem i mouseX och mouseY beroende på om player tittar horizontelt (X) eller vertikalt (Y)
    private void OnLook(InputValue input)
    {
        lookInput = input.Get<Vector2>();
        mouseX = lookInput.x;
        mouseY = lookInput.y;
        //mouseX = input.Get<Vector2>().x;
        //mouseY = input.Get<Vector2>().y;
    }
}
