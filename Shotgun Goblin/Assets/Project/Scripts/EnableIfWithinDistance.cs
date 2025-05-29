using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfWithinDistance : MonoBehaviour
{
    public GameObject enableTarget;
    [SerializeField] bool enableWithin = true;
    [SerializeField] bool onEnter = true;
    [SerializeField] bool onExit = true;

    void Awake()
    {
        if(enableTarget == null)
        {
            enableTarget = gameObject;
        }

        enableTarget.SetActive(!enableWithin);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onEnter) 
            enableTarget.SetActive(enableWithin);
    }

    private void OnTriggerExit(Collider other)
    {
        if (onExit)
            enableTarget.SetActive(!enableWithin);
    }
}
