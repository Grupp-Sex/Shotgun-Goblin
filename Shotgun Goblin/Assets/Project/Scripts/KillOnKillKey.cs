using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnKillKey : MonoBehaviour
{
    public HealthManager HealthManager;


    // Start is called before the first frame update
    void Awake()
    {
        if(HealthManager == null)
        {
            HealthManager = GetComponent<HealthManager>();
        }
    }

    private void OnKillPlayerKey()
    {
        HealthManager.imortal = false;
        HealthManager.invincible = false;
        HealthManager.Kill();
    }
    
}
