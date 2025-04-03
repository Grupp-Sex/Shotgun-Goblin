using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnDeath : MonoBehaviour, IDeathActivated
{
    public void OnDeath(float remainingHealth)
    {
        gameObject.SetActive(false);
    }
}
