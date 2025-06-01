/*
 * PlayerDash.cs
 * 
 * Handles dash input and execution for a player character.
 * Inherits from BaseDash, which defines shared dash logic like ApplyDash() and DashData.
 * Supports two types of dash:
 * - Shift Dash: Activated using the left shift key.
 * - Key Double-Tap Dash: Activated by double-tapping directional keys (e.g., WASD).
 * 
 * Features:
 * - Dash cooldown system to prevent spamming
 * - Directional dashing using input from PlayerMovement or key taps
 * - Multitap support with a time threshold (0.5s) to detect double-tap behavior
 * - Fallback base dash if no directional input is provided
 * - Dashes apply force using Rigidbody and configurable ForceMode
 * 
 * Serialized Fields:
 * - cam: Player camera transform (optional, not directly used in current script)
 * - orientation: Player orientation transform (optional, not directly used)
 * - dashForce: Strength of the dash impulse
 * - baseDash: Default dash direction if no input is detected
 * - dashDuration: Duration of the dash effect
 * - dashCD: Cooldown period between dashes
 * - canDash: Internal cooldown flag
 * 
 * Dependencies:
 * - Requires Rigidbody component on the same GameObject
 * - Requires PlayerMovement component to get input direction
 * - Requires PlayerSettings.dash_shift to be enabled to allow dashing
 * 
 * Key Methods:
 * - OnDashStart(): Handles dashing via shift key
 * - OnKeysDashStart(): Handles directional key double-tap dashing
 * - Dash(): Coroutine that applies dash force and manages cooldown
 * - KeysDash(): Detects direction from cumulative tap count and triggers dash
 * - CheckDirection(): Validates and clamps input direction
 * - CreateDashData(): Prepares DashData to pass into BaseDash logic
 * 
 * Author:
 * - Originally written by Mikael
 * - Updated and extended by Ansgar
 */


using System.Collections;
using System.Collections.Generic;
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
        if (!canDash || !PlayerSettings.dash_dubbleTap) return;


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
