using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TelekenesisManager : MonoBehaviour
{
    
    [SerializeField] public List<TelekenesisPhysicsObject> HeldObjects {  get; protected set; }
    [SerializeField] public float GrabDistanceThreshold { get; protected set; }
    [SerializeField] public float HoldDistanceThreshold { get; protected set; }

    [SerializeField] public Transform TargetPosition {  get; protected set; }


    public static float Vector3DistanceSquared(Vector3 a, Vector3 b)
    {
        Vector3 ab = a - b;

        return Vector3.SqrMagnitude(ab);
    }

    // get all objects that can be affected by telekenesis
    public TelekenesisPhysicsObject[] GetAllAfectableObjects()
    {
        return FindObjectsOfType<TelekenesisPhysicsObject>();
    }

    protected void TelekenesisGrabObjects()
    {
        
    }

    private void AddObjectsWithinThreasholdToList(List<GameObject> targetList, Vector3 originPos, float threshold, GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(Vector3DistanceSquared(originPos, objects[i].transform.position) <= threshold * threshold)
            {
                if (!targetList.Contains(objects[i]))
                {
                    targetList.Add(objects[i]);
                }
            }
        }
    }

    protected virtual void RunTelekeneisAbilaty()
    {

    }
}
public abstract class BaseTelekenesisAbilaty : MonoBehaviour
{


}
