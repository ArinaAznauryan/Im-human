using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickTrigger : MonoBehaviour
{
    public bool triggered = false;
    public Vector2 direction;

    void Start()
    {
      
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        triggered = true;
            
    }
    void OnCollisionExit2D(Collision2D col) {
        triggered = false;
    }

}
