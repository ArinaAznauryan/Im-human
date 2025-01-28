using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllHints {
    public LevelHint[] allHints;
}

[System.Serializable]
public class LevelHint {
    public byte status;
    public Quests[] quests;
}

[System.Serializable]
public class Quests {
    public string[] description;
}

public class hintDispenser : MonoBehaviour
{
    public AudioSource hintAudio;
    public int availableHints = 0, openedHints = 0, j = 0;
    private TextAsset hintsJson;
    private AllHints allHintsJson;
    private int level, allHintsNum;

    void Start() {
        level = GameEventsManager.instance.level;

        hintsJson = Resources.Load<TextAsset>("HintData/hints");
        availableHints = ES3.Load<int>("availableHints", 0);
        allHintsJson = JsonUtility.FromJson<AllHints>(hintsJson.text);
        allHintsNum = allHintsJson.allHints[level-1].quests[0].description.Length;

        if (availableHints >= allHintsNum) availableHints = allHintsNum;
    }

    void Update() {
            
        bool tapped = GameEventsManager.instance.inputEvents.tapped, triggered = gameObject.GetComponent<itemGrabTrigger>().triggered;
        bool clicked = tapped && triggered ? true : false;
           
        if (clicked) {
            if (availableHints > 0) {
        
                if (allHintsJson.allHints[level-1].quests.Length > 0) {

                    string curHintDescription = allHintsJson.allHints[level-1].quests[0].description[j];
                    
                    GameEventsManager.instance.questManager.questHintRef.PlayHint(curHintDescription);
                
                    if (j == availableHints-1) j = 0;
                    else j++;
                }
            }
        } 
    }

}
