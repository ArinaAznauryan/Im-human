using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFinishManager : MonoBehaviour
{
    public static EventFinishManager instance;
    public bool done = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FinishCallback(bool result)
    {
        if (result)
        {
            Debug.Log("Appear function finished, returned true. Do something!");
            done = true;
        }
    }
}
