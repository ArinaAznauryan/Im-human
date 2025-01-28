using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGlassesQuestStep1 : QuestStep
{
    private int glassCollected = 0; 
    private int glassToComplete = 6; 
    public string puzzleSpritePath;
    //public PuzzleChallenge puzzleChallenge;
    public Vector2 size;

    void Start() {
        gameObject.AddComponent<PuzzleChallenge>();
    }

    void Update() {
        //glassCollected = GameEventsManager.instance.itemGrabEvents.GetItemNumber("glass");

        var puzzleChallenge = gameObject.GetComponent<PuzzleChallenge>();

        if (puzzleChallenge.challengeState == PuzzleChallengeState.FINISHED) {
            FinishQuestStep();
        }
        else {
            puzzleChallenge.StartChallenge(puzzleSpritePath, size);
            //reload = true;
        }
    }
    
}
