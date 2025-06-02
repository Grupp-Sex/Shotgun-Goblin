using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpactManager : MonobehaviorScript_ToggleLog
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [SerializeField] float SumFalloff;

    [SerializeField] float LowerThreshold = 0.1f;
    [SerializeField] float TruncationLerpValue;

    private List<ContactPoint> contactPoints_Pool = new List<ContactPoint>();
    private List<Vector3> contactVector3_Pool = new List<Vector3>();

    [SerializeField] float KESum_relative;
    [SerializeField] float KESumMax_relative;


    [SerializeField] float MomentumSum_relative;
    [SerializeField] float MomentumSumMax_relative;


    [SerializeField] float KESum_collider;
    [SerializeField] float KESumMax_collider;


    [SerializeField] float MomentumSum_collider;
    [SerializeField] float MomentumSumMax_collider;

    protected IImapctManagerNotify[] imapctNotifyScripts;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        imapctNotifyScripts = GetComponents<IImapctManagerNotify>();


    }

    private void OnCollisionStay(Collision collision)
    {
        Colition(collision);
        

    }

    private void Update()
    {
        KESum_relative *= SumFalloff * Time.deltaTime * 60;
        KESum_collider *= SumFalloff * Time.deltaTime * 60;

        MomentumSum_relative *= SumFalloff * Time.deltaTime * 60;
        MomentumSum_collider *= SumFalloff * Time.deltaTime * 60;
        
    }

    protected void Notify(CollisionData collision)
    {
        for(int i = 0; i < imapctNotifyScripts.Length; i++)
        {
            imapctNotifyScripts[i].OnNotifyedCollision(collision);
        }
    }

    protected virtual void Colition(Collision collision)
    {
        
        Rigidbody coliderRB = collision.rigidbody;

        if(rb == null && coliderRB == null)
        {
            return;
        }

        Vector3 relativeColitionV = collision.relativeVelocity;

        



        if(relativeColitionV.sqrMagnitude > LowerThreshold * LowerThreshold)
        {
            Vector3 collisionNormal = collision.GetContact(0).normal;

            Vector3 relativeV = Vector3.Project(relativeColitionV,collisionNormal);

            relativeV = relativeV * (1 - TruncationLerpValue) + relativeColitionV * TruncationLerpValue;

            //DebugLog("" +collisionNormal + " " + collision.gameObject.name);

            if (relativeV.sqrMagnitude > LowerThreshold * LowerThreshold)
            {

                //Vector3 colitionPoint = AvragePoint(GetAllPoints(collision));

                float relativeMass = CalculateRelativeMass(rb, coliderRB);

                float relative_kineticEnergy = CalculateKineticEnergy(relativeMass, relativeV);

                float relative_momentum = CalculateMomentum(relativeMass, relativeV);



                //", number of points: " + collision.contactCount + ", avrage point " + colitionPoint +
                DebugLog("Collision: " + collision.collider.name + ", relative velocity: " + relativeV + ", kinetic energy: " + relative_kineticEnergy + ", momentum: " + relative_momentum);

                KESum_relative += relative_kineticEnergy;
                if (KESum_relative > KESumMax_relative)
                {
                    KESumMax_relative = KESum_relative;
                }

                MomentumSum_relative += relative_momentum;
                if (MomentumSum_relative > MomentumSumMax_relative)
                {
                    MomentumSumMax_relative = MomentumSum_relative;
                }




                if (RigidbodyExists(coliderRB))
                {
                    float collider_kineticEnergy = CalculateKineticEnergy(coliderRB.mass, relativeV);
                    float collider_momentum = CalculateMomentum(coliderRB.mass, relativeV);

                    KESum_collider += collider_kineticEnergy;
                    if (KESum_collider > KESumMax_collider)
                    {
                        KESumMax_collider = KESum_collider;
                    }

                    MomentumSum_collider += relative_momentum;
                    if (MomentumSum_collider > MomentumSumMax_collider)
                    {
                        MomentumSumMax_collider = MomentumSum_collider;
                    }
                }


                Vector3 position = AvragePoint(GetAllPoints(collision));

                CollisionData collisison = CollisionData.CreateColitionData
                    (
                        relativeColitionV,
                        relativeV,
                        position,
                        collisionNormal,
                        MomentumSum_relative,
                        KESum_relative,
                        KESum_collider,
                        MomentumSum_collider
                    );

                Notify(collisison);
            }
        }

        

    }

    protected bool RigidbodyExists(Rigidbody rb)
    {
        if(rb == null) return false;
        if(rb.isKinematic) return false;

        return true;
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

    protected float CalculateRelativeMass(Rigidbody personalRB, Rigidbody colliderRB)
    {
        float massCalculation = 0;

        (bool, bool) rbExists = (RigidbodyExists(personalRB), RigidbodyExists(colliderRB));

        switch (rbExists)
        {
            case (true, true): // both objects are rigidbodys

                massCalculation = (personalRB.mass * colliderRB.mass) / (personalRB.mass + colliderRB.mass);

                break;

            case (false, true): // the colider allone is a rigidibody

                massCalculation = colliderRB.mass;

                break;

            case (true, false):// script holder allone is a rigidibody

                massCalculation = personalRB.mass;

                break;

        }

        return massCalculation;

    }

    

    protected float CalculateKineticEnergy(float massCalculation, Vector3 relativeV)
    {
        
        float VSqrd = relativeV.sqrMagnitude;


        float kineticEnergy = 0.5f * massCalculation * VSqrd; 


        return kineticEnergy;
    }

    protected float CalculateMomentum(float massCalculation, Vector3 relativeV)
    {
        float V = relativeV.magnitude;

        float momentum = massCalculation * V;

        return momentum;
        
    }

    
        
}

public interface IImapctManagerNotify
{

    public void OnNotifyedCollision(CollisionData collition);

}

public struct CollisionData
{
    public static CollisionData CreateColitionData(Vector3 relativeVelocity, Vector3 truncatedRelativeVelocity, Vector3 position, Vector3 normalVector, float momentum, float kineticEnergy, float collider_momentum, float collider_kinematicEnergy)
    {
        return new CollisionData()
        {
            relativeVelocity = relativeVelocity,
            truncatedRelativeVelocity = truncatedRelativeVelocity,
            position = position,
            normalVector = normalVector,
            kineticEnergy = kineticEnergy,
            momentum = momentum,
            colider_kineticEnergy = collider_kinematicEnergy,
            colider_momentum = collider_momentum
        };

    }

    public Vector3 relativeVelocity;
    public Vector3 truncatedRelativeVelocity;

    public Vector3 position;
    public Vector3 normalVector;

    public float momentum;
    public float kineticEnergy;

    public float colider_momentum;
    public float colider_kineticEnergy;
    
}




