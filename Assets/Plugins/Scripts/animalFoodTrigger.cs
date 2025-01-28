using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;
using static Tools.Tools;

public class animalFoodTrigger : MonoBehaviour
{
    public bool pausedOrClosed = false;
    
    public CinemachineVirtualCamera cineCamera;
    public int level, j = 0, curAnimal = 0;
    public GameObject scene, player, discussion, animalMessage, joystick, animal, highlighter;
    public Vector3 worldMousePos;
    public int helpedAnimals = 0;
    int tapNum;
    public bool addTapNum = true, taskCompleted = false, endDiscussion = false, addAnimalIntNum = false, resizeAnimalArray = false;
    public Camera camera;
    public float cameraMoveSpeed, highlightSpeed;
    public GameObject[] animals;
    int closestItemIndex;
    List<float> animalDistances;
    string mode, curButton, animFollowName = "idle";

    bool tapped, startAnimalYesNo = false, oneTime = true , continueDialogue = false; //for touch.tapped
    Dialogue dialogue;
    DialogueManager dialogueManag;

    IEnumerator animFinished(float delay, System.Action<bool> callback) {
        yield return new WaitForSeconds(delay);
        callback(true);
    }

    void Start()
    {
        animalDistances = new List<float>(new float[animals.Length]);
        
    }

    void Update()
    {     
        
        dialogue = animal.GetComponent<DialogueTrigger>().dialogue;
        dialogueManag = FindObjectOfType<DialogueManager>();
        tapped = GameEventsManager.instance.inputEvents.tapped;
        

        var animalSprite = animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>();
        animalSprite.flipX = direction(player.transform.position, animal.transform.position).x < 0;

        try {curButton = EventSystem.current.currentSelectedGameObject.name;}  
        catch (System.NullReferenceException e){}

        float distance = Vector2.Distance(player.transform.position, animal.transform.position);
        for (int i = 0; i<animals.Length; i++) {
            animalDistances[i] = Vector2.Distance(player.transform.position, animals[i].transform.position);
            if (i==(animals.Length-1)) break;
            float minVal = animalDistances.Min();
            closestItemIndex = animalDistances.IndexOf(minVal);
        }

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(animal.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (onScreen && animal == animals[closestItemIndex]) highlighter.transform.position = Vector3.MoveTowards(highlighter.transform.position, animal.transform.position, (highlightSpeed/Mathf.InverseLerp(0, 1, distance))*Time.deltaTime);
        
        if (distance < 30f) {
            if (oneTime) {
                if (GameObject.Find("tutorial") && gameObject.GetComponent<tutorialTrigger>()) {
                    gameObject.GetComponent<tutorialTrigger>().tutEvent = 1;
                    oneTime = false;
                }
            }

            allignMessage(scene, animal, player, animalMessage);

            var playerMovement = player.GetComponent<playerMovement>();

            if (dialogue.interactNum > 1) {
                    if (dialogue.foodScore.GetComponent<itemScore>().score >= dialogue.foodNum) {
                        dialogue.letter = "sentences2";
                        dialogue.taskCompleted = true;
                    }
                    else dialogue.taskCompleted = false;
            }
            else if (dialogue.interactNum <= 1) dialogue.taskCompleted = false;

        
            if (animal.GetComponent<itemGrabTrigger>().triggered && tapped) {
                tapNum++;

                if (gameObject.GetComponent<tutorialTrigger>()) gameObject.GetComponent<tutorialTrigger>().tutEvent = 2;
                playerMovement.mode = "goToAnimalStandPos";
                playerMovement.animal = animal;
                    
            }
 
        //________________QUESTION MARK_____________________//
            if (dialogue.taskCompleted) 
                animal.transform.Find("questionMark").gameObject.SetActive(true);

            if (dialogue.endDiscuss) {
                if (dialogue.taskCompleted) {
                    animal.GetComponent<Animator>().Play("running");
                    animal.GetComponent<itemGrabTrigger>().startToTrigger = false;
                }
                animal.transform.Find("questionMark").gameObject.SetActive(false);
                
                if(dialogue.minusScore) {
                    dialogue.foodScore.GetComponent<itemScore>().score -= dialogue.foodNum;
                    int score = dialogue.foodScore.GetComponent<itemScore>().score;
                    dialogue.foodScore.GetComponent<TextMeshProUGUI>().text = (score).ToString();
                    dialogue.minusScore = false;
                }
                //Destroy(this);
            }

        //________________QUESTION MARK_____________________// 
        }
      
        if (animals.Length == dialogueManag.helpedAnimals) FindObjectOfType<levelEnter>().allTasksDone = true;

    }
    
    
    Vector2 direction(Vector2 position1, Vector2 position2) {
        var dir = (position1 - position2).normalized;
        if (dir.x > 0 & dir.y > 0) dir = new Vector2(1f, 1f);
        else if (dir.x < 0 & dir.y < 0) dir = new Vector2(-1f, -1f);
        else if (dir.x < 0 & dir.y > 0) dir = new Vector2(-1f, 1f);
        else if (dir.x > 0 & dir.y < 0) dir = new Vector2(1f, -1f);
        else if (dir.x == 0 && dir.y != 0) {
            if (dir.y < 0) new Vector2(0f, -1f);
            else new Vector2(0f, 1f);
        }
        else if (dir.y == 0 && dir.x != 0) {
            if (dir.x < 0) dir = new Vector2(-1f, 0f);
            else dir = new Vector2(1f, 0f);
        }
        return dir;
    }

    float spriteFlipNormalize(SpriteRenderer sprite) {
        if (sprite.flipX) {
            Debug.Log("left");
            return -1;
        }
        else {
            Debug.Log("right");
            return 1;
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) pausedOrClosed = true;
        else pausedOrClosed = false;
    }

    public void StartDialogue(string discussAnimName = "nothing") {
        animal.GetComponent<DialogueTrigger>().TriggerDialogue();
        if (dialogueManag.startDiscussion && !dialogueManag.endDiscussion) {
            
            FindObjectOfType<joystickStats>().joystickAnimName = "joystickDisappear";

            animal.GetComponent<Animator>().Play("idle");
            discussAnimName = "animalAppearMessage";
        }

        else if (!dialogueManag.startDiscussion && dialogueManag.endDiscussion) {
            
            addAnimalIntNum = true;
            discussAnimName = "nothing";

            player.GetComponent<playerMovement>().mode = "joystick";
            
            FindObjectOfType<joystickStats>().joystickAnimName = "joystickAppear";
        
        }
        //______________DISCUSS ANIM NAME______________
        discussion.GetComponent<Animator>().Play(discussAnimName);
        //______________DISCUSS ANIM NAME______________
    }
    // public int GetIndexOfLowestValue(float[] arr) {
    //     float value = float.PositiveInfinity;
    //     int index = -1;
    //     for(int i = 0; i < arr.Length; i++)
    //     {
    //         if(arr[i] < value)
    //         {
    //             index = i;
    //             value = arr[i];
    //         }
    //     }
    //     return index;
    // }
}