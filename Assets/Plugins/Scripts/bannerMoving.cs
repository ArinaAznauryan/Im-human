using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bannerMoving : MonoBehaviour
{
    public GameObject banner;
    public float x, y, factor, rotationFactor;
    public Vector3 pos;
    void Start()
    {
        pos = banner.transform.position;
    }

    void Update()
    {
        banner.transform.rotation = Quaternion.Euler(0, 0, Mathf.Cos(Time.time)*rotationFactor);
        banner.transform.position = new Vector2(pos.x+Mathf.Cos(Time.time)*factor, pos.y+Mathf.Sin(Time.time)*factor);
    }
}
