using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstansiateObjectOnDeath : MonoBehaviour, IDeathActivated
{
    public List<GameObject> templates;

    public void OnDeath(DamageInfo damage)
    {
        for(int i = 0; i < templates.Count; i++)
        {
            GameObject newObject = Instantiate(templates[i]);
            newObject.transform.position = transform.position;
            newObject.transform.rotation = transform.rotation;
        }
    }

    
}
