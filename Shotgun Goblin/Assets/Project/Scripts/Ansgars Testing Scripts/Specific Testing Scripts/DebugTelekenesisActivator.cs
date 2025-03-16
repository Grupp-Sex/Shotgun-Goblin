using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTelekenesisActivator : MonoBehaviour
{
    private TelekenesisManager telekenesisManager;
    // Start is called before the first frame update
    void Start()
    {
        telekenesisManager = GetComponent<TelekenesisManager>();
    }

    protected void OnDebug()
    {
        telekenesisManager.GrabOjcects();
    }

    protected void OnDebugRelease()
    {
        telekenesisManager.ThrowObjects();
    }

}
