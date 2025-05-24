using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableLogKey : MonoBehaviour
{

    private void Start()
    {
        Debug.developerConsoleEnabled = true;
        Debug.developerConsoleVisible = true;
    }

    private void OnToggleConsoleKey()
    {
        Debug.Log("Console toggled");
        Debug.developerConsoleEnabled = !Debug.developerConsoleEnabled;
    }
}
