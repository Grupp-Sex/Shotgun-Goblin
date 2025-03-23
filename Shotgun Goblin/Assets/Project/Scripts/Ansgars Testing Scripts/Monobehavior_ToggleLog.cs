using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonobehaviorScript_ToggleLog : MonoBehaviour
{
    [SerializeField] protected bool LogInDebugConsole;



    protected virtual void DebugLog(string log)
    {
        if (LogInDebugConsole)
        {
            Debug.Log(name + ":" + '\n' + log + '\n');
        }

    }
}
