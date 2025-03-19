using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolableObject : MonoBehaviour
{
    [HideInInspector] public PoolOfObjects parent;

    public virtual void OnDisable()
    {
        parent.AddObjectToPool(this);
    }
}
