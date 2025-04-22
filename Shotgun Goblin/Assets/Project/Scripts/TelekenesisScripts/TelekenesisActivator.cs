using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekenesisActivator : MonobehaviorScript_ToggleLog
{
    private TelekenesisManager telekenesisManager;

    private void Start()
    {
        telekenesisManager = GetComponent<TelekenesisManager>();

    }

    protected void OnSecondaryShoot()
    {
        DebugLog("SecondaryShoot start");

        Grab();
    }

    protected void OnSecondaryShootStop()
    {
        DebugLog("SecondaryShoot end");

        Throw();
    }


    protected virtual void Grab()
    {
        telekenesisManager.ActivateGrab();
    }

    protected virtual void Throw()
    {
        telekenesisManager.ActivateThrow();
    }


}
