using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Toolbars;
using UnityEngine;

public class FractureCustom : Fracture, IDebugActivatableVoid
{
    [SerializeField] public GameObject FractureTemplate;

    [SerializeField] public bool FractureOnStartup;

    [SerializeField] public bool ActivateFractureButton;
    private void ActivateFractureButtonScript()
    {
        if (ActivateFractureButton)
        {
            FractureObject();

            ActivateFractureButton = false;
        }

    }

    public void Start()
    {
        if (FractureOnStartup)
        {
            FractureObject();
        }
    }


    void Update()
    {
        ActivateFractureButtonScript();
    }

    public void VoidDebugActivate()
    {

        FractureObject();
    }


    public virtual void FractureObject()
    {
        CauseFracture();
    }

    protected override GameObject CreateFragmentTemplate()
    {
        GameObject obj = Instantiate(FractureTemplate);

        obj.SetActive(true);

        ModifyTemplateObject(obj);

        return obj;
    }

}
