using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movableObjTrigger : MonoBehaviour
{

    public GameObject mouse;
    public movableObjManager manager;
    public bool move = false;
    bool once = true;

    void Update()
    {
        var touch = GameEventsManager.instance.inputEvents;
        var dragged = touch.dragged;
        var tapped = touch.InputTapped(); 

        var mouseWorldPos = GameEventsManager.instance.inputEvents.joyWorldMovePos;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouseWorldPos);
        if (Physics2D.Raycast(mouseWorldPos, new Vector2(0f, 0f), 100)) {//Physics2D.Raycast(player.transform.position, dir, distance, doorMask);
        
        }
        
        if (gameObject.GetComponent<itemGrabTrigger>().triggered) {

            Debug.Log("BRO UR NOT IN AN EDIT BRUUU");

            if (dragged) {
                if (!manager.target) {
                    once = true;
                    manager.target = gameObject;
                    move = true;
                }
            }
        }

        if (!gameObject.GetComponent<itemGrabTrigger>().triggered && !dragged) {
            move = false;
            manager.target = null;
        }

        if (!dragged) {
            if (once) {
                
                gameObject.GetComponent<itemGrabTrigger>().triggered = false; //_________Do once
                once = false;

                move = false;
            }
            manager.target = null;
            
        }

        if (move) {
            gameObject.transform.position = mouseWorldPos;
        }
    }
}
