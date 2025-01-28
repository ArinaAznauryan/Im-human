using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintTrigger : MonoBehaviour
{
    public bool triggered = false;
    public GameObject mouse;

    
    void FixedUpdate()
    {
        var tapped = GameEventsManager.instance.inputEvents.tapped;

        triggered = gameObject.GetComponent<itemGrabTrigger>().triggered && tapped;
    }

}
