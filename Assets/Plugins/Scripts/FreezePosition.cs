using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePosition : MonoBehaviour
{
    public Vector2 targetPos;
    
    void Update()
    {
        transform.localPosition = targetPos;
    }
}
