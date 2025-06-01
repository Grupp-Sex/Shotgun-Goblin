using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] List<string> IgnoreTags;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IgnoreTags.Contains(other.tag))
        {
            KillObject(other.GetComponent<HealthManager>());
        }
    }

    protected void KillObject(HealthManager helth)
    {
        if (helth != null)
        {
            helth.Damage(999999999f, helth.transform.position);
            helth.Damage(helth.MaxHealth, helth.transform.position);
            helth.Kill();
        }
    }
}
