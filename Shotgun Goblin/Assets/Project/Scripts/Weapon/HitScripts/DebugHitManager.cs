using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHitManager : BaseHitManage
{
    [SerializeField] GameObject hitObjectTemplate;

    [SerializeField] List<GameObject> hitObjects;
    protected override void HitLogic(RaycastHit hit, ProjectileInfo projectile)
    {
        base.HitLogic(hit, projectile);

        PlaceHitObject(hit.point);
    }

    protected void PlaceHitObject(Vector3 point)
    {
        GameObject newHitObject = Instantiate(hitObjectTemplate, point, transform.rotation);
        newHitObject.SetActive(true);

        hitObjects.Add(newHitObject);
    }


}
