using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillTalley : MonoBehaviour
{
    public EventPusher<int> Event_EnemyKilled = new EventPusher<int>();

    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    [SerializeField] private int KillCounter = 0;

    // Start is called before the first frame update
    void Awake()
    {
        SubscribeToEnemySpawners();
    }

    protected void SubscribeToEnemySpawners()
    {
        for(int i = 0; i < enemySpawners.Count; i++)
        {
            enemySpawners[i].Event_ObjectSpawned.Subscribe(Event_EnemySpawned);
        }
    }

    protected void Event_EnemySpawned(object sender, GameObject newEnemy)
    {
        EventOnDestroy destroyEvent = newEnemy.AddComponent<EventOnDestroy>();

        destroyEvent.Event_OnDisable.Subscribe(Event_EnemyDeath);
    }

    protected void Event_EnemyDeath(object sender, object args)
    {
        KillCounter++;
        Event_EnemyKilled.Invoke(this, KillCounter);
    }
}
