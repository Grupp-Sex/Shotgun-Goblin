using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

public class ToggleShader : MonobehaviorScript_ToggleLog
{
    [SerializeField] UniversalRendererData RendererData;

    [SerializeField] List<ShaderToggleOrNot> ToggleShaders;


    private void OnValidate()
    {
        ToggleAll();
    }

    private void Start()
    {
        ToggleAll();
    }


    protected void ToggleAll() 
    {
        if (isActiveAndEnabled)
        {
            DebugLog("Toggle Shaders");
            for (int i = 0; i < ToggleShaders.Count; i++)
            {
                Toggle(ToggleShaders[i]);
            }
        }
    }

    protected void Toggle(ShaderToggleOrNot shaderToggle)
    {
        if (RendererData != null)
        {
            List<ScriptableRendererFeature> shaders = RendererData.rendererFeatures;


            for (int i = 0; i < shaders.Count; i++)
            {
                if (shaders[i].name == shaderToggle.ShaderName)
                {
                    shaders[i].SetActive(shaderToggle.Toggle);
                    
                    if(shaderToggle.Toggle)
                    {
                        DebugLog(shaderToggle.ShaderName + " on");
                    }
                    else
                    {
                        DebugLog(shaderToggle.ShaderName + " off");
                    }

                    
                }
            }
        }

    }

   

}

[System.Serializable]
public struct ShaderToggleOrNot
{
    [SerializeField] public string ShaderName;
    [SerializeField] public bool Toggle;
}
