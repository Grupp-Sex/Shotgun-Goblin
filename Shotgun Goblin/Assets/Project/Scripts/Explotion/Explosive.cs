using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] bool DestroyOnExplotion;

    [SerializeField] float ExplotionTimerDuration;
    protected bool isExploding;
    

    [SerializeField] List<Explotion> Explotions;

    [SerializeField] bool ExplodeButton;

    protected void OnValidate()
    {
        if (isActiveAndEnabled)
        {
            if (ExplodeButton)
            {
                Explode();

                ExplodeButton = false;
            }
        }
    }

    public void Explode()
    {
        StartExplotionTimer();

    }

    protected void StartExplotionTimer()
    {
        if (isActiveAndEnabled && !isExploding)
        {
            isExploding = true;
            StartCoroutine(ExplotionTimer(ExplotionTimerDuration));
        }
    }

    protected IEnumerator ExplotionTimer(float time)
    {
        yield return new WaitForSeconds(time);

        DoExplotion();
    }

    protected void DoExplotion()
    {
        for (int i = 0; i < Explotions.Count; i++)
        {
            Explotion newExplotion = Instantiate(Explotions[i], transform.position, Quaternion.identity);
            newExplotion.gameObject.SetActive(true);
        }

        if (DestroyOnExplotion)
        {
            Destroy(gameObject);
        }
    }

    


}


