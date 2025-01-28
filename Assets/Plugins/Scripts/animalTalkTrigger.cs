using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;
using static Tools.Tools;
using UnityEngine.Events;

public class animalTalkAdditionalEventTrigger : AdditionalEventHandler.AdditionalEventHandler
{
    public bool additEvent = false, startOver = false;
    //public int index = 0;

    override public void additionalEventTrigger() {
        additEvent = animalTalkTrigger.dialogue.acceptYesNo;
    }

    override public void annulParameters() {
    //     additE
     }
}

public class animalTalkTrigger : MonoBehaviour
{
    public UnityEvent eventTrigger, hintsEnabledEvent;

    public animalTalkAdditionalEventTrigger additEventHandlerTrigger;
    public bool pausedOrClosed = false;
    
    public CinemachineVirtualCamera cineCamera;
    public int level, j = 0, curAnimal = 0;
    public GameObject scene, mouse, player, discussion, animalMessage, joystick, animal, highlighter;
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

    //public additionalEventHandlerTrigger additEventTrigger;

    bool tapped, startAnimalYesNo = false, searching = false, oneTime = true , continueDialogue = false;

    public static Dialogue dialogue;
    DialogueManager dialogueManag;

    float movingSpeed = 30f;

    //________Follow parameters_________
        GameObject followTarget = null;
        Vector2 followBounds = Vector2.zero;
        bool manualFollow = true;
    //________Flip parameters___________

    void Start()
    {
        animalDistances = new List<float>(new float[animals.Length]);
        
    }

    void Update()
    {     
        dialogue = animal.GetComponent<DialogueTrigger>().dialogue;
        dialogueManag = FindObjectOfType<DialogueManager>();
        tapped = mouse.GetComponent<InputManager>().tapped;

        hintAdditionalEventTrigger additEventTrigger = null;
        if (animal.GetComponent<hintManager>()) {
            additEventTrigger = animal.GetComponent<hintManager>().additEventHandlerTrigger;
            additEventTrigger.additionalEventTrigger();
        }

    //___________SPRITE FLIP___________//
        var animalSprite = animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>();
        animalSprite.flipX = direction(player.transform.position, animal.transform.position).x < 0;

        try {
            //Debug.Log("NAME ORK: " + EventSystem.current.currentSelectedGameObject);/*curButton = EventSystem.current.currentSelectedGameObject.name;*/}  
        
            if (EventSystem.current.currentSelectedGameObject != null) curButton = EventSystem.current.currentSelectedGameObject.name;
        }
        catch (System.NullReferenceException e){}
    //___________SPRITE FLIP___________//

    //__________CLOSEST ANIMAL INDEX___________//
        float distance = Vector2.Distance(player.transform.position, animal.transform.position);
        for (int i = 0; i<animals.Length; i++) {
            animalDistances[i] = Vector2.Distance(player.transform.position, animals[i].transform.position);
            if (i==(animals.Length-1)) break;
            float minVal = animalDistances.Min();
            closestItemIndex = animalDistances.IndexOf(minVal);
        }
    //__________CLOSEST ANIMAL INDEX___________//

    //__________HIGHLIGHT__________//
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(animal.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (onScreen && animal == animals[closestItemIndex]) highlighter.transform.position = Vector3.MoveTowards(highlighter.transform.position, animal.transform.position, (highlightSpeed/Mathf.InverseLerp(0, 1, distance))*Time.deltaTime);
    //__________HIGHLIGHT__________//

//______________MAIN BODY______________//
        if (distance < 30f) {

        //___________________TUTORIAL EVENT TRIGGER_________________//
            if (oneTime) {
                if (GameObject.Find("tutorial") && gameObject.GetComponent<tutorialTrigger>()) {
                    gameObject.GetComponent<tutorialTrigger>().tutEvent = 1;
                    oneTime = false;
                }
            }
        //___________________TUTORIAL EVENT TRIGGER_________________//

            allignMessage(scene, animal, player, animalMessage);

            var playerMovement = player.GetComponent<playerMovement>();

            if (searching) {
                //Debug.Log("AdditEventTrigger: " + additEventTrigger.additEvent);
        
               if (animal.GetComponent<hintManager>()) {
                    if (additEventTrigger.additEvent) {
                        Debug.Log("sentences1 broksi");
                        dialogue.letter = "sentences1";
                        dialogue.automatic = true;
                        //additEventTrigger.additEvent = false;
                    }
                }
            }
            

        //_______________START DIALOGUE____________________//
            if (animal.GetComponent<itemGrabTrigger>().triggered && tapped || dialogue.automatic) {
                //Debug.Log("Its automatic biach");
                tapNum++;

                if (searching) {
                    if (additEventTrigger.additEvent && !dialogueManag.startDiscussion && dialogueManag.endDiscussion) {
                        allignPlayerToConversation(playerMovement);
                    }
                }

                else {
                    allignPlayerToConversation(playerMovement);
                }

                if (gameObject.GetComponent<tutorialTrigger>()) gameObject.GetComponent<tutorialTrigger>().tutEvent = 2;
                // playerMovement.mode = "goToAnimalStandPos";
                // playerMovement.animal = animal;
            }
 
            if (dialogueManag.startDialogue) {
                Debug.Log("kakafka");
                StartDialogue();
            }

            if (!dialogue.taskCompleted) {
                if (startAnimalYesNo/* && dialogue.interactNum > 1*/) startYesNo();
            }

        //_______________START DIALOGUE____________________//

        //________________QUESTION MARK_____________________//
            if (dialogue.taskCompleted) 
                animal.transform.Find("questionMark").gameObject.SetActive(true);

            if (dialogue.endDiscuss) {
                if (dialogue.taskCompleted) {
                    animal.GetComponent<Animator>().Play("running");
                    animal.GetComponent<itemGrabTrigger>().startToTrigger = false;
                    animal.GetComponent<itemGrabTrigger>().triggered = false;
                }
                animal.transform.Find("questionMark").gameObject.SetActive(false);
            }

        //________________QUESTION MARK_____________________// 
        }
      
        if (animals.Length == dialogueManag.helpedAnimals) FindObjectOfType<levelEnter>().allTasksDone = true;
//______________MAIN BODY______________//

        try {
            FollowTarget(animal, followTarget, manualFollow, followBounds);
            //Debug.Log(followTarget.name + manualFollow.ToString() + followBounds);
        }
        catch (System.NullReferenceException e) {}

    }

    void allignPlayerToConversation(playerMovement playerMovement) {
        playerMovement.mode = "goToAnimalStandPos";
        playerMovement.animal = animal;
        playerMovement.goToStandPosMode = 0;
        dialogueManag.startDialogue = true;
    }

    // void additEventInit(AdditionalEventHandlerTrigger additEvent) {
    //     additEvent.additionalEventTrigger();
    // }
    

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
        
        //Debug.Log("halo");
        animal.GetComponent<DialogueTrigger>().TriggerDialogue();
        if (dialogueManag.startDiscussion && !dialogueManag.endDiscussion) {
            
            //camera.transform.position = Vector3.MoveTowards(camera.transform.position, animal.transform.position, cameraMoveSpeed);
            //cineCamera.Follow = animal.transform;
            //joystick.SetActive(false);
            //FindObject(scene, "textRect").SetActive(true);
            FindObjectOfType<joystickStats>().joystickAnimName = "joystickDisappear";

            animal.GetComponent<Animator>().Play("idle");
            //Settings.animFinished(joystick.GetComponent<Animator>());
            //if (Settings.isAnimFinished) joystick.GetComponent<Animator>().Play("nothing");
            //discussion.GetComponent<Animator>().Play("animalAppearMessage");
            discussAnimName = "animalAppearMessage";
        }

        else if (!dialogueManag.startDiscussion && dialogueManag.endDiscussion) {

            //if (interactNum < 1) {
                //______________Set follow parameters_________________
                    followTarget = player;
                    manualFollow = true;
                    followBounds = new Vector2(20f, 15f);
                //______________Set follow parameters_________________
           // }

            addAnimalIntNum = true;
            discussAnimName = "nothing";

            
            if (searching) startAnimalYesNo = true;
 
            searching = true;

            player.GetComponent<playerMovement>().mode = "joystick";
            //player.GetComponent<playerMovement>().mode = "joystick";
            
            FindObjectOfType<joystickStats>().joystickAnimName = "joystickAppear";
            //}
            //FindObject(scene, "textRect").SetActive(true);
            //discussion.GetComponent<Animator>().Play("nothing");

            //if (animals[i].GetComponent<DialogueTrigger>().dialogue.taskCompleted) animals[i].transform.Find("questionMark").gameObject.SetActive(false);
        }
        //______________DISCUSS ANIM NAME______________
        discussion.GetComponent<Animator>().Play(discussAnimName);
        //______________DISCUSS ANIM NAME______________
    }

    void FollowTarget(GameObject animal, GameObject target, bool manual, Vector2 bounds = default(Vector2)) {
        var tarDir = player.GetComponent<playerMovement>();
        float distance = Vector2.Distance(target.transform.position, animal.transform.position);
        var animalSprite = animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>();

        //bounds = new Vector3(max, min)
        if (!target) return;

        else {
            if (manual) {
                if (distance > bounds[0]){
                    //animFollowName = "run";
                    animal.GetComponent<Animator>().SetBool("run", true);
                    animal.transform.position = Vector2.MoveTowards(animal.transform.position, target.transform.position, movingSpeed*Time.deltaTime);
                    //if (tarDir.dir == new Vector2(0f, 0f))
                }

                else if (distance < bounds[1]){
                    /*if (tarDir.dir == new Vector2(0f, 0f))*/ //animFollowName = "idle";
                    animal.GetComponent<Animator>().SetBool("run", false);
                    //else animFollowName = "idle";
                }

                else if (distance > bounds[1] && distance < bounds[0]-1f) animal.GetComponent<Animator>().SetBool("run", false);

                else if (distance < bounds[0] && distance > bounds[0]-1f && tarDir.dir == new Vector2(0f, 0f)) animal.GetComponent<Animator>().SetBool("run", false);
                //Debug.Log(distance);
                //animal.GetComponent<Animator>().Play(animFollowName);
            }

            else {
                animal.transform.position = Vector2.MoveTowards(animal.transform.position, target.transform.position, movingSpeed*Time.deltaTime);
                animal.GetComponent<Animator>().SetBool("run", true);

                if (animal.transform.position.x == target.transform.position.x && animal.transform.position.y == target.transform.position.y) animal.GetComponent<Animator>().SetBool("run", false);
                //Debug.Log("in manual false");
            }
        }
    }

    public void startYesNo(string yesNoAnimName = "nothing") {
        var additEventTrigger = animal.GetComponent<hintManager>().additEventHandlerTrigger;

        var yesNo = FindObject(discussion, "yesNo").gameObject;
        yesNoAnimName = "flyingYesNo";

        yesNo.transform.position = Camera.main.WorldToScreenPoint(animal.transform.position);
        
        var noTrigger = yesNo.transform.Find("no").GetComponent<itemGrabTrigger>().triggered;
        var yesTrigger = yesNo.transform.Find("yes").GetComponent<itemGrabTrigger>().triggered;

        if ((noTrigger || yesTrigger) && tapped) {

            yesNoAnimName = "nothing";
            yesNo.gameObject.SetActive(false);
            startAnimalYesNo = false;
            FindObjectOfType<joystickStats>().joystickAnimName = "joystickAppear";

            if (noTrigger) {
                dialogue.acceptYesNo = false;
                hintsEnabledEvent.Invoke();
            }

            else if (yesTrigger) {
                dialogue.acceptYesNo = true;
                eventTrigger.Invoke();
            }

            //____________________ADDITIONAL EVENT START OVER____________________
                additEventTrigger.startOver = true;
            //____________________ADDITIONAL EVENT START OVER____________________
        }

        // else if (yesTrigger && tapped) {

        //     yesNoAnimName = "nothing";
        //     yesNo.gameObject.SetActive(false);
        //     FindObjectOfType<joystickStats>().joystickAnimName = "joystickAppear";
        //     startAnimalYesNo = false;
        //     dialogue.acceptYesNo = true;
        //     //searching = true;
        // }

        
        
        //______________DISCUSS ANIM NAME______________
        discussion.GetComponent<Animator>().Play(yesNoAnimName);
        //______________DISCUSS ANIM NAME______________
    }

    void LateUpdate() {
        //var additEventTrigger = animal.GetComponent<hintManager>().additEventHandlerTrigger;
        //additEventTrigger.additionalEventTrigger();
        if (animal.GetComponent<hintManager>()) {
            var additEventTrigger = animal.GetComponent<hintManager>().additEventHandlerTrigger;
            if (additEventTrigger.additEvent) additEventTrigger.additEvent = false;
        }
    } 

}
