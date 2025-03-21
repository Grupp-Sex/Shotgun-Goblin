using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.UI.Image;


// TO DO
// SEPARATE GOTSHOTLOGIC INTO NEW IHITLOGIC SCRIPT

public class BaseGun : MonoBehaviour
{
    [SerializeField] float baseDamage;
    protected IHitLogic[] hitLogicScripts;

    void Start()
    {
        hitLogicScripts = GetComponents<IHitLogic>();
    }

    protected virtual float GetDamage(RaycastHit hitinfo)
    {
        return baseDamage;
    }

    // Shoots one projectile one time, use it multiple times for shotgun!
    protected virtual void ShootOneTime(Vector3 origin, Vector3 direction, float maxDistance)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, maxDistance))
        {
            Hit(direction,hitInfo,origin);
        }
    }
    
    protected virtual ProjectileInfo CreateProjectileInfo(Vector3 direction, float damage, Vector3 origin)
    {
        ProjectileInfo projectile = new ProjectileInfo()
        {
            direction = direction,
            damage = damage,
            origin = origin
        };
        return projectile;
    }

    protected virtual void Hit(Vector3 direction, RaycastHit hitInfo, Vector3 origin)
    {
        ProjectileInfo projectile = CreateProjectileInfo(direction,GetDamage(hitInfo),origin);
        NotifyHitLogic(hitInfo, projectile);
    }

    public virtual void NotifyHitLogic(RaycastHit hitinfo, ProjectileInfo projectile)
    {
        foreach (var hit in hitLogicScripts)
        {
            hit.RunHitLogic(hitinfo, projectile);
        }
    }

    
}




//Cointains all projectile info for the bullets 
public struct ProjectileInfo
{
    public Vector3 direction;
    public float damage;
    public Vector3 origin;
    
}



public interface IHitLogic
{
    public void RunHitLogic(RaycastHit hitinfo, ProjectileInfo projectile);
}