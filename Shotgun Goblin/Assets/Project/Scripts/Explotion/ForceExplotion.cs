using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceExplotion : MonobehaviorScript_ToggleLog, IOnExplotionInRadius
{
    [SerializeField] float KNewtonPerSec = 1000;
    [SerializeField] float Radius = 100;
    public void OnExplode(Collider target, float effect)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();

        float force = KNewtonPerSec * 1000 * effect * Time.deltaTime;

        rb.AddExplosionForce(force, transform.position, Radius);

        DebugLog("exploded: " + target.name + " force: " + force);
    }
}
