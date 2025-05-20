using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : BaseDash
{

    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;
    private PlayerMovement movement;
    private Rigidbody playerRB;



    [Header("Dash Values")]
    [SerializeField] private float dashForce;
    [SerializeField] private Vector3 baseDash = Vector3.forward;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCD;
    [SerializeField] private bool canDash = true;

    

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Do when pressing left shift
    private void OnDashStart()
    {
        if (!canDash) return;

        // Store input direction by player movement to be able to dash in any direction 

        Vector3 dashDirection = movement.GetInputDirection();
        StartCoroutine(Dash(dashDirection));
    }

    //Coroutine for dash behaviour
    IEnumerator Dash(Vector3 direction)
    {

        canDash = false;

        movement.WheelBreaksOff();
        
        //Vector3 forceToApply = direction.normalized * dashForce;

        //playerRB.velocity = Vector3.zero;

        //Added force when dash is initiated in game in player local space, not world space
        //playerRB.AddRelativeForce(forceToApply, ForceMode.Impulse);
        if(direction.sqrMagnitude <= 0.0001f * 0.0001f)
        {
            direction = baseDash;
        }
        

        ApplyDash(CreateDashData(direction), playerRB);

        yield return new WaitForSeconds(dashDuration);

        movement.WheelBreaksOn();

        yield return new WaitForSeconds(dashCD);

        canDash = true;
    }

    protected DashData CreateDashData(Vector3 direction)
    {
        return new DashData()
        {
            forceMode = ForceMode.Impulse,
            truncateDirection = -direction,
            unalterd_direction = direction,
            velocity = dashForce,
            worldForce = false,
        };


    } 
}
