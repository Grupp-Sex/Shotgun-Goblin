using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FovBySpeed : MonoBehaviour
{


    // unused

    public Camera Camera;

    public Rigidbody rigidbody;

    public bool useRigidbody;

    [SerializeField] protected float minSpeed = 0.05f;
    [SerializeField] protected float midSpeed = 0.1f;
    [SerializeField] protected float maxSpeed = 0.4f;

    //protected float speedRange => Mathf.Abs(maxSpeed - minSpeed);


    //[SerializeField] AnimationCurve fovCurve;
    [SerializeField] protected float fovMid = 70;
    [SerializeField] protected float fovMax = 90;
    private float fovMin;
    [SerializeField] private float targetFOV;

    [SerializeField] private float currentFOV;
    [SerializeField] private float currentFovLerpValue = 0.5f;
    

    [SerializeField] private float speed;
    [SerializeField] private float speedLerp = 0.9f;



    protected Vector3 lastPosition;
    protected Vector3 velocity;

    // Start is called before the first frame update
    void Awake()
    {
        if(Camera == null)
        {
            Camera = GetComponent<Camera>();
        }
        fovMin = Camera.fieldOfView;
        currentFOV = fovMin;
        targetFOV = fovMin;

        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        if(rigidbody == null)
        {
            useRigidbody = false;
        }
        
    }

    
    void FixedUpdate()
    {
        CalculateTransformVelocity();
        GetSpeed();
        CalculateFOV(speed);
        UpdateFOV();
        //SetFOV(currentFOV);
    }

    private void Update()
    {
        SetFOV(currentFOV);
    }


    protected void GetSpeed()
    {
        float currntSpeed = 0;
        if (useRigidbody)
        {
            currntSpeed = rigidbody.velocity.magnitude;
        }
        else
        {
            currntSpeed = velocity.magnitude;
        }

        speed = Lerp(speed, currntSpeed, speedLerp);
    }

    protected void CalculateTransformVelocity()
    {
        velocity = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    //protected void CalculateFOV(float speed)
    //{
    //    float curvePos = Mathf.Clamp01((speed- minSpeed) / speedRange);

    //    float lerpValue = fovCurve.Evaluate(curvePos);


    //    targetFOV = Lerp(targetFOV, Lerp(fovMin, fovMax, lerpValue), targetFovLerpValue);

    //}

    protected void CalculateFOV(float speed)
    {
        float fov = fovMin;
        if (speed < minSpeed) fov = fovMin;
        else if(speed > maxSpeed ) fov = fovMid;
        //else if(speed >)
        else return;

        targetFOV = fov; //Lerp(targetFOV, fov, targetFovLerpValue);

    }

    protected float Lerp(float x, float y, float t)
    {
        return x * (1 - t) + y * t;
    }

    protected void UpdateFOV()
    {
        currentFOV = Lerp(currentFOV, targetFOV, currentFovLerpValue);
    }

    protected void SetFOV(float fov)
    {
        if(Camera != null)
        {
            Camera.fieldOfView = fov;
        }
    }


}
