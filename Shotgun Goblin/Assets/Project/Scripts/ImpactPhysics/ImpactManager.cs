using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonobehaviorScript_ToggleLog
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [SerializeField] float KEsumFalloff;

    [SerializeField] float LowerThreshold;
    

    private List<ContactPoint> contactPoints_Pool = new List<ContactPoint>();
    private List<Vector3> contactVector3_Pool = new List<Vector3>();

    [SerializeField] float KESum;
    [SerializeField] float KESumMax;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionStay(Collision collision)
    {
        Colition(collision);
        

    }

    private void FixedUpdate()
    {
        KESum *= KEsumFalloff + 1;
    }


    protected virtual void Colition(Collision collision)
    {
        
        Rigidbody coliderRB = collision.rigidbody;

        if(rb == null && coliderRB == null)
        {
            return;
        }

        Vector3 relativeV = collision.relativeVelocity;

        if(relativeV.sqrMagnitude > LowerThreshold)
        {
            //Vector3 colitionPoint = AvragePoint(GetAllPoints(collision));

            float kineticEnergy = CalculateKineticEnergy(rb, coliderRB, relativeV);

            //", number of points: " + collision.contactCount + ", avrage point " + colitionPoint +
            DebugLog("Collision: " + collision.collider.name +  ", relative velocity: " + relativeV + ", kinetic energy: " + kineticEnergy);
            KESum += kineticEnergy;

            if(KESum > KESumMax)
            {
                KESumMax = KESum;
            }
        }

        

    }


    protected List<Vector3> GetAllPoints(Collision collision)
    {
        contactPoints_Pool.Clear();
        contactVector3_Pool.Clear();
        collision.GetContacts(contactPoints_Pool);

        

        for(int i = 0; i < contactPoints_Pool.Count; i++)
        {
            contactVector3_Pool.Add(contactPoints_Pool[i].point);
        }
        
        return contactVector3_Pool;

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

    protected float CalculateKineticEnergy(Rigidbody personalRB, Rigidbody colliderRB, Vector3 relativeV)
    {
        float kineticEnergy = 0;

        float VSqrd = relativeV.sqrMagnitude;

        float massCalculation = 0;

        switch (personalRB, colliderRB)
        {
            case (not null, not null): // both objects are rigidbodys

                massCalculation = ( personalRB.mass * colliderRB.mass) / (personalRB.mass + colliderRB.mass);

                break;

            case ( null, not null): // the colider allone is a rigidibody

                massCalculation =  colliderRB.mass;

                break;

            case (not null, null):// script holder allone is a rigidibody

                massCalculation = personalRB.mass;

                break;

        }

        kineticEnergy = 0.5f * massCalculation * VSqrd; 


        return kineticEnergy;
    }

    
    

    

    
}

public struct ColitionData
{
    public Vector3 relativeVelocity;
    public Vector3 avragePoint;
    public Vector3 colider_momentum;
}




