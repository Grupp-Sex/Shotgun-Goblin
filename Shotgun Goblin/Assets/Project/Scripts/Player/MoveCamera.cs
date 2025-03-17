using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform playerTransform;
    public float lerpValue;
  
    void Update()
    {
        //Byter ut sin egna transform (koordinater) för en annan transform vilket vi vill sätta till 
        transform.position = playerTransform.position * lerpValue + transform.position * (1 - lerpValue); //Lerp gör så kameran "trailar" bakom spelaren för att göra en smooth movement utan jitter.
        transform.rotation = playerTransform.rotation;
    }
}


//Main Camera Holder är parent till Main Camera vilket gör att Main Camera är bunden till Main Camera Holder. Ge Main Camera Holder detta script för att sedan ändra Main Camera Holder's transform
//till MainCameraPos vilket är en child av player. Detta gör så Main Camera följer med player fast den inte är en child av det GameObjektet.