using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonobehaviorScript_ToggleLog
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        DebugLog("Collision Detected");
    }

   

    protected Vector3 AvragePoint(List<Vector3> points)
    {
        Vector3 pointSum = Vector3.zero;

        int pointCount = points.Count;

        for (int i = 0; i < pointCount; i++)
        {
            pointSum += points[i];
        }

        Vector3 pointAvrage = pointSum / pointCount;

        return pointAvrage;

    }


    protected float CalculateKineticEnergy(ColiderData colider)
    {
        float speed = colider.speed;
        float mass = colider.mass;



        return 0;
    }

    protected float CalculateMomentumAtPoint(Rigidbody rigidBody, Vector3 point)
    {
        Vector3 velocity = rigidBody.GetPointVelocity(point);
        float mass = rigidBody.mass;



        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct ColiderData
{
    public float mass;
    public float speed; 
}




