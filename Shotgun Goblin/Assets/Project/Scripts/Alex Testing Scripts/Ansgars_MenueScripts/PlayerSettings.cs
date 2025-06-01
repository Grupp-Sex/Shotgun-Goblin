using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerSettings: MonoBehaviour
{
    public static bool dash_dubbleTap = true;
    public void ToggleDubbleTapDash(bool value)
    {
        dash_dubbleTap = value;
        Debug.Log(value);
    }

    public static bool dash_shift = true;
    public void ToggleShiftDash(bool value)
    {
        dash_shift = value;
        Debug.Log(value);
    }

    public static float fov = 60f;

}
