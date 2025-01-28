using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Tools.Tools;

public class darkGhostManager : MonoBehaviour
{
    public Sprite forceFollowSprite;
    public SpriteRenderer playerSprite;

    public bool tapped, active, oneTime = true, startChallenge = false, startConfusionScene = false, startGhostYesNo = false, answered = false, isWaitToReadRunning, startAgain = false;
    public GameObject mirror, player, stolenAnimal, mouse, discussion, scene;
    Animator animator, mirrorAnimator;

    public float forceFollowSpeed;

    joystickStats joystick;

    Dialogue dialogue;
    DialogueManager dialogueManag;

    Animator musicAnimator;
    //________FolloVw parameters_________
    // GameObject followTarget = null;
    // Vector2 followBounds = Vector2.zero;
    // bool manualFollow = true;
    //________Flip parameters___________

    void Start()
    {
        dialogue = gameObject.GetComponent<DialogueTrigger>().dialogue;
        dialogueManag = FindObjectOfType<DialogueManager>();
        animator = gameObject.GetComponent<Animator>();
        mirrorAnimator = mirror.GetComponent<Animator>();
        joystick = FindObjectOfType<joystickStats>();
        musicAnimator = GameObject.Find("music").GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if (active) {
            float distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
            float pointerDistance = Vector2.Distance(player.transform.position, gameObject.transform.Find("pointer").transform.position);
            tapped = mouse.GetComponent<InputManager>().tapped;

            if (FindObjectOfType<levelEnter>().allTasksDone) {
                gameObject.GetComponent<itemGrabTrigger>().startToTrigger = true;
            }

            if (pointerDistance < 40f && FindObjectOfType<levelEnter>().allTasksDone) {
              
                musicAnimator.Play("horrorSwitch");

                allignMessage(scene, gameObject, player, discussion);

                joystick.joystickAnimName = "joystickDisappear";

                if (FollowAndFinish()) {
                    dialogueManag.startDialogue = true;
                    if (oneTime) {
                        animator.SetBool("start", true);
                        dialogue.automatic = true;
                        oneTime = false;
                    }
                }

                if (!answered) {
                    if (startGhostYesNo) {
                        startYesNo();
                    }
                }

                else {
                    var reload = FindObjectOfType<ReloadAdditiveScene>().reload;
                    if (reload) {
                        dialogue.letter = "sentences3";
                        dialogue.automatic = true;
                        animator.SetBool("start", true);
                    }
                }
            }

            if (dialogueManag.startDialogue && !startGhostYesNo) StartDialogue(); /*StartCoroutine("WaitToRead");*/
            
            if (startConfusionScene) {
                if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("lastConfusionFCKU")) {
                    StartCoroutine(animFinished(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
                        if(animFinish) {
                            ES3.Save("startAgain", true);

                            var playerData = FindObjectOfType<Player>();
                            playerData.DeleteSaveFile(Application.persistentDataPath + "SaveFile.es3");
                            playerData.DeleteSaveFile(Application.persistentDataPath + "SaveFile.txt");

                            ES3.Save("level", 1);
                        }
                    }));
                }
            }
        }

        else {

            DestroyDarkGhost(mirrorAnimator);
        }
    }

    void DestroyDarkGhost(Animator mirrorAnim){
        mirrorAnim.Play("disappear");

        stolenAnimal.GetComponent<wantedAnimalTrigger>().interactable = true;
        
        StartCoroutine(animFinished(mirrorAnim.GetCurrentAnimatorStateInfo(0).length, animFinish => {
            if(animFinish) {
                mirror.SetActive(false);
                gameObject.SetActive(false);
            }
        }));
    }

    void StartAgain() {
        SceneManager.LoadScene("level1");
    }

    bool FollowAndFinish() {
        var target = gameObject.transform.Find("pointer");
        if (Vector2.Distance(player.transform.position, target.position) > 1f) {
            forceFollowSpeed -= 0.0012f;
            playerSprite.sprite = forceFollowSprite;
            player.transform.position = Vector2.MoveTowards(player.transform.position, target.position, forceFollowSpeed);
            return false;
        }

        else return true;
    }

    public void StartDialogue(string discussAnimName = "nothing") {
        
        gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
        
        if (dialogueManag.startDiscussion && !dialogueManag.endDiscussion) {

            joystick.joystickAnimName = "joystickDisappear";
            
            discussAnimName = "animalAppearMessage";
        }
        else if (!dialogueManag.startDialogue && !dialogueManag.startDiscussion && dialogueManag.endDiscussion) {

            if (answered) {
                if (dialogue.acceptYesNo) {
                    animator.Play("darkGhostDisappear");
                    mirrorAnimator.Play("disappear");
                    player.GetComponent<Animator>().enabled = true;
                    player.GetComponent<Animator>().Play("confusion");
                    player.GetComponent<Animator>().SetInteger("confusionLev", FindObjectOfType<settings>().levelNum);
                    startConfusionScene = true;

                    startAgain = true;
                }
                else {
                    if (dialogue.letter != "sentences3") {
                        startChallenge = true;

                        FindObjectOfType<InputManager>().touchControls.Disable();
                    }
                    else {
                        joystick.joystickAnimName = "joystickAppear";
                        active = false;
                    }
                }
            }

            else startGhostYesNo = true;
            
        }
        //______________ANIM NAMES______________
        discussion.GetComponent<Animator>().Play(discussAnimName);
        //______________ANIM NAMES______________
        
    }

    public void startYesNo(string yesNoAnimName = "flyingYesNo") {

        var yesNo = FindObject(discussion, "yesNo").gameObject;
        yesNoAnimName = "flyingYesNo";

        yesNo.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        bool yes = yesNo.transform.Find("yes").GetComponent<itemGrabTrigger>().triggered && tapped ? true : false;
        bool no = yesNo.transform.Find("no").GetComponent<itemGrabTrigger>().triggered && tapped ? true : false;
        
        if (no) {
            yesNoAnimName = "nothing";
            yesNo.gameObject.SetActive(false);
            startGhostYesNo = false;
            dialogue.acceptYesNo = false;
            dialogue.letter = "sentences1";
            dialogue.automatic = true;
            answered = true;
        }

        else if (yes) {
            yesNoAnimName = "nothing";
            yesNo.gameObject.SetActive(false);
            startGhostYesNo = false;
            dialogue.acceptYesNo = true;
            dialogue.letter = "sentences2";
            dialogue.automatic = true;
            answered = true;
        }

        //______________DISCUSS ANIM NAME______________
        discussion.GetComponent<Animator>().Play(yesNoAnimName);
        //______________DISCUSS ANIM NAME______________
    }
}
