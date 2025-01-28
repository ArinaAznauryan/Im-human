using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tools.Tools;

public class joyStickSystem : MonoBehaviour
{
    GameObject[] points;
    GameObject follower, joystick, joystickCollider;
    bool triggered;
    public Vector2 direction, movePos;
    joystickTrigger[] triggers;

    void Start()
    {
        follower = GameObject.FindWithTag("canvasFollower");
        joystick = GameObject.FindWithTag("joystick");
        joystickCollider = joystick.transform.Find("points").gameObject;

        points = ConvertToPoints();
    }

    GameObject[] ConvertToPoints() {
        var pointsGroup = joystick.transform.Find("points").gameObject;
        var result = GetGameObjectInChildren(pointsGroup);
        return result.ToArray();
    }

    void Update()
    {
        var touch = GameEventsManager.instance.inputEvents;
        triggers = new List<GameObject>(points).ConvertAll<joystickTrigger>(delegate(GameObject p_it) { return p_it.GetComponent<joystickTrigger>(); }).ToArray();
        if(!touch.clickUp && touch.clickDown && joystick.activeSelf) {
            
            foreach (GameObject point in points){
                if (point && point.GetComponent<joystickTrigger>().triggered) {
                    joystickCollider.gameObject.transform.localScale = new Vector2(2f, 2f);
                    break;
                }
            }
            
            foreach (joystickTrigger trigger in triggers) {
                if (trigger.triggered) {
                    direction = trigger.direction;
                    break;
                }
                direction = new Vector2(0, 0);
            }
        }
        else if (touch.clickUp && !touch.clickDown) {
            joystickCollider.transform.localScale = new Vector2(1f, 1f);
            direction = new Vector2(0, 0);
        }

        movePos = new Vector2(touch.joyMovePos.x, touch.joyMovePos.y);
        follower.transform.position = movePos; 
    }

}
