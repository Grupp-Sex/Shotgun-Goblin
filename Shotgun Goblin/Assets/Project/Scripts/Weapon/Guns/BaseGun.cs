using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.UI.Image;


// TO DO
// SEPARATE GOTSHOTLOGIC INTO NEW IHITLOGIC SCRIPT

public class BaseGun : MonobehaviorScript_ToggleLog, IHeldItem
{
    [SerializeField] float baseDamage;
    protected IHitLogic[] hitLogicScripts;
    protected IShotActivated[] shootActivatedScripts;

    void Start()
    {
        hitLogicScripts = GetComponents<IHitLogic>();
        shootActivatedScripts = GetComponents<IShotActivated>();



    }

    public void DoAction()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        NotifyShotActivated();
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
    
    protected virtual ProjectileInfo CreateProjectileInfo(Vector3 direction, float damage, Vector3 origin, Vector3 hitPos)
    {
        ProjectileInfo projectile = new ProjectileInfo()
        {
            direction = direction,
            damage = damage,
            origin = origin,
            hitPos = hitPos
        };
        return projectile;
    }

    protected virtual void Hit(Vector3 direction, RaycastHit hitInfo, Vector3 origin)
    {
        ProjectileInfo projectile = CreateProjectileInfo(direction,GetDamage(hitInfo),origin,hitInfo.point);
        NotifyHitLogic(hitInfo, projectile);
    }

    public virtual void NotifyHitLogic(RaycastHit hitinfo, ProjectileInfo projectile)
    {
        foreach (var hit in hitLogicScripts)
        {
            hit.RunHitLogic(hitinfo, projectile);
        }
    }
    public virtual void NotifyShotActivated()
    {
        foreach (var shot in shootActivatedScripts)
        {
            shot.RunShootLogic();
        }
    }

}




//Cointains all projectile info for the bullets 
public struct ProjectileInfo
{
    public Vector3 direction;
    public float damage;
    public Vector3 origin;
    public Vector3 hitPos;
    
}


public interface IShotActivated
{
    public void RunShootLogic();
}
public interface IHitLogic
{
    public void RunHitLogic(RaycastHit hitinfo, ProjectileInfo projectile);
}