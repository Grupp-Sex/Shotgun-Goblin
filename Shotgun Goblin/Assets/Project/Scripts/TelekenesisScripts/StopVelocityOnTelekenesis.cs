using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopVelocityOnTelekenesis : MonoBehaviour, IOnTelekenesisEnter
{
    [SerializeField] float interpolation = 0;

    public Rigidbody Rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        if(Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }      
    }

    public void OnTelekenesisEnter()
    {
        Rigidbody.velocity = Rigidbody.velocity * interpolation;
    }
}
