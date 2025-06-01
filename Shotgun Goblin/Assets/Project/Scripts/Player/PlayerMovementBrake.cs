using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBrake : MonoBehaviour
{
    public PlayerMovement movement;
    public GameObject BrakeObject;

    // Start is called before the first frame update
    void Awake()
    {
        if (movement == null)
        {
            movement = GetComponent<PlayerMovement>();
        }
    }



    private void OnEnable()
    {
        movement.Event_ToggleBrake.Subscribe(Event_PlayerBrake);
    }

    private void OnDisable()
    {
        movement.Event_ToggleBrake.UnSubscribe(Event_PlayerBrake);
    }

    protected void Event_PlayerBrake(object sender, bool toggle)
    {
        BrakeObject.SetActive(toggle);
    }

}
