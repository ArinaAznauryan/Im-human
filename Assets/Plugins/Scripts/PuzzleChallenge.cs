using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuzzleChallengeState
{
    START_CHALLENGE, 
    FINISH_CHALLENGE,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED,
    DISABLED
}

public class PuzzleChallenge : MonoBehaviour
{
    public PuzzleChallengeState challengeState;
    
    void LateUpdate() {
        Debug.Log("fck u, nigger");
        switch (challengeState) {
            case PuzzleChallengeState.START_CHALLENGE:
                break;
            default: break;

        }

        var reload = FindObjectOfType<ReloadAdditiveScene>().reload;

        if (reload) {
            challengeState = PuzzleChallengeState.FINISHED;
            FindObjectOfType<ReloadAdditiveScene>().reload = false;
            Destroy(this);
        }
    }

    public void StartChallenge(string spritePath, Vector2 size) {
        PlayerPrefs.SetString("puzzleSprite", spritePath);
        PlayerPrefs.SetInt("puzzleSize", (int)size.x*10 + (int)size.y);
        
        var levelEnter = FindObjectOfType<levelEnter>();
        levelEnter.TransitionLevel("puzzleScene", "levelTransition", true);

    }

}
