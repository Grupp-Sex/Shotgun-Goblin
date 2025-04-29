using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Mesh;

public abstract class BaseDash : MonobehaviorScript_ToggleLog
{

    

    [SerializeField] GameObject DashObject;


    protected void NotifyDashObject(DashData dashData)
    {
        if (DashObject != null) 
        {
            IOnDash[] onDashScripts =DashObject.GetComponents<IOnDash>();

            for (int i = 0; i < onDashScripts.Length; i++)
            {
                onDashScripts[i].OnDash(dashData);
            }
        }
    }

    
    public virtual void Dash(DashData dashData, Rigidbody rb)
    {
        dashData = AlterDashData(dashData);
        ApplyDash(dashData, rb);
    }

    protected virtual DashData AlterDashData(DashData dashData)
    {
        return dashData.CloneViaFakeSerialization();
    }

    protected virtual void ApplyDash(DashData dashData, Rigidbody rb)
    {
        Vector3 force = GetDashForce(dashData);

        if (dashData.truncateDirection != Vector3.zero)
        {
            TrucateVelocity(rb, dashData.truncateDirection);
        }

        if (dashData.worldForce)
        {
            rb.AddForce(force, dashData.forceMode);
        }
        else
        {
            rb.AddRelativeForce(force, dashData.forceMode);
        }

        


        DebugLog("Dash, force = " + force);
       
    }


    protected virtual Vector3 GetDashForce(DashData dashData)
    {
        Vector3 force = dashData.direction * dashData.velocity;

        return force;
    }

    protected virtual void TrucateVelocity(Rigidbody rb, Vector3 direction)
    {
        rb.velocity = rb.transform.TransformVector(TruncateVector(rb.velocity, direction));
    }

    protected virtual Vector3 TruncateVector(Vector3 vector, Vector3 truncator)
    {
        Vector3 result = vector;

        if (Vector3.Dot(vector, truncator) < 0)
        {
            result = Vector3.ProjectOnPlane(vector, truncator);
        }

        return result;
    }
    
}

public interface IOnDash
{
    public void OnDash(DashData dashData);
}

[System.Serializable]
public struct DashData
{



    public Vector3 unalterd_direction;

    public Vector3 direction => unalterd_direction.normalized;

    public float velocity;

    public Vector3 truncateDirection;

    public bool worldForce;

    public ForceMode forceMode;

    public void Initialize(Vector3 dash_direction, float dash_velocity)
    {
        velocity = dash_velocity;
        unalterd_direction = dash_direction;
    }
    
}
