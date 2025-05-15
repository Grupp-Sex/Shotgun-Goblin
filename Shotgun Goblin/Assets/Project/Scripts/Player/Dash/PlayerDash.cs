using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;
    private PlayerMovement movement;
    private Rigidbody playerRB;



    [Header("Dash Values")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCD;
    private bool canDash = true;


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


    private void OnDashStart()
    {
        if (!canDash) return;

        // Store input direction by player movement to be able to dash in any direction 

        Vector3 dashDirection = movement.GetInputDirection();
        StartCoroutine(Dash(dashDirection));
    }

    //private void dash()
    //{
    //    Vector3 forceToApply = orientation.forward * dashForce;

    //    playerRB.AddForce(forceToApply, ForceMode.Impulse);

    //}

    IEnumerator Dash(Vector3 direction)
    {

        canDash = false;
        Vector3 forceToApply = direction.normalized * dashForce;

        playerRB.velocity = Vector3.zero;
        playerRB.AddForce(forceToApply, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}
