using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnStart : MonoBehaviour
{

    public ObjectDespawner Despawner;

    [SerializeField] float DespawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        if(Despawner == null)
        {
            Despawner = GetComponent<ObjectDespawner>();
        }

        Despawner.StartDespawnTimer(this, DespawnTimer);
    }

    
}
