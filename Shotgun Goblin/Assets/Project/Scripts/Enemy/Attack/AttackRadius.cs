using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class AttackRadius : EnemyAttackScript
{
    public SphereCollider Collider;
    protected List<IDamageAbleByEnemy> damageAbles = new List<IDamageAbleByEnemy>();
    [SerializeField] public float Damage = 10.0f;
    [SerializeField] public float AttackDelay = 0.5f;
    public delegate void AttackEvent(IDamageAbleByEnemy Target);
    private AttackEvent OnAttack;
    protected Coroutine AttackCoroutine;
    private IOnEnemyAttack[] attacksScripts;//här
    private IOnEnemyHit[] hitsScripts;//här

    private void Start() //här
    {
        attacksScripts = GetComponents<IOnEnemyAttack>();
        hitsScripts = GetComponents<IOnEnemyHit>();
    }

    protected virtual void Awake()
    {
        Collider = GetComponent<SphereCollider>();    
    }

    protected void NotifyAttack()//här
    {
        foreach (var attacksScripts in attacksScripts)
        {
            attacksScripts.OnAttack();
        }
    }

    protected void NotifyHit(IDamageAbleByEnemy hitObject)//här
    {
        foreach (var hitsScripts in hitsScripts)
        {
            hitsScripts.OnAttackHit(hitObject, Damage);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageAbleByEnemy damageable = other.GetComponent<IDamageAbleByEnemy>();

        if (damageable != null)
        {
            damageAbles.Add(damageable);

            DebugLog("Target in hit radius: " + other.gameObject.name);

            if (AttackCoroutine == null)
            {
                AttackCoroutine = StartCoroutine(Attack());
                
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        IDamageAbleByEnemy damageable = other.GetComponent<IDamageAbleByEnemy>();

        if (damageable != null)
        {
            damageAbles.Remove(damageable);

            DebugLog("Target left radius: " + other.gameObject.name);


            if (damageAbles.Count == 0)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        yield return Wait;

        IDamageAbleByEnemy closestDamageable = null;  
        float closestDistance = float.MaxValue;

        while (damageAbles.Count > 0)
        {
            for (int i = 0; i < damageAbles.Count; i++)
            {
                Transform damageableTransform = damageAbles[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = damageAbles[i];
                }
            }

            if (AttackIsUnblocked())
            {

                NotifyAttack();

                if (closestDamageable != null)
                {
                    //OnAttack?.Invoke(closestDamageable);
                    //closestDamageable.TakeDamage(Damage);

                    NotifyHit(closestDamageable);

                    DebugLog("Hit Target: " + closestDamageable.GetTransform().name);

                }
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return Wait;

            damageAbles.RemoveAll(DisableDamageAbles);
        }

        AttackCoroutine = null;
    }

    protected bool DisableDamageAbles(IDamageAbleByEnemy damageAble)
    {
        return damageAble != null && !damageAble.GetTransform().gameObject.activeSelf;
    }
}

public interface IOnEnemyAttack//här
{
    public void OnAttack();
}

public interface IOnEnemyHit//här
{
    public void OnAttackHit(IDamageAbleByEnemy hitObject, float damage);


}
