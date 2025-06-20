/*
 * Player_Post_Processing.cs
 * 
 * Modifies post-processing effects based on the player's current health.
 * Integrates with Unity's Universal Render Pipeline (URP) using Volume components and overrides
 * for effects like Vignette and Shadows/Midtones/Highlights.
 * 
 * Effects Implemented:
 * - Vignette: Intensity and smoothness increase as health drops below defined thresholds.
 * - Shadows: Enabled and modulated when health is below a set danger threshold.
 * - Midtones: A secondary Volume handles midtone-only changes (due to URP limitations with disabling individual overrides).
 * 
 * Core Features:
 * - Smooth oscillation (using Mathf.PingPong) for visual pulsing effects like vignette smoothness and shadow luminance.
 * - Health-based thresholds define when each effect activates or deactivates.
 * - Uses linear interpolation (Lerp and InverseLerp) to scale visual intensity dynamically.
 * - Works with two separate URP Volumes to independently control visual override effects.
 * 
 * Serialized Settings:
 * - Sensitivity thresholds and interpolation speeds for all effects
 * - Luminance ranges for shadows and midtones
 * 
 * Dependencies:
 * - Requires `HealthManager` component on the same GameObject to track player health
 * - Requires two active URP Volume objects in the scene:
 *      - One for general health effects (Vignette + Shadows)
 *      - One for midtones-only adjustments (custom solution to URP's override behavior)
 * 
 * Setup:
 * - Attach to the player GameObject
 * - Assign both Volume references in the Inspector or ensure they are named properly in the scene:
 *      - "Player Health Volume"
 *      - "Player Midtones Volume"
 * - Ensure Vignette and ShadowsMidtonesHighlights overrides are added to the Volume profiles
 * 
 * Author:
 * - Written by Mikael
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Player_Post_Processing : MonoBehaviour
{
    private HealthManager healthManager;

    [SerializeField] private Volume playerHealthVolume;
    //I have to create a second volume for midtones since i can't disable only midtones or shadows, i have to disable all effects so i made another volume for midtones only
    [SerializeField] private Volume midtonesVolume;

    private Vignette vignette;
    private ShadowsMidtonesHighlights shadows;
    private ShadowsMidtonesHighlights midtonesOnly;


    [SerializeField] private float volumeWeight1 = 0f;
    [SerializeField] private float volumeweight2 = 0f;

    [Header("LerpValues for Smoothness")]
    [SerializeField] private float minSmoothness = 0.2f;
    [SerializeField] private float maxSmoothness = 0.3f;
    [SerializeField] private float smoothSpeed = 0.5f;

    [Header("LerpValues for Shadows")]
    [SerializeField] private float minShadows = 0f;
    [SerializeField] private float maxShadows = 0.2f;
    [SerializeField] private float shadowSpeed = 0.5f;

    [Header("Health Thresholds")]
    [SerializeField] private float vignetteStartThreshold = 80f;
    [SerializeField] private float vignetteEndThreshold = 20f;
    [SerializeField] private float minVignetteIntensity = 0.4f;
    [SerializeField] private float maxVignetteIntensity = 0.7f;

    [SerializeField] private float shadowsToggleThreshold = 40f;

    [SerializeField] private float midtonesStartThreshold = 30f;
    [SerializeField] private float midtonesEndThreshold = 15f;
    [SerializeField] private float minMidtonesValue = -0.5f;
    [SerializeField] private float maxMidtonesValue = 0f;
    [SerializeField] private float midtonesSpeed = 0.5f;

   
    
    
    void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        playerHealthVolume = GameObject.Find("Player Health Volume").GetComponent<Volume>();
        midtonesVolume = GameObject.Find("Player Midtones Volume").GetComponent <Volume>();
        playerHealthVolume.profile.TryGet(out vignette);
        playerHealthVolume.profile.TryGet(out shadows);
        midtonesVolume.profile.TryGet(out midtonesOnly);

        playerHealthVolume.weight = 1.0f;
        midtonesVolume.weight = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        volumeWeight1 = playerHealthVolume.weight;
        volumeweight2 = midtonesVolume.weight;
        VignetteSmoothness();
        UpdateHealthEffects();
       

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
        
        float t = Mathf.PingPong(Time.time * shadowSpeed, 1f);

        
        float luminance = Mathf.Lerp(minShadows, maxShadows, t);

        Vector4 color = shadows.shadows.value;

        shadows.shadows.value = new Vector4(color.x, color.y, color.z, luminance);

        
    }

    private void UpdateHealthEffects()
    {
        float health = healthManager.Health;
        float maxHealth = healthManager.MaxHealth;

        //Gives whole number to the percent instead of decimal
        float healthPercent = (health / maxHealth) * 100f;

        //Vignette intensity increases as health drops below the threshold
        if (healthPercent >= vignetteStartThreshold)
        {
            vignette.intensity.value = 0f;
        }
        else
        {
            //Gets how far the players health has fallen from starting point to end point as a value between 0 and 1 and then intensify the vignette based on the value between 2 points (min and max)
            float t = Mathf.InverseLerp(vignetteStartThreshold, vignetteEndThreshold, Mathf.Clamp(healthPercent, vignetteEndThreshold, vignetteStartThreshold));
            vignette.intensity.value = Mathf.Lerp(minVignetteIntensity, maxVignetteIntensity, t);

        }

        if (healthPercent <= shadowsToggleThreshold)
        {
            shadows.active = true;

            ShadowsLuminance();
        }
        else
        {
            //Disable shadows if not at the threshold
            shadows.active = false;
        }


        //enable midtones and interpolate them between 2 values
        if (healthPercent <= midtonesStartThreshold)
        {
            // Enable override
            midtonesOnly.midtones.overrideState = true;

            float t = Mathf.InverseLerp(midtonesStartThreshold, midtonesEndThreshold, Mathf.Clamp(healthPercent, midtonesEndThreshold, midtonesStartThreshold));
            float pulse = Mathf.PingPong(Time.time * midtonesSpeed, 1f);
            float luminance = Mathf.Lerp(minMidtonesValue, maxMidtonesValue, pulse);
            float finalLuminance = Mathf.Lerp(maxMidtonesValue, luminance, t);

            Vector4 color = midtonesOnly.midtones.value;
            midtonesOnly.midtones.value = new Vector4(color.x, color.y, color.z, finalLuminance);
        }
        else
        {
            // Disable override so it doesn't apply midtones
            midtonesOnly.midtones.overrideState = false;
        }
    }
}
