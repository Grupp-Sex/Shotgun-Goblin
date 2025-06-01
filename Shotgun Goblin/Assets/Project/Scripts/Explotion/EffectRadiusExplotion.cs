using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EffectRadiusExplotion : MonoBehaviour, IExplotion
{
    [SerializeField] float Duration;

    [SerializeField] float Radius;
    [SerializeField] AnimationCurve EffectOverDistance;

    protected bool isExploding;

    protected IOnExplotionInRadius[] onExplotionScripts;

    protected List<Collider> collidersInExplotion = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        onExplotionScripts = GetComponents<IOnExplotionInRadius>();
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        AffectObjectsInExplotion();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnterExplotion(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ExitExplotion(other);
    }

    protected void AffectObjectsInExplotion()
    {
        for(int i = 0; i < collidersInExplotion.Count; i++)
        {
              InExplotion(collidersInExplotion[i]);
        }

        
    }

    
    

    protected void InExplotion(Collider other)
    {
        if (other != null && other == isActiveAndEnabled)
        {
            float effect = GetEffect(other.ClosestPoint(transform.position));
            NotifyExplotion(other, effect);
        }
        else
        {
            ExitExplotion(other);
        }

    }

    protected void EnterExplotion(Collider collider)
    {
        if (collider != null && collider == isActiveAndEnabled)
        {
            if (!collidersInExplotion.Contains(collider))
            {
                collidersInExplotion.Add(collider);
            }
        }
    }

    protected void ExitExplotion(Collider collider)
    {
        if (collidersInExplotion.Contains(collider))
        {
            collidersInExplotion.Remove(collider);
        }
    }

    protected float GetEffect(Vector3 impactPoint)
    {
        float distance = Vector3.Distance(transform.position, impactPoint) / Radius;

        return EffectOverDistance.Evaluate(distance);
    }

    protected void NotifyExplotion(Collider collider, float effect)
    {
        for (int i = 0; i < onExplotionScripts.Length; i++)
        {
            onExplotionScripts[i].OnExplode(collider, effect);
        }

    }

    public void Explode()
    {
        gameObject.SetActive(true);
        DestroyIn(Duration);
        isExploding = true;
    }

    protected void ResetExplotion()
    {
        gameObject.SetActive(false);
        isExploding = false;
    }

    protected void DestroyIn(float duration)
    {
        if (!isExploding)
        {
            StartCoroutine(DestroyTimer(duration));
        }

    }

    protected IEnumerator DestroyTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        ResetExplotion();
    }

}

public interface IOnExplotionInRadius
{
    public void OnExplode(Collider collider, float effect);
}

