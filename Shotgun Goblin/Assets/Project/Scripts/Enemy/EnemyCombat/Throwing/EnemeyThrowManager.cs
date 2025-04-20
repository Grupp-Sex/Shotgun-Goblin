using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyThrowManager : MonobehaviorScript_ToggleLog
{
    [SerializeField] ObjectThrower Thrower;

    [SerializeField] EnemyMovement Entety;

    [SerializeField] bool ToggleAttacks = true;
    [SerializeField] float AttackSpeed = 5;
    [SerializeField] float AimSpeed = 1;
    [SerializeField] float MaxDistance = 30;
    

    protected bool CanAttack => WillHit && AttackStandby;

    [Header("Data")]

    [SerializeField] bool WillHit;
    [SerializeField] bool AttackStandby;
    

   

    private void OnEnable()
    {
        CooldownWeapon();
        StartCoroutine(AimTimer());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (ToggleAttacks)
        {
            if (CanAttack)
            {
                Shoot();
            }
        }
    }

  
    protected void SetTarget()
    {
        Thrower.Target = Entety.Target;

    }

    protected void AimThrower()
    {
        SetTarget();

        Thrower.Aim();

        WillHit = CheckDistance() && CheckThrejectory();

        if (WillHit) WillHit = TraceTrejectory();
    }

    protected bool CheckDistance()
    {
        return Vector3.SqrMagnitude(Thrower.Target.position - Thrower.GetAimerPos.position) < MaxDistance * MaxDistance;
    }

    protected bool CheckThrejectory()
    {
        return Thrower.Aimer.FoundTrejectory;
    }

    protected bool TraceTrejectory()
    {
        return true; // for future development
    }

    protected IEnumerator Cooldown(float timer)
    {
        AttackStandby = false;
        yield return new WaitForSeconds(timer);
        AimThrower();
        AttackStandby = true;
    }

    protected IEnumerator AimTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(AimSpeed);
            yield return new WaitForFixedUpdate();
            if (ToggleAttacks)
            {
                AimThrower();
            }
        }
    }

    protected void Shoot()
    {
        Thrower.Throw();
        CooldownWeapon();
    }

    protected void CooldownWeapon()
    {
        StartCoroutine(Cooldown(AttackSpeed));
    }


    

    

}
