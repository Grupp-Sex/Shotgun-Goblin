using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    [SerializeField] bool AimButton;
    [SerializeField] bool ShootButton;

    [SerializeField] Rigidbody ThrownObjectTemplate;

    public ThrowAimer Aimer;
    public Transform GetAimerPos => Aimer.transform;
    [SerializeField] float VelocityMult = 1;
    [SerializeField] float MaxVelocity = 15;
    [SerializeField] float MaxDistance = 5;

    [SerializeField] float MinVelocity = 10;
    [SerializeField] float MinDistance = 0;

    [SerializeField] bool UpdateOnUpdate = true;

    public Transform Target;


    private void OnValidate()
    {
        if (isActiveAndEnabled)
        {
            if (AimButton)
            {
                Aim();
                AimButton = false;
            }
            
        }
    }

    protected void CheckButtons()
    {
        if (isActiveAndEnabled)
        {
            if (AimButton)
            {
                Aim();
                AimButton = false;
            }
            if (ShootButton)
            {
                Throw();
                ShootButton = false;
            }
        }
    }

    protected void Fire()
    {
        Aim();
        Throw();
    }

    protected float CalculateVelocity()
    {
        float distance = Vector3.Distance(Target.position, Aimer.transform.position);

        distance = Mathf.Clamp(distance, MinDistance, MaxDistance);

        float lerpValue = (distance - MinDistance) / (MaxDistance - MinDistance);

        lerpValue = 1 - lerpValue;

        lerpValue *= lerpValue;

        lerpValue = 1 - lerpValue;

        return (MaxVelocity * lerpValue + MinVelocity * (1 - lerpValue)) * VelocityMult;
    }

    public void Aim()
    {
        Aimer?.Aim(Target.position, CalculateVelocity());
    }

    public void Throw()
    {
        Rigidbody projectile = Instantiate(ThrownObjectTemplate, Aimer.OriginObject);
        projectile.gameObject.SetActive(true);
        projectile.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);

        projectile.transform.parent = null;

        projectile.AddRelativeForce(new Vector3(0,0, CalculateVelocity()), ForceMode.VelocityChange);
        
    }

    private void Update()
    {
        CheckButtons();

        if (UpdateOnUpdate)
        {
            Aim();
        }   
    }

}
