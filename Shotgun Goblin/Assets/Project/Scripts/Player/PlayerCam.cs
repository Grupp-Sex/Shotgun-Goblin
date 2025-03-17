using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    //public Transform orientation;
    public Transform yTransform;
    public Transform xTransform;

    public float sensitivity = 5f;

    float mouseX;
    float mouseY;
    //[SerializeField] private float turnTime;

    private Vector2 lookInput;

    //float xRotationVelocity;
    //float yRotationVelocity;
    float xRotation;
    float yRotation;

    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   
    void LateUpdate()
    {
        //Scaling mouse movement med delta time (Tid mellan varje frame) och sensitivity f�r att inte skapa inconsintency
        mouseX *= Time.deltaTime * sensitivity;
        mouseY *= Time.deltaTime * sensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Begr�nsar spelar fr�n att titta mer �n 90 grader upp och ner

        //rotera kamera �t b�gge axis (x och y)
        //transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //rotera orientation (vilket �r spelaren) p� y-axeln endast 


        yTransform.rotation = Quaternion.Euler(0, yRotation, 0);
        xTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0);


    }

    
    //OnLook tar player inputs fr�n musen och f�rvarar dem i mouseX och mouseY beroende p� om player tittar horizontelt (X) eller vertikalt (Y)
    private void OnLook(InputValue input)
    {
        lookInput = input.Get<Vector2>();
        mouseX = lookInput.x;
        mouseY = lookInput.y;
        //mouseX = input.Get<Vector2>().x;
        //mouseY = input.Get<Vector2>().y;
    }
}
