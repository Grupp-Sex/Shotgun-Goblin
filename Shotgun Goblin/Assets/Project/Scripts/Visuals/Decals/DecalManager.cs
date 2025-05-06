using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms;

public class DecalManager : MonobehaviorScript_ToggleLog
{
    public List<DecalHolder> Decals;
    [SerializeField] bool GetDecalsFromChildren = true;

    private void Start()
    {
        if (GetDecalsFromChildren)
        {
            DecalProjector[] childProjectors = GetComponentsInChildren<DecalProjector>();

            for(int i = 0; i < childProjectors.Length; i++)
            {
                Decals.Add(DecalHolder.New(childProjectors[i]));
            }
        }
        DebugLog("Save values");
        SaveValues();
    }

    protected void SaveValues()
    {
        for (int i = 0; i < Decals.Count; i++)
        {
            Decals[i] = Save(Decals[i]);
            DebugLog(Decals[i].ToString());
        }
    }

    protected DecalHolder Save(DecalHolder decal)
    {
        
        if (decal.decal != null)
        {
            decal.startSize = decal.decal.size;
            decal.startOppacity = decal.decal.fadeFactor;
        }

        return decal;
        
    }

    public void RunActionOnAllDecals<T>(Action<DecalHolder, T> action, T input)
    {
        for(int i = 0;i < Decals.Count; i++)
        {
            action.Invoke(Decals[i], input);
        }
    }

}


[System.Serializable]
public struct DecalHolder
{
    public DecalProjector decal;

    public Vector3 startSize;
    public float startOppacity;

    public static DecalHolder New(DecalProjector decal)
    {
        return new DecalHolder() { decal = decal };
    }

    

    public override string ToString()
    {
        return "DecalHolder, Size: " + startSize + " oppacity: " + startOppacity;        
    }
}
