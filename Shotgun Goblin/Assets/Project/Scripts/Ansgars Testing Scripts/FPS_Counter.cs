using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    public float FPS;
    protected float UnRoundedFPS;

    public float True_FPS;

    [SerializeField] int decimals;
    [SerializeField] float lerpValue = 0.5f;
    

   
    void Update()
    {
        True_FPS = 1f / Time.deltaTime;

        UnRoundedFPS = UnRoundedFPS * (1-lerpValue) + True_FPS * lerpValue;

        float decimalPower = math.pow(10f, decimals);

        FPS = math.round(UnRoundedFPS * decimalPower) / decimalPower;
        
    }
}
