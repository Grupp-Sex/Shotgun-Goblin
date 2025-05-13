using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [Header("References")]
    public PlayerCam cam;
    private PlayerMovement movement;
    

    //Vet inte för tillfället hur Ansgars kod fungerar så vi kan byta till inheritance
    private Rigidbody rb;

    [Header("Dashing")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDashStart()
    {

    }

    private void dash()
    {
        Vector3 forceToApply = 
    }
}
