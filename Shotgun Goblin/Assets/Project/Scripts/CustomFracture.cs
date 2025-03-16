using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFracture : Fracture
{
    //any components in this object will be placed onto the fragments
    [SerializeField] GameObject FractureTemplate;

    protected override GameObject CreateFragmentTemplate()
    {


        GameObject fractureTemplate = Instantiate(FractureTemplate);

        ModifyFragmentTemplate(fractureTemplate);
        fractureTemplate.SetActive(isActiveAndEnabled);
        return fractureTemplate;
    }


}
