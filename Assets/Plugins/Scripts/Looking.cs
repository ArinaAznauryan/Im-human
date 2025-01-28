using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Looking : MonoBehaviour
{
    public CinemachineVirtualCamera cineCamera;
    public float coefficient, x, y, speed;
    public Vector3 velocity;
    public Transform target;
    void Start()
    {
        
    }

    void Update()
    {
        if (!cineCamera.Follow) cineCamera.Follow = target.transform;

        transform.position = new Vector3(transform.position.x, transform.position.y, -35f);
        
    }
}
