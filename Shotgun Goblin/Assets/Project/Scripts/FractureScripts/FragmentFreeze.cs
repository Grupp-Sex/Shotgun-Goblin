using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class FragmentFreeze : MonobehaviorScript_ToggleLog, IShootAble
{
    [SerializeField] bool StartFrozen;
    [SerializeField] bool DoActivate;

    protected Rigidbody rb;
    

    protected IFrozenOnFractionFreeze[] componentsToBeThawed; 


    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        componentsToBeThawed = GetComponents<IFrozenOnFractionFreeze>();
        if (StartFrozen)
        {
            Freeze();
        }
    }

    private void OnValidate()
    {
        if (DoActivate)
        {
            DoActivate = false;
            Activate();
        }
    }

    protected void ActivateFreezeComponents(bool frezeOrThaw)
    {
        foreach(var component in componentsToBeThawed)
        {
            if (frezeOrThaw)
            {
                component.Freze();
                DebugLog("Component frozen");

            }
            else
            {
                component.Thaw();
                DebugLog("Component thawed");
            }
            

        }
    }

    protected virtual void Freeze()
    {
        rb.isKinematic = true;
        rb.Sleep();
        ActivateFreezeComponents(true);
        DebugLog("Object Frozen: " + name);
    }

    protected virtual void Thaw()
    {
        rb.isKinematic = false;
        ActivateFreezeComponents(false);
        DebugLog("Object Thawed: " + name);
    }

    public virtual void Activate()
    {
        Thaw();
    }
    
    public virtual void ActivateNeighbours(Vector3 pos, float radius)
    {
        Collider[] colidersInside = Physics.OverlapSphere(pos, radius);
        int counter = 0;
        for (int i = 0; i < colidersInside.Length; i++)
        {

            DebugLog(colidersInside[i].gameObject.name);
            FragmentFreeze fragment = colidersInside[i].gameObject.GetComponent<FragmentFreeze>();
            
            if (fragment != null)
            {
                counter++;
                DebugLog("neighbour found");
                fragment.Thaw();
            }
        }

        DebugLog("neighbours found: " + counter + " of " + colidersInside.Length + " total objects");



    }

    public void GotShotLogic(ProjectileInfo projectile)
    {
        ActivateNeighbours(projectile.hitPos, 0.3f);
    }

}

public interface IFrozenOnFractionFreeze
{
    public bool IsFrozen { get; set; }

    public void Freze();

    public void Thaw();

}
