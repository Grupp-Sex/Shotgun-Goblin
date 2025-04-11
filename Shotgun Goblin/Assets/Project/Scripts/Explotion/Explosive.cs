using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] bool DestroyOnExplotion;
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
        for(int i = 0; i < Explotions.Count; i++)
        {
            Explotion newExplotion = Instantiate(Explotions[i], transform.position, Quaternion.identity);
            newExplotion.gameObject.SetActive(true);
        }

        if (DestroyOnExplotion)
        {
            Destroy(gameObject);
        }

    }

    protected void RunIExplotions()
    {

    }

}


