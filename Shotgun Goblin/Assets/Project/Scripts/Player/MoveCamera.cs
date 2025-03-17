using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform playerTransform;
    public float lerpValue;
  
    void Update()
    {
        //Byter ut sin egna transform (koordinater) f�r en annan transform vilket vi vill s�tta till 
        transform.position = playerTransform.position * lerpValue + transform.position * (1 - lerpValue); //Lerp g�r s� kameran "trailar" bakom spelaren f�r att g�ra en smooth movement utan jitter.
        transform.rotation = playerTransform.rotation;
    }
}


//Main Camera Holder �r parent till Main Camera vilket g�r att Main Camera �r bunden till Main Camera Holder. Ge Main Camera Holder detta script f�r att sedan �ndra Main Camera Holder's transform
//till MainCameraPos vilket �r en child av player. Detta g�r s� Main Camera f�ljer med player fast den inte �r en child av det GameObjektet.