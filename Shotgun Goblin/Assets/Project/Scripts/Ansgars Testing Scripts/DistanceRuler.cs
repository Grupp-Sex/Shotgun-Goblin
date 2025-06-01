using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceRuler : MonoBehaviour
{
    [SerializeField] Transform Golpost1;
    [SerializeField] Transform Golpost2;
    [SerializeField] Transform Runner;
    [SerializeField] TextMeshPro Text;
    [SerializeField] float Distance;
    [SerializeField] float LerpDistance;

    
   

    // Update is called once per frame
    void Update()
    {
        UpdateTracker();
        UpdatePosition();
        UpdateText();
    }

    protected void UpdateTracker()
    {
        Vector3 goalpostDirection = Golpost2.position - Golpost1.position;

        Vector3 runnerLocal = Runner.position - Golpost1.position;

        Vector3 runnerProjection = Vector3.Project(runnerLocal, goalpostDirection.normalized);
        Distance = runnerProjection.magnitude;
        LerpDistance = Distance / goalpostDirection.magnitude;



    }

    protected void UpdatePosition()
    {
        float tLerpDistance = Mathf.Clamp(LerpDistance, 0, 1);

        transform.position = Golpost2.position * tLerpDistance + Golpost1.position * (1 - tLerpDistance);
    }

    protected void UpdateText()
    {
        Text.text = Mathf.Round(Distance).ToString();
        Text.transform.LookAt(Runner.position);
        Text.transform.Rotate(new Vector3(0, 1, 0), 180f);
    }
}
