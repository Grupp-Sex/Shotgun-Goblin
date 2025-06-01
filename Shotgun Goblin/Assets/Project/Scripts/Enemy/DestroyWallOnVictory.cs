using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWallOnVictory : MonoBehaviour
{
    public OnVictory OnVictory;
    public List<GameObject> Walls;

    // Start is called before the first frame update
    void Start()
    {
        if(OnVictory == null)
        {
            OnVictory = GetComponent<OnVictory>();
        }

        OnVictory.Event_Victory.Subscribe(Event_OnVictory);
    }

    protected void Event_OnVictory(object sender, object args)
    {
        for(int i = 0; i < Walls.Count; i++)
        {
            Walls[i].gameObject.SetActive(false);
        }
    }
}
