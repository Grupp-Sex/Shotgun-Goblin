using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleatArrowCollision : MonoBehaviour
{

    public GameObject DeleatedGameObject;
    public GameObject CollisionDetection;
    protected ICollision collision1 => CollisionDetection.GetComponent<ICollision>();


    // Start is called before the first frame update
    void Awake()
    {
        if(DeleatedGameObject == null)
        {
            DeleatedGameObject = gameObject;
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

        DeleatedGameObject.SetActive(false);
        
    }
}
