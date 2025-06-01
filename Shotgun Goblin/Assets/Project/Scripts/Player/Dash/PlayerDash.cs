using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Original written by Mikael, updated by Ansgar
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


    private Vector3 tappCount;
    
    

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
        if (!canDash || !PlayerSettings.dash_shift) return;

        // Store input direction by player movement to be able to dash in any direction 

        Vector3 dashDirection = movement.GetInputDirection();
        StartCoroutine(Dash(dashDirection));
    }
    private void OnKeysDashStart(InputValue input)
    {
        if (!canDash || !PlayerSettings.dash_shift) return;


        if (tappCount.sqrMagnitude <= 0)
        {
            StartCoroutine(Multitap(0.5f));
        }

        Vector3 inputV = input.Get<Vector3>();
        Vector3 dashDirection = new Vector3(inputV.x, 0, inputV.y);

        tappCount += dashDirection;

        KeysDash(tappCount);

        
    }

    private IEnumerator Multitap(float duration)
    {
        yield return new WaitForSeconds(duration);

        tappCount = Vector3.zero;
    }

    protected void KeysDash(Vector3 input)
    {

        Vector3 dash;

        if (CheckDirection(new Vector3(1, 0, 0), input, out dash))
        {
            StartCoroutine(Dash(dash));
            tappCount = Vector3.zero;
            return;
        }
        else if (CheckDirection(new Vector3(0, 0, 1), input, out dash))
        {
            StartCoroutine(Dash(dash));
            tappCount = Vector3.zero;
            return;
        }

        //tappCount = Vector3.zero;

    }

    protected bool CheckDirection(Vector3 direction, Vector3 input, out Vector3 fixedInput)
    {
        fixedInput = new Vector3(direction.x * input.x, direction.y * input.y, direction.z * input.z);

        return fixedInput.sqrMagnitude > 1; 
    }

    


    //Coroutine for dash behaviour
    IEnumerator Dash(Vector3 direction)
    {

        canDash = false;

        //movement.WheelBreaksOff();
        
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

        //movement.WheelBreaksOn();

        yield return new WaitForSeconds(dashCD);

        canDash = true;
    }

    protected DashData CreateDashData(Vector3 direction)
    {
        return new DashData()
        {
            forceMode = ForceMode.Impulse,
            truncateDirection = direction,
            unalterd_direction = direction,
            velocity = dashForce,
            worldForce = false,
        };
    } 
}
