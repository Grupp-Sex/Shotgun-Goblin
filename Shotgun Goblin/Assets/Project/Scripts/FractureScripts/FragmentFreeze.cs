using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;

public class FragmentFreeze : MonobehaviorScript_ToggleLog
{
    [SerializeField] float ThawNeighboursRadius = 0.3f;
    [SerializeField] bool RecallOnStart = true;
    [SerializeField] bool StartFrozen;
    [SerializeField] bool DoActivate;
    [SerializeField] public bool IsFrozen;
    
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

    public void SoftImpact(CollisionData collision)
    {
        if (IsFrozen)
        {
            ActivateNeighbours(collision.position, 0.5f);
        }
    }

    public void HardImpact(CollisionData collision)
    {
        if (!IsFrozen)
        {
            gameObject.SetActive(false);
        }
        //Destroy(gameObject);
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
        IsFrozen = true;
        rb.isKinematic = true;
        rb.Sleep();

        if (RecallOnStart)
        {
            StartCoroutine(StartupTimer(0.001f));
        }

        ActivateFreezeComponents(true);
        DebugLog("Object Frozen: " + name);
    }

    protected IEnumerator StartupTimer(float time)
    {
        yield return new WaitForSeconds(time);

        yield return new WaitUntil(() => rb != null && transform.parent != null);

        rb.MoveRotation(transform.parent.rotation);
        rb.MovePosition(transform.parent.position);
    }

    protected virtual void Thaw()
    {
        IsFrozen = false;
        rb.isKinematic = false;
        ActivateFreezeComponents(false);
        DebugLog("Object Thawed: " + name);

    }

    public virtual void Activate()
    {
        Thaw();
    }

    public virtual void ActivateNeighbours(Vector3 pos)
    {
        ActivateNeighbours(pos, ThawNeighboursRadius);
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
