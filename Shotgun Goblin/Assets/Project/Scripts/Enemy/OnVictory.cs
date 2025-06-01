using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnVictory : MonoBehaviour
{
    [SerializeField] int enemyKillThreshold = 70;

    public EnemyKillTalley EnemyKillTalley;

    public EventPusher<object> Event_Victory = new EventPusher<object>();

    [SerializeField] bool hasWon;

    // Start is called before the first frame update
    void Awake()
    {
        if(EnemyKillTalley  == null)
        {
            EnemyKillTalley = GetComponent<EnemyKillTalley>();
        }

        EnemyKillTalley.Event_EnemyKilled.Subscribe(Event_EnemyDeath);
    }

    public void Event_EnemyDeath(object sender, int count)
    {
        if(enemyKillThreshold <= count)
        {
            if (!hasWon)
            {
                Event_Victory.Invoke(this, null);
            }

            hasWon = true;

        }
    }
}
