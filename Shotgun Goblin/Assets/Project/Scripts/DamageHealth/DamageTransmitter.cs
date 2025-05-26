using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTransmitter : MonoBehaviour
{
    public HealthManager targetHealthManager;

    protected HealthManager objectHealthManager;

    // Start is called before the first frame update
    void Awake()
    {
        objectHealthManager = GetComponent<HealthManager>();
        objectHealthManager.Event_Damage.Subscribe(Event_Damage);
    }

    protected void Event_Damage(object sender, DamageInfo damage)
    {
        if (targetHealthManager != null)
        {
            targetHealthManager.Damage(damage);
        }
    }
}
