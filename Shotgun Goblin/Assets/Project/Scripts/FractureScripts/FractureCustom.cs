using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Toolbars;
using UnityEngine;

public class FractureCustom : FractureBase, IDebugActivatableVoid
{

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

    

}
