using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBySpeed : MonoBehaviour
{

    [SerializeField] float baseDamage;
    [SerializeField] float maxSpeed;
    [SerializeField] AnimationCurve damageCurve;
    public Rigidbody rigidbody;
    
    public GameObject CollisionDetection;
    protected ICollision collision1 => CollisionDetection.GetComponent<ICollision>();


    // Start is called before the first frame update
    void Awake()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        if (CollisionDetection == null)
        {
            CollisionDetection = gameObject;

        }
    }

    private void OnEnable()
    {
        collision1.Event_Collision.Subscribe(Event_Collision);

    }

    private void OnDisable()
    {
        collision1.Event_Collision.UnSubscribe(Event_Collision);
    }

    private void Event_Collision(object sender, Collision collision)
    {
        
        HealthManager health = collision.gameObject.GetComponent<HealthManager>();
        if (health == null) return;

        float speed = collision.relativeVelocity.magnitude;

        health.Damage(new DamageInfo() { damage = CalculateDamage(speed), position = collision.GetContact(0).point, direction = -collision.relativeVelocity });
    }


    protected float CalculateDamage(float speed)
    {
        float lerpValue = speed / maxSpeed;

        lerpValue = Mathf.Clamp01(lerpValue);
        
        float damageMult = damageCurve.Evaluate(lerpValue);

        return baseDamage * damageMult;


    }



}


