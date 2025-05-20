using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnDisable: IDisposable
{
    protected Action onDisable;

    public ActionOnDisable(Action onDisable)
    {
        this.onDisable = onDisable;
    }


    public void Dispose()
    {
        onDisable.Invoke();
    }

}
