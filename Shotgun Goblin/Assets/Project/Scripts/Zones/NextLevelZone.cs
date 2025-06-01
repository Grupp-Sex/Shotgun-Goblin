using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NextLevelZone : MonoBehaviour
{
    [SerializeField] private float timer;
    public EventPusher<object> Event_GameEnd = new EventPusher<object>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartEndGameTimer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    protected void StartEndGameTimer()
    {
        StartCoroutine(EndGameTimer(timer));
    }

    protected IEnumerator EndGameTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        EndGame();
    }

    protected void EndGame()
    {
        Event_GameEnd.Invoke(this, null);
    }
}
