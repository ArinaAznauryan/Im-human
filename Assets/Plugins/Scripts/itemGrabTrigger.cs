using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemGrabTrigger : MonoBehaviour
{
    public bool triggered = false, subTriggered = false, startToTrigger, additEvent = false;
    public int score;
    public int scoreAdder;
    public bool nullWorldPos = false;
    private bool clicked;

    void Awake() {
    }
    void Update(){
        if (triggered) subTriggered = true;
        else subTriggered = false;

        CheckIfClicked();
    }

    void OnCollisionEnter2D(Collision2D col) {
        triggered = startToTrigger;
    }

    void OnCollisionExit2D(Collision2D col) {
        triggered = false;
        clicked = false;
    }

    void OnTriggerStay2D(Collider2D col) {
        triggered = startToTrigger;
    }

    void OnTriggerEnter2D(Collider2D col) {
        triggered = startToTrigger;
    }

    void CheckIfClicked() {
        clicked = triggered && GameEventsManager.instance.inputEvents.InputTapped();
    }
    
    void OnTriggerExit2D(Collider2D col) {
        triggered = false;
        clicked = false;
    }

    public bool OnClick() {return clicked;}


}
