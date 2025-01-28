using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGlassesQuestStep : QuestStep
{
    private int glassCollected = 0; 
    private int glassToComplete = 6; 

    void Update() {
        glassCollected = GameEventsManager.instance.itemGrabEvents.GetItemNumber("glass");
        GlassesCollected();
        Debug.Log("Glasses: " + glassCollected);
    }
    
    private void GlassesCollected() { 
        // if (glassCollected < glassToComplete) { 
        //     glassCollected++; 
        // } 
        
        if (glassCollected >= glassToComplete) { 
            FinishQuestStep();
        }
    }
}
