using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class AttackRadius : MonoBehaviour
{
    public SphereCollider Collider;
    private List<IDamageAbleByEnemy> damageAbles = new List<IDamageAbleByEnemy>();
    [SerializeField] public float Damage = 10.0f;
    [SerializeField] public float AttackDelay = 0.5f;
    public delegate void AttackEvent(IDamageAbleByEnemy Target);
    private AttackEvent OnAttack;
    private Coroutine AttackCoroutine;
    private IOnEnemyAttack[] attacksScripts;
    private IOnEnemyHit[] hitsScripts;

    private void Start()
    {
        attacksScripts = GetComponents<IOnEnemyAttack>();
        hitsScripts = GetComponents<IOnEnemyHit>();
    }

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();    
    }

    protected void NotifyAttack()
    {
        foreach (var attacksScripts in attacksScripts)
        {
            attacksScripts.OnAttack();
        }
    }

    protected void NotifyHit(IDamageAbleByEnemy hitObject)
    {
        foreach (var hitsScripts in hitsScripts)
        {
            hitsScripts.OnAttackHit(hitObject, Damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAbleByEnemy damageable = other.GetComponent<IDamageAbleByEnemy>();

        if (damageable != null)
        {
            damageAbles.Add(damageable);  
            
            if (AttackCoroutine == null)
            {
                AttackCoroutine = StartCoroutine(Attack());
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageAbleByEnemy damageable = other.GetComponent<IDamageAbleByEnemy>();

        if (damageable != null)
        {
            damageAbles.Remove(damageable);

            if (damageAbles.Count == 0)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
        }
    }

    private IEnumerator Attack()
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

            NotifyAttack();

            if (closestDamageable != null)
            {
                //OnAttack?.Invoke(closestDamageable);
                //closestDamageable.TakeDamage(Damage);

                NotifyHit(closestDamageable);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return Wait;

            damageAbles.RemoveAll(DisableDamageAbles);
        }

        AttackCoroutine = null;
    }

    private bool DisableDamageAbles(IDamageAbleByEnemy damageAble)
    {
        return damageAble != null && !damageAble.GetTransform().gameObject.activeSelf;
    }
}

public interface IOnEnemyAttack
{
    public void OnAttack();
}

public interface IOnEnemyHit
{
    public void OnAttackHit(IDamageAbleByEnemy hitObject, float damage);


}
