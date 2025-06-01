using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUiSpawner : MonoBehaviour
{
    public GameObject UiTemplate;
    protected Game_Manager game_manager;
    public HealthManager health_manager;

    // Start is called before the first frame update
    void Awake()
    {
        if(health_manager == null)
        {
            health_manager = GetComponent<HealthManager>();
        }

        health_manager.Event_Death.Subscribe(Event_PlayerDeath);

        GameObject newUiTemplate = Instantiate(UiTemplate);
        game_manager = newUiTemplate.GetComponent<Game_Manager>();
    }

    protected void Event_PlayerDeath(object sender, DamageInfo damage)
    {
        game_manager.GameOver();
    }

    
}
