using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstasiateObjectOnShoot : MonoBehaviour, IShotActivated
{
    public Transform SpanwedObjectParrent;
    [SerializeField] List<GameObject> OnShotObject = new List<GameObject>();
    [SerializeField] List<GameObject> OnProjectileObject = new List<GameObject>();

    protected GameObject CreateObject(GameObject obj)
    {
        GameObject newObject = Instantiate(obj, SpanwedObjectParrent);

        return newObject;
        
    }

    protected GameObject CreateProjectileGameObject(GameObject obj, ProjectileInfo projectile)
    {
        GameObject newObject = CreateObject(obj);
        RotateObjectToProjectile(newObject, projectile);

        return newObject;
    }

    protected void RotateObjectToProjectile(GameObject obj, ProjectileInfo projectile)
    {
        obj.transform.rotation = Quaternion.LookRotation(projectile.direction);
    }

    public void RunShootLogic()
    {
        for(int i = 0; i < OnShotObject.Count; i++)
        {
            CreateObject(OnShotObject[i]);
        }

    }
    
    public void RunProjectileLogic(ProjectileInfo projectile)
    {
        for (int i = 0; i < OnProjectileObject.Count; i++)
        {
            CreateProjectileGameObject(OnProjectileObject[i], projectile);
        }
    }

    public void RunReloadLogic()
    {

    }
}
