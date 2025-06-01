using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    enum HealthSpecification
    {
        HealthNR,
        HealthProcent,
        DamageTaken,
    }
    [SerializeField] HealthManager DamagableObject;
    [SerializeField] TextMeshPro TextDisplay;
    [SerializeField] HealthSpecification NumberSpecification;


    // Update is called once per frame
    void Update()
    {
        if (NumberSpecification == HealthSpecification.HealthNR)
        {
            TextDisplay.text = Mathf.Round(DamagableObject.Health).ToString();
        }
        else if (NumberSpecification == HealthSpecification.HealthProcent)
        {
            float hpDiv = DamagableObject.Health / DamagableObject.MaxHealth;

            TextDisplay.text = Mathf.Round(hpDiv * 100).ToString() + "%";
        }
        else if (NumberSpecification == HealthSpecification.DamageTaken)
        {
            float damageTaken = -(DamagableObject.Health - DamagableObject.MaxHealth);

            TextDisplay.text = Mathf.Round(damageTaken).ToString();
        }
    }
}
