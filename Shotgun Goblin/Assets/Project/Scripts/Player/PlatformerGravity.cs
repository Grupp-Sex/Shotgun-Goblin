using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerGravity : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public PlayerMovement Movement;

    [SerializeField] private float downMult = 1;
    [SerializeField] private float uppMult = 0;
    [SerializeField] private bool applyGravity;
    [SerializeField] private bool useJumping;

    public bool fallQuickly = false;

    void Awake()
    {
        if(Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }   

        if(Movement == null)
        {
            Movement = GetComponent<PlayerMovement>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        CheckFalling();
        RunPlatformerGravity();
    }

    protected void CheckFalling()
    {
        bool isMoveingUp = Vector3.Dot(Rigidbody.velocity, Physics.gravity) < 0;
        bool isJumping = isMoveingUp;


        if (Movement != null && useJumping)
        {
            isJumping = Movement.isJumping;
        }

        fallQuickly = !(isMoveingUp && isJumping);


    }

    protected void RunPlatformerGravity()
    {
        Vector3 gravity = Physics.gravity;

        if (fallQuickly)
        {
            ApplyGravity(gravity, downMult);
        }
        else
        {

            ApplyGravity(gravity,uppMult);
        }
    }

    protected void ApplyGravity(Vector3 gravity, float mult)
    {
        if (mult != 0)
        {
            Rigidbody.AddForce(gravity * mult, ForceMode.Acceleration);
        }
    }

    
}
