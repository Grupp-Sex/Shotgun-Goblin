using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureBase : Fracture
{
    [SerializeField] public GameObject FractureTemplate;


    protected override GameObject CreateFragmentTemplate()
    {

        GameObject obj = Instantiate(FractureTemplate);

        obj.SetActive(true);

        ModifyTemplateObject(obj);

        return obj;
    }
}
