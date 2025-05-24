using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageHudOverlay : MonoBehaviour, IDamageActivated, IDeathActivated
{
    protected HealthManager healthManager;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float lerpValue;
    public Volume urpVolume;

    protected float targetValue;
    protected float currentValue;

    // Start is called before the first frame update
    void Awake()
    {
        healthManager = GetComponent<HealthManager>();
    }

    public void OnDamage(DamageInfo damageInfo)
    {
        CalculateHealthLerp();
    }

    public void OnDeath(DamageInfo damage)
    {
        targetValue = 1;
        currentValue = 1;
        if (urpVolume != null)
        {
            urpVolume.weight = 1;
        }
    }

    protected void CalculateHealthLerp()
    {
        float lerpValue = healthManager.Health / healthManager.MaxHealth;

        lerpValue = Mathf.Clamp01(1 - lerpValue);

        float curveValue = curve.Evaluate(lerpValue);

        targetValue = Mathf.Clamp01(curveValue);
    }

    protected void Interpolate()
    {
        currentValue = currentValue * lerpValue + targetValue * (1-lerpValue);
    }

    protected void UpdateVolume()
    {
        if(urpVolume != null)
        {
            urpVolume.weight = currentValue;
        }
    }

    private void Update()
    {
        Interpolate();
        UpdateVolume();
    }


}


