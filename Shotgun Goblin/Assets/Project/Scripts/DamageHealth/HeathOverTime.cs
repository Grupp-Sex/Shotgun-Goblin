using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathOverTime : MonoBehaviour
{
    public HealthManager healthManager;
    [SerializeField] float healthPerSec = 10;
    [SerializeField] float damageStopHealTimer = 2;
    [SerializeField] bool doHeal = true;

    protected void Event_Damage(object sender, float damage)
    {
        if(damage <= 0)
        {
            StartCoroutine(PauseHeal(damageStopHealTimer));
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(healthManager == null)
        {
            healthManager = GetComponent<HealthManager>();
        }

        healthManager.Event_HealthChanged.Subscribe(Event_Damage);
    }

    

    // Update is called once per frame
    void Update()
    {
        HealEffect();
    }

    protected void HealEffect()
    {
        if (doHeal)
        {
            healthManager.Heal(healthPerSec * Time.deltaTime);
        }
    }

    protected IEnumerator PauseHeal(float time)
    {
        doHeal = false;

        yield return new WaitForSeconds(time);

        doHeal = true;
    }


    
}
