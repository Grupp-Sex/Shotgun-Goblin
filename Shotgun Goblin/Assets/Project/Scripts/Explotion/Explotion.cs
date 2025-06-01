using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    [SerializeField] float Duration;

    protected IExplotion[] explotionScripts;

    protected bool isExploding;

    void Start()
    {
        explotionScripts = GetComponentsInChildren<IExplotion>();
        Explode();
    }

    public void Explode()
    {
        gameObject.SetActive(true);
        DestroyIn(Duration);
        isExploding = true;
    }

    protected void ResetExplotion()
    {
        isExploding = false;
        Destroy(gameObject);
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

    protected void NotifyExplotions()
    {
        for (int i = 0; i < explotionScripts.Length; i++)
        {
            explotionScripts[i].Explode();
        }
    }


}

public interface IExplotion
{
    public void Explode();
}
