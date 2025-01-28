using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickStats : MonoBehaviour
{
    public string joystickAnimName = "nothing";
    GameObject joystick;

    void Start() {
        joystick = GameObject.FindWithTag("joystick");
    }

    public void DisableJoystick() {
        joystickAnimName = "joystickDisappear";
    }

    public void EnableJoystick() {
        joystickAnimName = "joystickAppear";
    }

    void Update()
    {
        joystick.GetComponent<Animator>().Play(joystickAnimName);
    }
}
