using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ParticlesOnDamage : MonoBehaviour, IDamageActivated
{
    [SerializeField] List<GameObject> InstanciatedObject;
    protected Collider mehscollider;

    void Start()
    {
        mehscollider = GetComponent<Collider>();
    }

    public void OnDamage(DamageInfo damageInfo)
    {
        
        for(int i = 0; i < InstanciatedObject.Count; i++)
        {
            InitializeObject(InstanciatedObject[i], damageInfo);

        }

    }



    protected void InitializeObject(GameObject template, DamageInfo damageInfo)
    {
        GameObject newObject = Instantiate(template, transform);

        newObject.SetActive(true);

        newObject.transform.position = damageInfo.position;

        if (damageInfo.hasDirection)
        {
            newObject.transform.LookAt(damageInfo.position + damageInfo.direction);
        }
        else if (mehscollider != null)
        {
            newObject.transform.LookAt(mehscollider.bounds.center);
        }
        else
        {
            newObject.transform.LookAt(transform.position);
        }

    }

    
}
