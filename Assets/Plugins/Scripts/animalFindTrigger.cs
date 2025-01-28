using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Events;
using static Tools.Tools;

public class shyAnimalsData : MonoBehaviour {
    public static int shyAnimalEscapeNum = 0;
}

public class animalFindTrigger : MonoBehaviour
{
    [Header("The 0-th index gotta be for \"YesNo()\" function")] 
    [Tooltip("The 0-th index gotta be for \"YesNo()\" function")] 
    public UnityEvent[] additEvents;
    private bool active = true;
    
    float movingSpeed = 30;

    public CinemachineVirtualCamera cineCamera;
    public int level;
    int j = 0, curAnimal = 0, conversationCompletedNum = 0;
    public GameObject scene, player, discussion, animalMessage, animal, highlighter;
    Vector3 worldMousePos;
    int helpedAnimals = 0, savedWantAnimals = 0;
    int tapNum;
    bool addTapNum = true, repeatOnce = true, searching = false, taskCompleted = false, endDiscussion = false, /*endTalk = false,*/ addAnimalIntNum = false, resizeAnimalArray = false;
    public Camera camera;
    public float cameraMoveSpeed, highlightSpeed;
    public GameObject[] animals, wantedAnimals;
    int closestItemIndex;
    List<float> animalDistances;
    string mode, curButton, animFollowName = "idle";
    GameObject curRescuedAnimal = null;
    public bool animalFollows = false;

    //________Follow parameters_________
    GameObject followTarget = null;
    Vector2 followBounds = Vector2.zero;
    bool manualFollow = true;
    //________Flip parameters___________

    GameObject flipTarget;

    bool tapped, startAnimalYesNo = false, oneTime = true;//,  taskStarted; //startDialogue = true; for touch.tapped
    public bool continueTask = false;
    bool oneTimeContinue = true;

    bool additionalEvent = false;
    //string discussAnimName = "nothing";
     Dialogue dialogue;
    DialogueManager dialogueManag;

    //bool _oneTime; 
    // bool OneTime { 
    //     get { return _oneTime; } 
    //     set { 
    //         if (!_oneTime && value) {
    //             savedWantAnimals++; _oneTime = value;
    //             value = false;
    //         }
    //     }
    // }

    IEnumerator animFinished(float delay, System.Action<bool> callback) {
        yield return new WaitForSeconds(delay);
        callback(true);
        //callback(true);
    }

    void Start()
    {
        animalDistances = new List<float>(new float[animals.Length]);
       // mode = animal.GetComponent<DialogueTrigger>().dialogue.mode.name;

        flipTarget = player;
        
    }

    void Update()
    {
        
        if (active) {
            
            //Debug.Log("tapped: " + tapped);
            dialogue = animal.GetComponent<DialogueTrigger>().dialogue;
            dialogueManag = FindObjectOfType<DialogueManager>();

            

            tapped = GameEventsManager.instance.inputEvents.tapped;

            var animalSprite = animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>();
            animalSprite.flipX = direction(flipTarget.transform.position, animal.transform.position).x < 0; 

            try {curButton = EventSystem.current.currentSelectedGameObject.name;}  
            catch (System.NullReferenceException e){}

            float distance = Vector2.Distance(player.transform.position, animal.transform.position);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var closestIndex = GetClosestObjectIndex(animals, player);
            //Debug.Log((highlightSpeed/Mathf.InverseLerp(0, 1, distance))*Time.deltaTime);
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(animal.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (onScreen && animal == animals[closestIndex]) {
                //if (mode == "findAnimal") {
                    if (!dialogue.acceptYesNo) highlighter.transform.position = Vector3.MoveTowards(highlighter.transform.position, animal.transform.position, (highlightSpeed/Mathf.InverseLerp(0, 1, distance))*Time.deltaTime);
                //}
                else highlighter.transform.position = Vector3.MoveTowards(highlighter.transform.position, animal.transform.position, (highlightSpeed/Mathf.InverseLerp(0, 1, distance))*Time.deltaTime);
            
            }
            //if (mode == "findAnimal") {
                //animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>().flipX = direction(player.transform.position, animal.transform.position).x < 0;
                if (distance < 30f) {
                    allignMessage(scene, animal, player, animalMessage);

                    var playerMovement = player.GetComponent<playerMovement>();
                    
                    if (animal.GetComponent<itemGrabTrigger>().triggered && tapped || dialogue.automatic) {
                        // if (dialogue.endDiscuss) {
                        //     if (dialogue.taskCompleted) {
                        //         //animal.GetComponent<Animator>().Play("running");
                        //         //animal.GetComponent<itemGrabTrigger>().startToTrigger = false;
                        //     }
                        // }
                            // curAnimal = i; 
                        //if (!startAnimalYesNo) {
                        //dialogue.automatic = false;
                        if (!additionalEvent && !startAnimalYesNo && !searching) {
                            tapNum++;

                            allignPlayerToConversation(playerMovement);
                        }

                        else if (additionalEvent) {
                            additionalEvent = false;
                            allignPlayerToConversation(playerMovement);
                        }
                        //}
                        /*if (!animalFollows)*/
                        
                        Debug.Log(animalFollows);
                    }

                    if (dialogueManag.startDialogue) StartDialogue(); 

                    if (!dialogue.taskCompleted) {
                        if (startAnimalYesNo) startYesNo();
                
                        if (dialogue.acceptYesNo) {
                            animalFollows = true;
                        }

                        if (animalFollows) {
                            foreach (GameObject wantAnimal in wantedAnimals) {
                                
                                var wantAnimalTrigger = wantAnimal.GetComponent<wantedAnimalTrigger>();
                                
                                if (wantAnimal.GetComponent<itemGrabTrigger>().triggered && tapped || wantAnimalTrigger.additionalEvent == 3f) {
                                    
                                    //dialogue.taskCompleted = true;
                                    //StartDialogue();
                                    Debug.Log("HOW MANY FCKN TIMES: " + wantAnimalTrigger.additionalEvent);
                                    searching = false;
                                    PlayRescued(wantAnimal);
                                    curRescuedAnimal = wantAnimal;
                                    break;
                                }

                                if (wantAnimalTrigger.additionalEvent == 1f) {
                                    dialogue.letter = "tipLetter";
                                    dialogue.automatic = true;
                                    wantAnimalTrigger.additionalEvent = 2f; 
                                    additionalEvent = true;

                                    //allignPlayerToConversation();
                                    break;
                                }
                            }
                        }
                    }

                    if (dialogue.endDiscuss) {
                        if (dialogue.taskCompleted) PlayGoodbye(/*curRescuedAnimal*/);
                        animal.transform.Find("questionMark").gameObject.SetActive(false);
                    }
                }  
                //} 

            if (animalFollows) {

                //______________Set follow parameters_________________
                followTarget = player;
                manualFollow = true;
                followBounds = new Vector2(20f, 15f);
                //______________Set follow parameters_________________

                if(repeatOnce) {
                   // Debug.Log("halo");
                    foreach (GameObject wantAnimal in wantedAnimals) {
                        wantAnimal.GetComponent<itemGrabTrigger>().startToTrigger = true; 
                        repeatOnce = false;
                    }
                }
            }

            if (animals.Length == dialogueManag.helpedAnimals) {
                FindObjectOfType<levelEnter>().allTasksDone = true;

                if (FindObject(scene, "darkMirror")) {
                    FindObject(scene, "darkMirror").gameObject.SetActive(true);
                }
            }

            try {
                FollowTarget(animal, followTarget, manualFollow, followBounds);
                //Debug.Log(followTarget.name + manualFollow.ToString() + followBounds);
            }
            catch (System.NullReferenceException e) {}
            
            // if (continueTask && oneTimeContinue) {
            //     ContinueTask();
            //     oneTimeContinue = false;
            // }
            
        }

        else {
            animal.SetActive(false);
            
            foreach (GameObject wantAnimal in wantedAnimals) {
                wantAnimal.SetActive(false);
            }
        }
        //Debug.Log("is active: " + FindObject(discussion, "yesNo").gameObject.activeSelf);
        
    }

    void allignPlayerToConversation(playerMovement playerMovement) {
        playerMovement.mode = "goToAnimalStandPos";
        playerMovement.animal = animal;
        playerMovement.goToStandPosMode = 0;
        dialogueManag.startDialogue = true;
    }
    


    void PlayGoodbye() {
        //go away and disappear
        dialogue.letter = "sentences2";

        FindObjectOfType<joystickStats>().EnableJoystick();
        animal.GetComponent<Animator>().Play("disappear"); 

        foreach (GameObject wantAnimal in wantedAnimals) {
            var wantAnimalTrigger = wantAnimal.GetComponent<wantedAnimalTrigger>();
            if (wantAnimalTrigger.saved == 1) wantAnimal.GetComponent<wantedAnimalTrigger>().saved = 2;
            Debug.Log("fuck want aNIMALS bro: " + wantAnimal.GetComponent<wantedAnimalTrigger>().followPointerIndex);
            wantAnimal.GetComponent<Animator>().Play("disappear");
            wantAnimal.GetComponent<itemGrabTrigger>().startToTrigger = false;
            animal.GetComponent<itemGrabTrigger>().startToTrigger = false;
        }
        
        //go away and disappear
        StartCoroutine(animFinished(animal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
            if(animFinish) active = false;
        }));
    }


    void PlayRescued(GameObject wantAnimal) {
        //dialogueManag.startDialogue = true;
        var wantAnimalTrigger = wantAnimal.GetComponent<wantedAnimalTrigger>();
        var wantAnimalItemGrabTrigger = wantAnimal.GetComponent<itemGrabTrigger>();

        if (wantAnimalItemGrabTrigger.triggered && tapped || wantAnimalTrigger.additionalEvent == 3f) {
            Debug.Log("polo: " + wantAnimal + wantAnimal.tag);
            Debug.Log("savedWantAnimals plus plus semicolon");
            savedWantAnimals++;
            wantAnimalTrigger.additionalEvent = 4f;
            wantAnimal.GetComponent<Animator>().SetLayerWeight(2, 1);
            //oneTime = false; 
        }

        // if (wantedAnimal.Length != savedWantAnimals) wantAnimalItemGrabTrigger.startToTrigger = false;
        // else wantAnimalItemGrabTrigger.startToTrigger = true;

        wantAnimalItemGrabTrigger.startToTrigger = (wantedAnimals.Length != savedWantAnimals && wantAnimalTrigger.additEventFinished) ? false : true;

        //FollowTarget(animal, wantedAnimal.transform.Find(wantedAnimal.name+"Pointer").gameObject, false);
        //if (OneTime)
        //savedWantAnimals++;
       // OneTime = true;
        

        if (wantedAnimals.Length == savedWantAnimals) {
            animalFollows = false;
            dialogue.letter = "sentences2";

            dialogue.taskCompleted = true;
            wantAnimalTrigger.lastSaved = true;

            //______________Set follow parameters_________________
            followTarget = wantAnimal.transform.Find(wantAnimal.name+"Pointer").gameObject;
            manualFollow = false;
            followBounds = Vector2.zero;
            //______________Set follow parameters_________________

            //______________Set flip parameter_________________
            flipTarget = wantAnimal.transform.Find(wantAnimal.name+"Pointer").gameObject;

            StartCoroutine(animFinished(wantAnimal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
                if(animFinish) {
                    //wantAnimal.GetComponent<itemGrabTrigger>().triggered = false;
                    animal.transform.Find("questionMark").gameObject.SetActive(true);
                }
            }));
        }

        else {
            dialogue.letter = "sentences1";
            dialogue.automatic = true;
            searching = false;
            //wantAnimal.GetComponent<wantedAnimalTrigger>().startFollow = true;
            wantAnimalTrigger.saved = 1;
            //MAKE SEARCHING TRUE AFTER THE DISCUSSION WITH ANIMAL AFTER FINDING THE WANTED ANIMAL, SOMEHOW BRUH
            //if (!dialogueManag.startDialogue) searching = true;
        }
    }

    public void StartDialogue(string discussAnimName = "nothing", string joystickAnimName = "nothing") {
        
        Debug.Log("in the fucking dialogue");
        animal.GetComponent<DialogueTrigger>().TriggerDialogue();

        
        if (dialogueManag.startDiscussion && !dialogueManag.endDiscussion) {
            //camera.transform.position = Vector3.MoveTowards(camera.transform.position, animal.transform.position, cameraMoveSpeed);
            //cineCamera.Follow = animal.transform;
            
            //FindObject(scene, "textRect").SetActive(true);
            FindObjectOfType<joystickStats>().DisableJoystick();
            //input.touchControls.Disable();
            //joystick.SetActive(false);

            animal.GetComponent<Animator>().Play("idle");
            //Settings.animFinished(joystick.GetComponent<Animator>());
            //if (Settings.isAnimFinished) joystick.GetComponent<Animator>().Play("nothing");
            //discussion.GetComponent<Animator>().Play("animalAppearMessage");
            discussAnimName = "animalAppearMessage";
        }
        else if (!dialogueManag.startDialogue && !dialogueManag.startDiscussion && dialogueManag.endDiscussion) {
            //cineCamera.Follow = null;
            //animalInteractNum++;
            //Debug.Log("SHIIIIIIIIIIIIIIIIIT");
            //else if (mode == "findAnimal") {

            conversationCompletedNum++;
            
            if (savedWantAnimals < 1 && !dialogue.acceptYesNo/*conversationCompletedNum == 1 &&*/ /*!startAnimalYesNo*/) {
                startAnimalYesNo = true;
            }

            else {
                FindObjectOfType<joystickStats>().EnableJoystick();
                if (!searching) searching = true;
            }
            Debug.Log("startAnimalYesNo: " + startAnimalYesNo);
            addAnimalIntNum = true;

            
            //startDialogue = false;
            //Debug.Log("I hate myself");
            //if (endTalk) {
            //player.GetComponent<playerMovement>().mode = "joystick";
            //input.touchControls.Enable();x
            //joystick.SetActive(true);
            //discussAnimName = "nothing";
            //endTalk = false;
            //}
            //}
            //FindObject(scene, "textRect").SetActive(true);
            //discussion.GetComponent<Animator>().Play("nothing");

            //if (animals[i].GetComponent<DialogueTrigger>().dialogue.taskCompleted) animals[i].transform.Find("questionMark").gameObject.SetActive(false);
        }
        //______________ANIM NAMES______________
        discussion.GetComponent<Animator>().Play(discussAnimName);
        //______________ANIM NAMES______________
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
            
            FindObjectOfType<joystickStats>().EnableJoystick();
        }

        else if (yesNo.transform.Find("yes").GetComponent<itemGrabTrigger>().triggered && tapped) {

            foreach (GameObject wantAnimal in wantedAnimals) {
                wantAnimal.SetActive(true);
                wantAnimal.GetComponent<wantedAnimalTrigger>().enabled = true;
            }

            yesNoAnimName = "nothing";
            yesNo.gameObject.SetActive(false);

            FindObjectOfType<joystickStats>().EnableJoystick();
            startAnimalYesNo = false;
            dialogue.acceptYesNo = true;

            if (additEvents.Length > 0) additEvents[0].Invoke();

            searching = true;
        }
        //______________DISCUSS ANIM NAME______________
        discussion.GetComponent<Animator>().Play(yesNoAnimName);
        //______________DISCUSS ANIM NAME______________
    }

    // void ContinueTask() {
    //     startAnimalYesNo = false;
    //     dialogue.acceptYesNo = true;
    //     searching = true;
    // }

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
}

