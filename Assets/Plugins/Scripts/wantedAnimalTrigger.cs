using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Tools.Tools;

public class wantedAnimalTrigger : MonoBehaviour
{
    public int curLayer, saved = 0 /* 0 - hasn't started; 1 - started; 2 - finished and disabled*/, layerIndex, layerWeight, followPointerIndex, mode /* 0 — not interactable; 1 — interact to thank*/;
    public bool shyAnimal = false, additEventFinished = false, startFollow = false, startDarkGhostScene = false, tapped = false, lastSaved, interactable = false, active = true;
    public float movingSpeed, additionalEvent = 0;
    public GameObject scene, player, mouse, animal, parentAnimal, discussion;
    bool oneTime = true, mirrorDropOneTime = true;

    public float shyAnimalSpeed = 0f;
    bool shyAnimalRunAwayStart;
    public GameObject[] shyAnimalSpawnPoints;
    List<GameObject> spawnData;

    //public int shyAnimalEscapeNum;

    Vector2[] runAwayDirs;
    Vector2 randDir2D, shyAnimalVelocity;

    //__________FOLLOW PARAMETERS_________//
    GameObject followTarget;
    bool followManually = false;
    //__________FOLLOW PARAMETERS_________//

    Dialogue dialogue;
    DialogueManager dialogueManag;

    public animalFood animalFood;

    float addTaskBound = 15f;


    void Start()
    {
        followTarget = player;
        gameObject.GetComponent<Animator>().SetLayerWeight(layerIndex, layerWeight);

        spawnData = shyAnimalSpawnPoints.ToList();
    }

    void Update() {
        // if (active) {
        //     if (interactable) {
        //         if (mode == 1) {
        //             animal.GetComponent<itemGrabTrigger>().startToTrigger = true;
        //             dialogue = animal.GetComponent<DialogueTrigger>().dialogue;
        //             dialogueManag = FindObjectOfType<DialogueManager>();

        //             if (/*gameObject.GetComponent<Animator>().GetLayerWeight(3) == 1 ||*/ !startDarkGhostScene) {
        //                 gameObject.GetComponent<Animator>().SetLayerWeight(3, 1);
        //                 gameObject.GetComponent<Animator>().Play("mirrorDrop");
        //                 StartCoroutine(animFinished(animal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
        //                     if(animFinish) {
        //                         //gameObject.GetComponent<Animator>().SetLayerWeight(3, 0);
        //                         Debug.Log("in mirrorDrop scene");
        //                         //curLayer = 2;
        //                         startDarkGhostScene = true;

                                

        //                         //gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
        //                     }
        //                 }));
        //             }

        //             else {
        //                 //startDarkGhostScene = true;
        //                 if (mirrorDropOneTime) {
        //                     dialogue.automatic = true;
        //                     gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
        //                     gameObject.GetComponent<Animator>().SetLayerWeight(3, 0);
        //                     mirrorDropOneTime = false;
                            
        //                 }
        //             }

        //             if (startDarkGhostScene) {
        //                 allignMessage(scene, animal, player, discussion);
                        
        //                 var playerMovement = player.GetComponent<playerMovement>();

        //                 tapped = mouse.GetComponent<InputManager>().tapped;

        //                 if (animal.GetComponent<itemGrabTrigger>().triggered && tapped || dialogue.automatic) {
        //                     //Debug.Log("tapped biach");        
        //                     //if (!startAnimalYesNo && !searching) {
        //                         // playerMovement.mode = "goToAnimalStandPos";
        //                         // playerMovement.animal = animal;
        //                         // playerMovement.goToStandPosMode = 0;
        //                         dialogueManag.startDialogue = true;
        //                     //}
        //                 }

        //                 if (dialogueManag.startDialogue) StartDialogue();
        //             } 
        //         }
        //     }

            
        //     //if (gameObject.GetComponent<itemGrabTrigger>().triggered) savedWantAnimals++;
        //     if (lastSaved) {
        //         gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
        //         gameObject.GetComponent<Animator>().Play("rescue");
        //     }

        //     else if (saved) {
        //         gameObject.GetComponent<itemGrabTrigger>().triggered = false;
        //         gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
        //         gameObject.GetComponent<Animator>().SetLayerWeight(1, 0);
            
        //         Transform playerPointer = player.transform.Find("PlayerPointer" + followPointerIndex);
                
        //         followTarget = playerPointer.gameObject;
        //         FollowTarget(gameObject, followTarget, followManually, new Vector2(20f, 15f));

        //         if (oneTime) {
        //             gameObject.transform.position = playerPointer.position;
        //             oneTime = false;
        //         }
        //     }
        // }
    }

    void runAwayFromTarget(GameObject wantAnimal, GameObject target, Vector2 bounds = default(Vector2)) {
        var tarDir = player.GetComponent<playerMovement>();
        float distance = Vector2.Distance(target.transform.position, wantAnimal.transform.position);

        var wantAnimalSprite = wantAnimal.transform.Find(wantAnimal.name+"Sprite").GetComponent<SpriteRenderer>();

        var targetDir = direction(player.transform.position, gameObject.transform.position).x;

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(wantAnimal.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        // v = s/t; s = screenPoint; t = 1.5 seconds, myaaaa

        if (!shyAnimalRunAwayStart) {
            if (targetDir > 0) {
                wantAnimalSprite.flipX = false; 
                Debug.Log("rinchik: RIGHT");
                shyAnimalVelocity = -1*((screenPoint*44f)/8f)*shyAnimalSpeed;
            }

            else {
                wantAnimalSprite.flipX = true;
                Debug.Log("rinchik: LEFT");
                shyAnimalVelocity = ((screenPoint*44f)/8f)*shyAnimalSpeed;
            } 
        }

        else {
            if (targetDir < 0) {
                wantAnimalSprite.flipX = false; 
                //Debug.Log("rinchik: RIGHT");
                //ViewportToCanvasPosition
                //shyAnimalVelocity = -1*((screenPoint*44f)/8f)*shyAnimalSpeed;
                //runAwayDirs = new Vector2[]{new Vector2(0f, randomChoice(-1, 1)), new Vector2(randomChoice(-1f, -0.1f), Random.Range(-1f, 1f))};       
            }

            else {
                wantAnimalSprite.flipX = true;
                //Debug.Log("rinchik: LEFT");
                //shyAnimalVelocity = ((screenPoint*44f)/8f)*shyAnimalSpeed;
                //runAwayDirs = new Vector2[]{new Vector2(0f, randomChoice(-1, 1)), new Vector2(randomChoice(0.1f, 1f), Random.Range(-1f, 1f))};
            }
        }

    
        Debug.Log("ScreenPoint: " + screenPoint);
        
        if (distance < bounds[0]){
            shyAnimalRunAwayStart = true;
        }

        if (shyAnimalRunAwayStart) {
            wantAnimal.GetComponent<Animator>().Play("scared");
            wantAnimal.GetComponent<Animator>().SetBool("run", true);
            var velocity3D = new Vector3(shyAnimalVelocity.x, shyAnimalVelocity.y, 0f);
            wantAnimal.transform.position += velocity3D;
        }

        if (shyAnimalRunAwayStart/*distance > bounds[1]*/&& !onScreen){
            wantAnimal.GetComponent<Animator>().SetBool("run", false);
            shyAnimalRunAwayStart = false;
            teleportShyAnimalRandomly(wantAnimal);
            shyAnimalsData.shyAnimalEscapeNum++;
        }
    }

    void teleportShyAnimalRandomly(GameObject curShyAnimal) {

    //shyAnimEscapeNum
        Debug.Log("its teleporting bru");

        var subAnimals = parentAnimal.GetComponent<animalFindTrigger>().wantedAnimals;

        var breakOuterLoop = false;

        int bufNum = 0;

        if (spawnData.Count == 0) {
            spawnData = shyAnimalSpawnPoints.ToList();
        }
        
        var randIndex = Random.Range(0, spawnData.Count-1);
        var randomPos = spawnData[randIndex].transform.position;

        curShyAnimal.transform.position = randomPos;

        spawnData.RemoveAt(randIndex);

    }

    //_________________MAKE IT FASTER THAN THE PLAYER, MAKE IT GET OUT OF THE SCREEN FOCUS, AND THEN TELEPORT IT TO ANOTHER RANDOM POSITION_______________

    void FollowTarget(GameObject wantAnimal, GameObject target, bool manual, Vector2 bounds = default(Vector2)) {
        var tarDir = player.GetComponent<playerMovement>();
        float distance = Vector2.Distance(target.transform.position, wantAnimal.transform.position);

        var wantAnimalSprite = wantAnimal.transform.Find(wantAnimal.name+"Sprite").GetComponent<SpriteRenderer>();
        wantAnimalSprite.flipX = direction(player.transform.position, gameObject.transform.position).x < 0; 
        //bounds = new Vector3(max, min)
        if (!target) return;

        else {
            if (manual) {
                if (distance > bounds[0]){
                    //animFollowName = "run";
                    wantAnimal.GetComponent<Animator>().SetBool("run", true);
                    wantAnimal.transform.position = Vector2.MoveTowards(wantAnimal.transform.position, target.transform.position, movingSpeed*Time.deltaTime);
                    //if (tarDir.dir == new Vector2(0f, 0f))
                }

                else if (distance < bounds[1]){
                    /*if (tarDir.dir == new Vector2(0f, 0f))*/ //animFollowName = "idle";
                    wantAnimal.GetComponent<Animator>().SetBool("run", false);
                    //else animFollowName = "idle";
                }

                else if (distance > bounds[1] && distance < bounds[0]-1f) wantAnimal.GetComponent<Animator>().SetBool("run", false);

                else if (distance < bounds[0] && distance > bounds[0]-1f && tarDir.dir == new Vector2(0f, 0f)) wantAnimal.GetComponent<Animator>().SetBool("run", false);
                //Debug.Log(distance);
                //animal.GetComponent<Animator>().Play(animFollowName);
            }

            else {
                wantAnimal.transform.position = Vector2.MoveTowards(wantAnimal.transform.position, target.transform.position, movingSpeed*Time.deltaTime);
                wantAnimal.GetComponent<Animator>().SetBool("run", true);

                if (wantAnimal.transform.position.x == target.transform.position.x && wantAnimal.transform.position.y == target.transform.position.y) {
                    wantAnimal.GetComponent<Animator>().SetBool("run", false);
                    followManually = true;
                }
                //Debug.Log("in manual false");
            }
        }
    }

    public void StartDialogue(string discussAnimName = "nothing", string joystickAnimName = "nothing") {
        animal.GetComponent<DialogueTrigger>().TriggerDialogue();
        
        if (dialogueManag.startDiscussion && !dialogueManag.endDiscussion) {

            FindObjectOfType<joystickStats>().joystickAnimName = "joystickDisappear";

            animal.GetComponent<Animator>().Play("idle");
            discussAnimName = "animalAppearMessage";
        }
        else if (!dialogueManag.startDialogue && !dialogueManag.startDiscussion && dialogueManag.endDiscussion) {

            if (interactable && mode == 1) {
                PlayGoodbye();
            }
        
        }
        //______________ANIM NAMES______________
        discussion.GetComponent<Animator>().Play(discussAnimName);
        //______________ANIM NAMES______________
    }

    void PlayGoodbye() {

        FindObjectOfType<joystickStats>().joystickAnimName = "joystickAppear";

        gameObject.GetComponent<Animator>().SetLayerWeight(layerIndex, layerWeight);
        gameObject.GetComponent<Animator>().SetLayerWeight(3, 0);
        gameObject.GetComponent<Animator>().SetLayerWeight(2, 0);
        Debug.Log("MUST BE 2 TIMES, BITCH: " + layerWeight);
        animal.GetComponent<Animator>().Play("disappear");

        animal.GetComponent<itemGrabTrigger>().startToTrigger = false;
        
        //go away and disappear
        StartCoroutine(animFinished(animal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
            if(animFinish) active = false;
        }));
    }

    void LateUpdate() {

        if (active) {
            if (interactable) {

                var animalSprite = animal.transform.Find(animal.name+"Sprite").GetComponent<SpriteRenderer>();
                animalSprite.flipX = direction(player.transform.position, animal.transform.position).x < 0; 

                if (mode == 1) {
                    animal.GetComponent<itemGrabTrigger>().startToTrigger = true;
                    dialogue = animal.GetComponent<DialogueTrigger>().dialogue;
                    dialogueManag = FindObjectOfType<DialogueManager>();

                    if (/*gameObject.GetComponent<Animator>().GetLayerWeight(3) == 1 ||*/ !startDarkGhostScene) {
                        gameObject.GetComponent<Animator>().SetLayerWeight(3, 1);
                        gameObject.GetComponent<Animator>().Play("mirrorDrop");
                        StartCoroutine(animFinished(animal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
                            if(animFinish) {
                                //gameObject.GetComponent<Animator>().SetLayerWeight(3, 0);
                                Debug.Log("in mirrorDrop scene");
                                //curLayer = 2;
                                startDarkGhostScene = true;

                                

                                //gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
                            }
                        }));
                    }

                    else {
                        //startDarkGhostScene = true;
                        if (mirrorDropOneTime) {
                            dialogue.automatic = true;
                            gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
                            gameObject.GetComponent<Animator>().SetLayerWeight(3, 0);
                            mirrorDropOneTime = false;
                            
                        }
                    }

                    if (startDarkGhostScene) {
                        allignMessage(scene, animal, player, discussion);
                        
                        var playerMovement = player.GetComponent<playerMovement>();

                        tapped = mouse.GetComponent<InputManager>().tapped;

                        if (animal.GetComponent<itemGrabTrigger>().triggered && tapped || dialogue.automatic) {
                            //Debug.Log("tapped biach");        
                            //if (!startAnimalYesNo && !searching) {
                                // playerMovement.mode = "goToAnimalStandPos";
                                // playerMovement.animal = animal;
                                // playerMovement.goToStandPosMode = 0;
                                dialogueManag.startDialogue = true;
                            //}
                        }

                        if (dialogueManag.startDialogue) StartDialogue();
                    } 
                }
            }
            
            if (lastSaved) {
                Debug.Log("bro you gon be rescued bru");
                gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
                gameObject.GetComponent<Animator>().Play("rescue");
            }

            else if (saved == 1) {
                gameObject.GetComponent<itemGrabTrigger>().triggered = false;
                gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
                //gameObject.GetComponent<Animator>().SetLayerWeight(1, 0);
            
                Transform playerPointer = player.transform.Find("PlayerPointer" + followPointerIndex);
                
                followTarget = playerPointer.gameObject;
                FollowTarget(gameObject, followTarget, followManually, new Vector2(20f, 15f));

                if (oneTime) {
                    gameObject.transform.position = playerPointer.position;
                    oneTime = false;
                }
            }

            else if (saved == 2) {
                followTarget = null;
            }

            if (shyAnimal) {
                gameObject.GetComponent<itemGrabTrigger>().startToTrigger = false;
                gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
                gameObject.GetComponent<Animator>().SetLayerWeight(1, 0);

                if (additionalEvent != 3f && additionalEvent != 4f) { 
                    if (!RunFoodTask1(gameObject)) {
                        runAwayFromTarget(gameObject, player, new Vector2(15f, 15f));
                    } 
                }  
            }
        }

    }

    bool RunFoodTask1(/*animalFood animalFood*/GameObject wantAnimal) {
        var subAnimals = parentAnimal.GetComponent<animalFindTrigger>().wantedAnimals;

        bool additEventHasntStartedForEveryone = true;
        int bufNum = 0;

        for (int i = 0; i < subAnimals.Length; i++) {
            var subAnimalTrigger = subAnimals[i].GetComponent<wantedAnimalTrigger>();
            if (subAnimalTrigger.additionalEvent != 1f) {
                bufNum++;
                if (bufNum == subAnimals.Length) {
                    additEventHasntStartedForEveryone = true;
                    break;
                }
            }
        }

        List<GameObject> availableClosestAnimals = new List<GameObject>();

        foreach (GameObject subAnimal in subAnimals) {
            var subAnimalTrigger = subAnimal.GetComponent<wantedAnimalTrigger>();
            if (subAnimalTrigger.additionalEvent != 3f && subAnimalTrigger.additionalEvent != 4f) {
                availableClosestAnimals.Add(subAnimal);
            }
        }

        var closestIndex = GetClosestObjectIndex(availableClosestAnimals.ToArray(), player);
        FindObjectOfType<playerHelperArrow>().target = availableClosestAnimals[closestIndex];

        if (bufNum != subAnimals.Length) additEventHasntStartedForEveryone = false;

        if (additEventHasntStartedForEveryone) {
            if (shyAnimalsData.shyAnimalEscapeNum == 3f) {
                animalFood.EnableTrees();   
                shyAnimalsData.shyAnimalEscapeNum = 3+1;
                Debug.Log("hola amigo");
                additionalEvent = 1f;
            }
        }   

        var playerFood = player.GetComponent<foodStats>();

        float distance = Vector2.Distance(player.transform.position, wantAnimal.transform.position);

        if (playerFood.Has(animalFood.food.type) && playerFood.GetFood(animalFood.food.type).amount >= animalFood.foodAmountGoal) {

            if (addTaskBound != 0 && distance < addTaskBound) {
                FindObjectOfType<playerHelperArrow>().target = null;
                
                addTaskBound = 0f; 
                additionalEvent = 3f;

                var foodScore = parentAnimal.GetComponent<DialogueTrigger>().dialogue.foodScore;

                parentAnimal.GetComponent<DialogueTrigger>().dialogue.foodScore.GetComponent<itemScore>().score -= animalFood.foodAmountGoal;
                int score = foodScore.GetComponent<itemScore>().score;
                foodScore.GetComponent<TextMeshProUGUI>().text = (score).ToString();
                return true;
            }

            else {
                additionalEvent = 2.1f;
            }
        }

        return false;
    }
}
