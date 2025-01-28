using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance {get; private set; } 

    public int myGlasses, level;
    public QuestEvents questEvents;
    public itemGrab itemGrabEvents;
    public InputManager inputEvents;
    public QuestManager questManager;
    
    private void Awake() { 
        if (instance != null) Debug.LogError("Found more than one Game Events Manager in the scene."); 
        instance = this;

        questEvents = new QuestEvents();
    }

    void Start() {
        level = ES3.Load("level", 1);
    }
}
