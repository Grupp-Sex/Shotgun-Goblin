using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.UI.Image;


// TO DO
// SEPARATE GOTSHOTLOGIC INTO NEW IHITLOGIC SCRIPT

public class BaseGun : MonobehaviorScript_ToggleLog, IHeldItem, IUserReference
{
    [SerializeField] float baseDamage;
    [SerializeField] int MaxRoudsLoaded = 2;
    [SerializeField] int CurrentRoudsLoaded;
    [SerializeField] float ReloadTimer = 2;
    [SerializeField] bool IsReloading;

    protected GameObject gunUser;

    public bool GetIsReloading => IsReloading;

    protected IHitLogic[] hitLogicScripts;
    protected IShotActivated[] shootActivatedScripts;

    void Start()
    {
        hitLogicScripts = GetComponents<IHitLogic>();
        shootActivatedScripts = GetComponents<IShotActivated>();



    }

    
    public void SetUser(GameObject user)
    {
        gunUser = user;
    }

    public void DoAction()
    {
        CheckReload();
    }

    public bool CanShoot()
    {
        return !IsReloading && CurrentRoudsLoaded > 0;
    }

    protected virtual void CheckReload()
    {
        if (!GetIsReloading)
        {
            if (CurrentRoudsLoaded > 0)
            {
                CurrentRoudsLoaded--;
                DebugLog("Gun Shot, rounds left: " + CurrentRoudsLoaded);
                Shoot();

            }
            if(!(CurrentRoudsLoaded > 0))
            {
                Reload();
            }
        }
    }

    protected virtual void Shoot()
    {
        NotifyShotActivated();
    }

    public void Reload()
    {
        if (!GetIsReloading)
        {
            //Micke modifierade i interface och här
            NotifyReloadActivated();

            StartCoroutine(DoReloadTimer(ReloadTimer));
        }
    }

    protected IEnumerator DoReloadTimer(float timer)
    {
        IsReloading = true;
        DebugLog("Reloading Started");
        yield return new WaitForSeconds(timer);

        CurrentRoudsLoaded = MaxRoudsLoaded;
        IsReloading = false;
        DebugLog("Reloading Finished, rouns left: " + CurrentRoudsLoaded);

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
        else
        {
            Miss(direction, maxDistance, origin);
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
        NotifyProjectileActivated(projectile);
        NotifyHitLogic(hitInfo, projectile);
    }

    protected virtual void Miss(Vector3 direction, float length, Vector3 origin)
    {
        ProjectileInfo projectile = CreateProjectileInfo(direction, 0, origin, origin + direction * length);
        NotifyProjectileActivated(projectile);
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

    public virtual void NotifyProjectileActivated(ProjectileInfo projectile)
    {
        foreach (var shot in shootActivatedScripts)
        {
            shot.RunProjectileLogic(projectile);

        }
    }

    //Micke lade till denna 
    public virtual void NotifyReloadActivated()
    {
        foreach (var shot in shootActivatedScripts)
        {
            shot.RunReloadLogic();
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

//Used in shotgun animations script as well (some of it)
public interface IShotActivated
{
    public void RunShootLogic();

    public void RunProjectileLogic(ProjectileInfo projectile);

    public void RunReloadLogic();
}
public interface IHitLogic
{
    public void RunHitLogic(RaycastHit hitinfo, ProjectileInfo projectile);
}