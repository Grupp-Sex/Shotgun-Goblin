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

    [SerializeField] ThrowAimer Aimer;


    [SerializeField] float Velocity = 5;

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

    protected void Aim()
    {
        Aimer?.Aim(Target.position, Velocity);
    }

    protected void Throw()
    {
        Rigidbody projectile = Instantiate(ThrownObjectTemplate, Aimer.OriginObject);
        projectile.gameObject.SetActive(true);
        projectile.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);

        projectile.transform.parent = null;

        projectile.AddRelativeForce(new Vector3(0,0,Velocity), ForceMode.VelocityChange);
        
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
