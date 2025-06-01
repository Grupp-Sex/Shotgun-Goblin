using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ansgar

public class EnableQueue 
{
    protected List<object> blockerQueue = new List<object>();

    public void AddBlocker(object key)
    {
        if (!blockerQueue.Contains(key))
        {
            blockerQueue.Add(key);
        }
    }

    public void RemoveBlocker(object key)
    {
        if (blockerQueue.Contains(key))
        {
            blockerQueue.Remove(key);
        }
    }

    public void ClearQueue()
    {
        blockerQueue.Clear();
    }

    public bool IsUnBlocked()
    {
        return blockerQueue.Count <= 0;
    }
}

public interface IBlockable
{
    public void AddBlocker(object key);

    public void RemoveBlocker(object key);

}
