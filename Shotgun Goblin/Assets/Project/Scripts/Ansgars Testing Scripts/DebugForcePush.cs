using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugForcePush : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float ForceMult;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.AddForce(new Vector3(0,0,1) * ForceMult);
        }

    }
}
