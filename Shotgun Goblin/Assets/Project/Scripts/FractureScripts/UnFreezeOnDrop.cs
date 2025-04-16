using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnFreezeOnDrop : MonoBehaviour, IOnDropped
{
    protected FragmentFreeze freezeScript;
    void Start()
    {
        freezeScript = GetComponent<FragmentFreeze>();
    }

    public void OnDropped()
    {
        if(freezeScript != null)
        {
            freezeScript.Activate();
        }
    }
}
