using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tools.Tools;
using System.Linq;
using Cinemachine;

public enum AnimalTaskState
{
    START_TASK, 
    END_TASK,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED,
    DISABLED
}

public class animalTaskManager : MonoBehaviour
{
    public AnimalTaskState taskState = AnimalTaskState.DISABLED;
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

    public joystickStats joystickStats;

    public bool tapped, startAnimalYesNo = false, searching = false;
    bool oneTime = true , continueDialogue = false;


    Dialogue dialogue;
    DialogueManager dialogueManag;

    float movingSpeed = 30f, distance;

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
        joystickStats = FindObjectOfType<joystickStats>();
        distance = Vector2.Distance(player.transform.position, animal.transform.position);

        FlipSprite();

        ClosestAnimalIndex();

        if (distance < 30) {
            allignMessage(scene, animal, player, animalMessage);

            var playerMovement = player.GetComponent<playerMovement>();
            
            if (animal.GetComponent<itemGrabTrigger>().triggered && tapped || dialogue.automatic) {
                if (!startAnimalYesNo && !searching && taskState != AnimalTaskState.FINISHED) {
                    tapNum++;
                    allignPlayerToConversation(playerMovement);
                }
            }

            if (dialogueManag.startDialogue) StartDialogue(); 

            if (!dialogue.taskCompleted) {
                if (startAnimalYesNo/* && dialogue.interactNum > 1*/) startYesNo();
            }

            //_______________START DIALOGUE______________//
        }
        
        AssignTaskState();
        AssignLetterState();
        CheckIfAllCompleted();

    }

    public void StartDialogue(string discussAnimName = "nothing", string joystickAnimName = "nothing") {
        
        Debug.Log("in the fucking dialogue");
        animal.GetComponent<DialogueTrigger>().TriggerDialogue();

        
        if (dialogueManag.startDiscussion && !dialogueManag.endDiscussion) {
            FindObjectOfType<joystickStats>().DisableJoystick();
            
            animal.GetComponent<Animator>().Play("idle");
            
            discussAnimName = "animalAppearMessage";
        }
        else if (!dialogueManag.startDialogue && !dialogueManag.startDiscussion && dialogueManag.endDiscussion) {

            if (!dialogue.acceptYesNo) {
                startAnimalYesNo = true;
            }

            else {
                FindObjectOfType<joystickStats>().EnableJoystick();
            }

            if (taskState == AnimalTaskState.CAN_FINISH) {
                taskState = AnimalTaskState.END_TASK;
                Debug.Log("DONT SPEAK");
            }
        }
        //______________ANIM NAMES______________
        discussion.GetComponent<Animator>().Play(discussAnimName);
        //______________ANIM NAMES______________
    }

    public void Follow() {
        try {
            FollowTarget(animal, followTarget, manualFollow, followBounds);
        }
        catch (System.NullReferenceException e) {}
    }

    public void allignPlayerToConversation(playerMovement playerMovement) {
        playerMovement.mode = "goToAnimalStandPos";
        playerMovement.animal = animal;
        playerMovement.goToStandPosMode = 0;
        dialogueManag.startDialogue = true;
    }

    void AssignTaskState() { 
        switch (taskState) {
            case AnimalTaskState.START_TASK:
            searching = true;
            break;

            case  AnimalTaskState.CAN_FINISH:
            searching = false;
            break;
        }
    }

    void AssignLetterState() { 
        switch (taskState) {
            case AnimalTaskState.START_TASK:
            break;

            case  AnimalTaskState.CAN_FINISH:
            dialogue.letter = "sentences2";
            break;
        }
    }

    public void startYesNo(string yesNoAnimName = "nothing") {
        var yesNo = FindObject(discussion, "yesNo").gameObject;
        yesNoAnimName = "flyingYesNo";

        yesNo.transform.position = Camera.main.WorldToScreenPoint(animal.transform.position);

        if (yesNo.transform.Find("no").GetComponent<itemGrabTrigger>().triggered && tapped) {
            yesNoAnimName = "nothing";
            yesNo.gameObject.SetActive(false);
            startAnimalYesNo = false;
            dialogue.acceptYesNo = false;
            
            joystickStats.joystickAnimName = "joystickAppear";
        }

        else if (yesNo.transform.Find("yes").GetComponent<itemGrabTrigger>().triggered && tapped) {
            taskState = AnimalTaskState.START_TASK;
            
            yesNoAnimName = "nothing";
            yesNo.gameObject.SetActive(false);

            joystickStats.joystickAnimName = "joystickAppear";
            startAnimalYesNo = false;
            dialogue.acceptYesNo = true;
        }
        //______________DISCUSS ANIM NAME______________
        discussion.GetComponent<Animator>().Play(yesNoAnimName);
        //______________DISCUSS ANIM NAME______________
    }

    public void CheckIfAllCompleted() {
        if (animals.Length == dialogueManag.helpedAnimals) FindObjectOfType<levelEnter>().allTasksDone = true;
    }

    public void FlipSprite() {
        var animalSprite = animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>();
        animalSprite.flipX = direction(player.transform.position, animal.transform.position).x < 0;
    }

    public void ClosestAnimalIndex() {
        for (int i = 0; i<animals.Length; i++) {
            animalDistances[i] = Vector2.Distance(player.transform.position, animals[i].transform.position);
            if (i==(animals.Length-1)) break;
            float minVal = animalDistances.Min();
            closestItemIndex = animalDistances.IndexOf(minVal);
        }
    }

    public void Highlight() {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(animal.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (onScreen && animal == animals[closestItemIndex]) highlighter.transform.position = Vector3.MoveTowards(highlighter.transform.position, animal.transform.position, (highlightSpeed/Mathf.InverseLerp(0, 1, distance))*Time.deltaTime);
    
    }

    void FollowTarget(GameObject animal, GameObject target, bool manual, Vector2 bounds = default(Vector2)) {
        var tarDir = player.GetComponent<playerMovement>();
        float tarDistance = Vector2.Distance(target.transform.position, animal.transform.position);
        var animalSprite = animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>();

        if (!target) return;

        else {
            if (manual) {
                if (tarDistance > bounds[0]){
                    animal.GetComponent<Animator>().SetBool("run", true);
                    animal.transform.position = Vector2.MoveTowards(animal.transform.position, target.transform.position, movingSpeed*Time.deltaTime);
                }

                else if (tarDistance < bounds[1]){
                    animal.GetComponent<Animator>().SetBool("run", false);
                }

                else if (tarDistance > bounds[1] && tarDistance < bounds[0]-1f) animal.GetComponent<Animator>().SetBool("run", false);

                else if (tarDistance < bounds[0] && tarDistance > bounds[0]-1f && tarDir.dir == new Vector2(0f, 0f)) animal.GetComponent<Animator>().SetBool("run", false);
                
            }

            else {
                animal.transform.position = Vector2.MoveTowards(animal.transform.position, target.transform.position, movingSpeed*Time.deltaTime);
                animal.GetComponent<Animator>().SetBool("run", true);

                if (animal.transform.position.x == target.transform.position.x && animal.transform.position.y == target.transform.position.y) animal.GetComponent<Animator>().SetBool("run", false);
           
            }
        }
    }
}
