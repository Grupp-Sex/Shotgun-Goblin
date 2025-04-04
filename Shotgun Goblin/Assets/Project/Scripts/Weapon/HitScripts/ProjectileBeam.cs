using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBeam : ProjectileVisuals
{
    protected ParticleSystem particles;
    [SerializeField] bool active = true;
    [SerializeField] float ParticlesPerLength = 1;
    [SerializeField] float MaxParticleDistance = 100;
    [SerializeField] float MinParticleDistance = 0;
    [SerializeField] bool OriginGunOrTarget = true;

    // Start is called before the first frame update
    void Start()
    {
        ResetBounds();
        particles = GetComponent<ParticleSystem>();
    }

    public override void Run(ProjectileInfo projectile)
    {
        if (active)
        {
            SetParticleBounds(projectile.origin, projectile.hitPos, out float distance);
            Emit(distance);
            ResetBounds();
        }
    }

    protected void ResetBounds()
    {
        transform.position = transform.TransformPoint(Vector3.zero);
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(0.1f,0.1f,1f);
    }

    protected void SetParticleBounds( Vector3 start, Vector3 end, out float distance)
    {
        distance = Vector3.Distance(transform.position, end);

        if (distance > MaxParticleDistance)
        {
            distance = MaxParticleDistance;
        }
        if(distance < MinParticleDistance)
        {
            distance = MinParticleDistance;
        }

        if (OriginGunOrTarget)
        {
            transform.LookAt(end);
        }
        else
        {
            transform.position = end;
            transform.LookAt(start);
        }

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
    }

    protected void Emit(float distance)
    {
        particles.Emit((int)(distance * ParticlesPerLength));
    }

    
}
