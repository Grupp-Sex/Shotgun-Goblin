using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalManager : MonoBehaviour
{
    public List<DecalHolder> Decals;

    private void Start()
    {
        SaveValues();
    }

    protected void SaveValues()
    {
        for (int i = 0; i < Decals.Count; i++)
        {
            Decals[i].Save();
        }
    }

    public void RunActionOnAllDecals<T>(Action<DecalHolder, T> action, T input)
    {
        for(int i = 0;i < Decals.Count; i++)
        {
            action.Invoke(Decals[i], input);
        }
    }

}



public struct DecalHolder
{
    public DecalProjector decal;

    public Vector3 startSize { get ; private set; }
    public float startOppacity { get; private set; }

    public void Save()
    {
        if (decal != null)
        {
            startSize = decal.size;
            startOppacity = decal.fadeFactor;
        }
    }
}
