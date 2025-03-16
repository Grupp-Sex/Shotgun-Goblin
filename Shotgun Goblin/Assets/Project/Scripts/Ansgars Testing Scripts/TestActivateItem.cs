using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestActivateItem : MonoBehaviour
{
    // to help debuging and development. 
    // lets you activate any script without needing lots of boilerplate code.

    [SerializeField] GameObject DebugTarget;

    public void OnDebug()
    {
        DebugActivate();
    }

    private void DebugActivate()
    {
        Debug.Log("Testing: Script Activated");
        DebugTarget.GetComponent<IDebugActivatableVoid>().VoidDebugActivate();
    }


}


// add this interface to any script to make it activatable by this script
public interface IDebugActivatableVoid
{
    public void VoidDebugActivate();
}
