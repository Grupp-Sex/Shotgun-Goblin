using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfWithinDistance : MonoBehaviour
{
    public GameObject enableTarget;
    [SerializeField] List<string> TargetTags = new List<string>() { "Player" };
    [SerializeField] bool enableWithin = true;
    [SerializeField] bool onEnter = true;
    [SerializeField] bool onExit = true;
    [SerializeField] bool isEnabled;

    void Awake()
    {
        if(enableTarget == null)
        {
            enableTarget = gameObject;
        }

        ToggleObject(!enableWithin);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTag(other))
        {
            if (onEnter)
            ToggleObject(enableWithin);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTag(other))
        {
            if (onExit)
                ToggleObject(!enableWithin);
        }
    }

    protected bool CheckTag(Collider other)
    {
        return TargetTags.Contains(other.tag);
    }

    protected void ToggleObject(bool enabled)
    {
        enableTarget.SetActive(enabled);
        isEnabled = enabled;
    }

    
}
