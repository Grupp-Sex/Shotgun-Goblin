using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnImpact : GenralSoundPlayer, IImapctManagerNotify
{
    [SerializeField] float threshold = 5;
    [SerializeField] ImpactThresholds.TypeOfImpactValue type;
    [SerializeField] AnimationCurve soundBySpeed;
    [SerializeField] float maxSpeed = 5;
    [SerializeField] float minSpeed = 1;
    [SerializeField] float cooldownDuration = 1;
    [SerializeField] bool isOnCool = false;
    protected float speedRange => maxSpeed - minSpeed;


    public void OnNotifyedCollision(CollisionData data)
    {
        if (!isOnCool)
        {

            float value = 0;

            switch (type)
            {
                case ImpactThresholds.TypeOfImpactValue.Relative_KineticEneryg:

                    value = data.kineticEnergy;

                    break;

                case ImpactThresholds.TypeOfImpactValue.Collider_KineticEneryg:

                    value = data.colider_kineticEnergy;

                    break;

                case ImpactThresholds.TypeOfImpactValue.Relative_Momentum:

                    value = data.momentum;

                    break;

                case ImpactThresholds.TypeOfImpactValue.Collider_Momentum:

                    value = data.colider_momentum;

                    break;
            }

            if (value > threshold)
            {
                PlaySounds(sounds, GetSpeedMult(data.relativeVelocity.magnitude));
            }

            StartCoroutine(Cooldown(cooldownDuration));
        }
    }

    protected IEnumerator Cooldown(float timer)
    {
        isOnCool = true;
        yield return new WaitForSeconds(timer);
        isOnCool = false;
    }

    protected float GetSpeedMult(float speed)
    {
        float lerpValue = (speed - minSpeed) / speedRange;

        lerpValue = Mathf.Clamp01(lerpValue);

        return soundBySpeed.Evaluate(lerpValue);
    }

    
}
