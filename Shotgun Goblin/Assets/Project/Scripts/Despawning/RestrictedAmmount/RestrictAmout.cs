using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestrictAmout : MonobehaviorScript_ToggleLog
{
    public static Dictionary<string, int> idCounters = new Dictionary<string, int>();

    public List<AmountTag> tags;
    

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            AddObject();
        }
    }

    private void OnDisable()
    {
        if (Application.isPlaying)
        {
            RemoveObject();
        }
    }

    protected void CountID(string id, int ammount)
    {
        if (!idCounters.ContainsKey(id))
        {
            idCounters.Add(id, 0);
        }
        
        idCounters[id]+= ammount;

        DebugLog("Count ID, tag = " + id + ", count = " + idCounters[id] + ", added ammount: " + ammount);
    }

    

    protected static int GetNrOfTag(string tag)
    {
        if (idCounters.ContainsKey(tag))
        {
            return idCounters[tag];
        }
        else
        {
            return 0;
        }
    }
    
    protected void CountAllIDs(int mult)
    {
        for(int i = 0; i < tags.Count; i++)
        {
            CountID(tags[i].tag, tags[i].objectValue * mult);
        }
    }

    protected void AddObject()
    {
        CountAllIDs(1);

        CheckAmount();
    }

    protected void RemoveObject()
    {
        CountAllIDs(-1);
    }

    protected void CheckAmount()
    {
        bool toBeDestroyed = false;


        for (int i = 0; i < tags.Count; i++) 
        {
            if (CheckAmountTag(tags[i]))
            {
                
                toBeDestroyed = true; break;
            }
        }

        if (toBeDestroyed)
        {
            

            Destroy(gameObject);
        }
    }

    protected bool CheckAmountTag(AmountTag tag)
    {
        int tagNr = GetNrOfTag(tag.tag);

        bool roofReched = tagNr > tag.maxAmount;

        if (roofReched)
        {
            DebugLog("id roof reached, ammount: " + tagNr + ", max amount: " + tag.maxAmount);
        }

        return roofReched;
    }

}

[System.Serializable]
public struct AmountTag
{
    public string tag;
    public int maxAmount; 
    public int objectValue;
}
