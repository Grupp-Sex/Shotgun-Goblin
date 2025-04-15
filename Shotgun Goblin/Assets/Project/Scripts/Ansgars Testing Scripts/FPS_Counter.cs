using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{

    [Header("FPS values")]

    public float FPS_Avrage;
    
    public float FPS;
    
    public float True_FPS;

    protected float UnRounded_FPS_Avrage;


    [Header("FPS_Avrage Variables")]
    [SerializeField] int FrameSampleInterval = 50;
    [SerializeField] int FrameCounter;
    protected float FPS_Sum;


    [Header("FPS Variables")]
    [SerializeField] int Decimals;
    [SerializeField] float Smootheness = 0.3f;

    protected float UnRounded_FPS;

    


    

    

    
    
    

   
    void Update()
    {
        float fps = SampleFPS();

        AvrageFPS(fps);

    }

    protected void AvrageFPS(float fps)
    {
        FPS_Sum += fps;

        FrameCounter = Time.frameCount % FrameSampleInterval;

        if(FrameCounter == 0)
        {
            UnRounded_FPS_Avrage = FPS_Sum / (float)FrameSampleInterval;
            FPS_Sum = 0;
        }

        FPS_Avrage = Round(UnRounded_FPS_Avrage, Decimals);

    }

    protected float SampleFPS()
    {
        True_FPS = 1f / Time.deltaTime;

        UnRounded_FPS = UnRounded_FPS * (1 - Smootheness) + True_FPS * Smootheness;

        FPS = Round(UnRounded_FPS, Decimals);

        return FPS;
    }

    protected float Round(float value, float decimals)
    {
        float decimalPower = math.pow(10f, decimals);

        return math.round(value * decimalPower) / decimalPower;

    }
}
