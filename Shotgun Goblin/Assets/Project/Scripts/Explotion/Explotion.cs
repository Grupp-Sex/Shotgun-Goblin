using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;

public class Explotion : MonoBehaviour, IExplotion
{
    [SerializeField] float ForceDuration;
    [SerializeField] float DamageDuration;


    [SerializeField] AnimationCurve ForceOverDistance;
    [SerializeField] AnimationCurve DamageOverDistance;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Explode()
    {

    }

}
