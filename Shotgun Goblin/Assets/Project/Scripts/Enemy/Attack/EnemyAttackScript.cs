using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackScript : MonobehaviorScript_ToggleLog, IBlockable
{
    protected EnableQueue BlockerQueue = new EnableQueue();

    public void AddBlocker(object key)
    {
        BlockerQueue.AddBlocker(key);
    }

    public void RemoveBlocker(object key)
    {
        BlockerQueue.RemoveBlocker(key);
    }
    
    public bool AttackIsUnblocked()
    {
        return BlockerQueue.IsUnBlocked();
    }
}
