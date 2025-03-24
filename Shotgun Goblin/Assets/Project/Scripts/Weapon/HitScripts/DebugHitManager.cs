using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DebugHitManager : BaseHitManage, IHitLogic
{
    [SerializeField] GameObject hitObjectTemplate;
    [SerializeField] float ForceMagnitude;

    [SerializeField] List<GameObject> hitObjects;
    [SerializeField] GameObject projectileHolder;

    protected override void HitLogic(RaycastHit hit, ProjectileInfo projectile)
    {
        base.HitLogic(hit, projectile);

        PlaceHitObject(hit);
        ActivateGotShotLogic(hit.collider.gameObject, projectile);

        PushObject(hit, projectile.direction, ForceMagnitude);

    }

    protected void PushObject(RaycastHit hit, Vector3 direction, float forceMagnitude)
    {
        Rigidbody rb = hit.rigidbody;

        if(rb != null)
        {
            Vector3 forceVector = direction.normalized * forceMagnitude;

            rb.AddForce(forceVector);
        }

    }

    protected void PlaceHitObject(RaycastHit hit)
    {
        if(projectileHolder == null)
        {
            projectileHolder = new GameObject();
            projectileHolder.name = "projectileHolder";
        }

        GameObject newHitObject = Instantiate(hitObjectTemplate, hit.point, transform.rotation, projectileHolder.transform);

        newHitObject.AddComponent<ParentConstraint>();

        ParentConstraint constraint = newHitObject.GetComponent<ParentConstraint>();
        
        constraint.AddSource(new ConstraintSource { sourceTransform = hit.transform, weight = 1 });
        constraint.SetTranslationOffset(0, hit.point - hit.transform.position );
        constraint.SetRotationOffset(0, transform.rotation.eulerAngles);
        constraint.constraintActive = true;
        

        newHitObject.SetActive(true);

        hitObjects.Add(newHitObject);
    }

    


}
