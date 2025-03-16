using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.UI.Image;

public class BaseGun : MonoBehaviour
{    
    void Start()
    {
        
    }
   
    void Update()
    {

    }
    // Shoots one projectile one time, use it multiple times for shotgun!
    protected virtual void ShootOneTime(Vector3 origin, Vector3 direction, float maxDistance)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, maxDistance))
        {
            Hit(origin,direction,hitInfo);
        }
    }
    //Triggers all IShootAble scripts that can be hit for the hit object, All things that needs to be
    //shot will have this script so this will just trigger it.
    protected virtual void ActivateGotShotLogic(GameObject hitObject, ProjectileInfo projectileinfo)
    {
        IShootAble[] shootablecomponents = hitObject.GetComponents<IShootAble>();
        for (int i = 0; i < shootablecomponents.Length; i++)
        {
            shootablecomponents[i].GotShotLogic(projectileinfo);
        }
    }

    protected virtual void Hit(Vector3 origin, Vector3 direction, RaycastHit hitInfo)
    {

    }

    protected virtual ProjectileInfo CreateProjectileInfo(Vector3 direction, float damage)
    {
        ProjectileInfo projectile = new ProjectileInfo()
        {
            direction = direction,
            damage = damage
        };
        return projectile;
    }

}
//Cointains all projectile info for the bullets 
public struct ProjectileInfo
{
    public Vector3 direction;
    public float damage;
    
}

//Hit for enemies to recognize damage, Give it to all enemies and things that needs to be shot
public interface IShootAble 
{
    public void GotShotLogic(ProjectileInfo projectile);
}