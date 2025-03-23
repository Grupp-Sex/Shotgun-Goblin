using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitManage : MonoBehaviour,IHitLogic
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunHitLogic(RaycastHit hit, ProjectileInfo projectile)
    {
        HitLogic(hit,projectile);
    }
    protected virtual void HitLogic(RaycastHit hit, ProjectileInfo projectile)
    {

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
}

//Hit for enemies to recognize damage, Give it to all enemies and things that needs to be shot
public interface IShootAble
{
    public void GotShotLogic(ProjectileInfo projectile);
}