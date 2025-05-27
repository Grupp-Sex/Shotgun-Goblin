using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player_Post_Processing : MonoBehaviour
{
    private HealthManager healthManager;
    [SerializeField] private Volume playerHealthVolume;

    private Vignette vignette;
    private ShadowsMidtonesHighlights shadows;

    [Header("LerpValues for Smoothness")]
    [SerializeField] private float minSmoothness = 0.2f;
    [SerializeField] private float maxSmoothness = 0.3f;
    [SerializeField] private float smoothSpeed = 0.5f;

    [Header("LerpValues for Shadows")]
    [SerializeField] private float minShadows = 0f;
    [SerializeField] private float maxShadows = 0.2f;
    [SerializeField] private float shadowSpeed = 0.5f;
   
    
    
    void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        playerHealthVolume.profile.TryGet(out vignette);
        playerHealthVolume.profile.TryGet(out shadows);
    }

    // Update is called once per frame
    void Update()
    {
        VignetteSmoothness();




        //if (healthManager.Health > 80f)
        //{
        //    vignette.intensity.value = 0f;
        //}
        //else if (healthManager.Health <= 80f)
        //{
        //    vignette
        //}
    }



    private void VignetteSmoothness()
    {
        // oscillating (swinging like a pendulum) between 0 and 1
        float t = Mathf.PingPong(Time.time * smoothSpeed, 1f);

        //Interpolating smoothness back and forth because of t
        float smoothness = Mathf.Lerp(minSmoothness, maxSmoothness, t);

        vignette.smoothness.value = smoothness;
    }

    private void ShadowsLuminance()
    {
        // oscillating (swinging like a pendulum) between 0 and 1
        float t = Mathf.PingPong(Time.time * shadowSpeed, 1f);

        //Interpolating smoothness back and forth because of t
        float intensity = Mathf.Lerp(minShadows, maxShadows, t);

        //shadows.shadows.value =
    }
}
