using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ansgar
public class PlayerGunActivator : MonoBehaviour
{
    protected PlayerHeldItemInteractor heldItem;
    
    // Start is called before the first frame update
    void Start()
    {
        heldItem = GetComponent<PlayerHeldItemInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPrimaryShoot()
    {
        heldItem.HeldItemDoInteraction();
    }
}
