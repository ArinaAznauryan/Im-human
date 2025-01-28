using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintAdditionalEventTrigger : AdditionalEventHandler.AdditionalEventHandler
{
    public bool additEvent = false, startOver = false;
    public int index = 0;

    override public void additionalEventTrigger() {
        additEvent = hintManager.trigger;
        index = hintManager.curHintIndex;
    }

    override public void annulParameters() {
        index = 0;
        startOver = false;
    }
}

public class hintManager : MonoBehaviour
{
    public hintAdditionalEventTrigger additEventHandlerTrigger;
    public GameObject animal, target;
    public GameObject[] hints;
    public static bool trigger = false;
    public itemGrab itemGrab;
    public static int curHintIndex = 0;
    public int goalNum = 0;

    void Start() {
        additEventHandlerTrigger = new hintAdditionalEventTrigger();

        if (!ES3.KeyExists("availableHints")) ES3.Save("availableHints", 0);
    }

    void Update() { 
        
        if (trigger) {
            trigger = false;
        }

        for (int i = 0; i<hints.Length; i++) {
            
            if (hints[i].GetComponent<hintTrigger>().triggered)  {
                target = hints[i];
                trigger = true;
                curHintIndex = i;
                
                triggersAre(false);
                break;
            }

            else trigger = false;

        }

        hintTask();

    }

    public void triggersAre(bool disableOrNot) { 
        foreach (GameObject hint in hints) {
            hint.GetComponent<itemGrabTrigger>().startToTrigger = disableOrNot;
        }
    }

    public void AddToGrabbables() { 
        hints[curHintIndex].GetComponent<itemGrabTrigger>().additEvent = true;
        itemGrab.items.Add(hints[curHintIndex]);
        Debug.Log("flop flop");
        additEventHandlerTrigger.annulParameters();
    }

    public void hintTask() {
        for (int i = 0; i < itemGrab.inventory.Length; i++) {
            if (itemGrab.inventory[i].type == "hint") {
                if (itemGrab.inventory[i].amount >= goalNum) {
                    FindObjectOfType<levelEnter>().allTasksDone = true;
                }
            }
        }
        
    }
}
