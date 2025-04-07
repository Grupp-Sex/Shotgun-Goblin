using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//could be better. Possibly using interfaces and scripts in the "activatable" objects

public class HoldEffectsTelekenesis : BaseTelekenesisAbilaty
{
    [SerializeField] List<GameObject> ActivatedOnTelekenesis;

    public override void OnGrabTelekenesisObject(TelekenesisPhysicsObject grabedObjec)
    {
        base.OnGrabTelekenesisObject(grabedObjec);

        ShowObjects();
    }

    protected void ShowObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    protected void ShowObjects()
    {
        for (int i = 0; i < ActivatedOnTelekenesis.Count; i++)
        {
            ShowObject(ActivatedOnTelekenesis[i]);
        }
    }

    protected void HideObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    protected void HideObjects()
    {
        for (int i = 0; i < ActivatedOnTelekenesis.Count; i++)
        {
            HideObject(ActivatedOnTelekenesis[i]);
        }
    }

    protected override void OnUpdate()
    {
        if(heldObjects.Count <= 0)
        {
            HideObjects();
        }
    }
}
