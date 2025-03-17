using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;


    private void Awake()
    {
        offset = transform.position - player.position;
    }


    private void fixedfUpdate()
    {
        Vector3 playerPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref currentVelocity, smoothTime);
    }
}
