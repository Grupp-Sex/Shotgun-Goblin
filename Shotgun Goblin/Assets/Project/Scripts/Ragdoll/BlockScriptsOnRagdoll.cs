using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScriptsOnRagdoll : MonobehaviorScript_ToggleLog
{
    public List<GameObject> blockedObjects = new List<GameObject>();

    public RagdollManager RagdollManager;

    protected void Awake()
    {
        if(RagdollManager == null)
        {
            RagdollManager = GetComponent<RagdollManager>();
        }
    }

    private void OnEnable()
    {
        RagdollManager.Event_RagdollEnd.Subscribe(Event_RagdollStop);
        RagdollManager.Event_RagdollStart.Subscribe(Event_RagdollStart);
    }

    private void OnDisable()
    {
        RagdollManager.Event_RagdollEnd.UnSubscribe(Event_RagdollStop);
        RagdollManager.Event_RagdollStart.UnSubscribe(Event_RagdollStart);

        UnBlockObjects();
    }

    protected void Event_RagdollStart(object sender, object args)
    {
        BlockObjects();
    }

    protected void Event_RagdollStop(object sender, object args)
    {
        UnBlockObjects();
    }

    protected void BlockObjects()
    {
        for(int i = 0; i < blockedObjects.Count; i++)
        {
            InvokeIblockable(blockedObjects[i], BlockObject);
        }
    }

    protected void UnBlockObjects( )
    {
        for (int i = 0; i < blockedObjects.Count; i++)
        {
            InvokeIblockable(blockedObjects[i], UnblockObject);
        }
    }



    protected void InvokeIblockable(GameObject obj, Action<IBlockable> acc)
    {
        IBlockable[] blockableScripts = obj.GetComponents<IBlockable>();

        for (int i = 0; i < blockableScripts.Length; i++)
        {
            acc.Invoke(blockableScripts[i]);
        }
    }

    protected void BlockObject(IBlockable obj)
    {
        DebugLog("Object Blocked");
        obj.AddBlocker(this);
    }

    public void UnblockObject(IBlockable obj)
    {
        DebugLog("Object UnBlocked");
        obj.RemoveBlocker(this);
    }


}
