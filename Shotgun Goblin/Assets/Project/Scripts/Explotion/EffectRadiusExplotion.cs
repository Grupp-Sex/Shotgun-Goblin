using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRadiusExplotion : MonoBehaviour, IExplotion
{
    [SerializeField] float Duration;

    [SerializeField] float Radius;
    [SerializeField] AnimationCurve EffectOverDistance;

    protected bool isExploding;

    protected IOnExplotionInRadius[] onExplotionScripts;



    // Start is called before the first frame update
    void Start()
    {
        onExplotionScripts = GetComponents<IOnExplotionInRadius>();
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerStay(Collider other)
    {
        float effect = GetEffect(other.ClosestPoint(transform.position));

        NotifyExplotion(other, effect);
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

