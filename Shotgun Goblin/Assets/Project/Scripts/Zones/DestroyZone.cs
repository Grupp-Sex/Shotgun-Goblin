using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    [SerializeField] List<string> IgnoreTags;
    [SerializeField] bool OnlyRigidbody = true;


    private void OnTriggerEnter(Collider other)
    {
        if (!IgnoreTags.Contains(other.tag))
        {
            bool hasRigidbody = other.attachedRigidbody != null;

            if (!OnlyRigidbody || hasRigidbody)
            {
                
                if(hasRigidbody)
                    other.attachedRigidbody.gameObject.SetActive(false);
                
                other.gameObject.SetActive(false);
            }
        }
    }


}
