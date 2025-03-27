using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingForce : MonoBehaviour
{
    [SerializeField] Vector3 startingForce;
    [SerializeField] Vector3 continualForce;
    protected Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(startingForce, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (continualForce != Vector3.zero)
        {
            rb.AddForce(continualForce, ForceMode.VelocityChange);
        }
    }
}
