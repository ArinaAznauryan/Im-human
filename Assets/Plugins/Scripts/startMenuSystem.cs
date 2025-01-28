using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startMenuSystem : MonoBehaviour
{
    //public Button startButton;
    public bool startButtonPressed = false;
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log("fuck you: " + startButtonPressed);
    }

    public void pressed() {
        startButtonPressed = true;
        //return true;
    }
}
