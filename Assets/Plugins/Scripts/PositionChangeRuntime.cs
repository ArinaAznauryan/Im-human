using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChangeRuntime : MonoBehaviour
{
    [Header("Assign only in WORLD positions")]
    public Vector3 targetPos;
    public byte worldOrCanvas = 0; //world - 0; canvas - 1

    public Transform tarObj;
    public bool dependentOfObj = false;

    void Update()
    {
        Vector3 curPos;

        if (!dependentOfObj) curPos = targetPos;
        else curPos = tarObj.position;

        transform.position = worldOrCanvas == 1 ? Camera.main.WorldToScreenPoint(curPos) : curPos;
    }
}
