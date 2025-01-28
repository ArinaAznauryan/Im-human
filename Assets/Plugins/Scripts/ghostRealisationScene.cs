using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Tools.Tools;

public class ghostRealisationScene : MonoBehaviour
{
    public bool tapped, appearPlay = false, start = false, oneTime = true, startChallenge = false, startGhostYesNo = false, answered = false, isWaitToReadRunning, startAgain = false;
    public GameObject player, discussion, scene;
    public Animator animator;
    public Sprite[] scenePart2Sprites;

    Dialogue dialogue;
    DialogueManager dialogueManag;
    DialogueTrigger dialogueTrigger;


    void Start()
    {
        dialogue = gameObject.GetComponent<DialogueTrigger>().dialogue;
        dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        dialogueManag = FindObjectOfType<DialogueManager>();
        player.GetComponent<Animator>().enabled = true;
        animator = player.GetComponent<Animator>();

        ES3.DeleteFile("I'm human//SaveFile.es3");
        ES3.DeleteFile("I'm human//SaveFile.txt");
    }

    void LateUpdate()
    {
        tapped = GameEventsManager.instance.inputEvents.tapped;

        FindObjectOfType<joystickStats>().joystickAnimName = "joystickDisappear";

        if (dialogue.letter == "sentences0") animator.Play("realisation");
        else player.GetComponent<Animator>().enabled = false;

        if (player.GetComponent<Animator>().enabled) {
            StartCoroutine(animFinished(animator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
                if(animFinish) {
                    Debug.Log("anim finished...");
                    start = true;
                    dialogueManag.startDialogue = true;

                    if (oneTime) {
                        dialogue.automatic = true;
                        oneTime = false;
                    }
                }
            }));
        }

        if (start) StartDialogue();

    }

    public void StartDialogue(string discussAnimName = "nothing") {
        
        var thoughtsAnim = dialogueTrigger.thoughtsMessage.GetComponent<Animator>();
        
        gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();

        if (gameObject.GetComponent<itemGrabTrigger>().triggered && tapped) {
            thoughtsAnim.Play("appearMessage");
            thoughtsAnim.Play("idle");
        }

        if (dialogue.letter == "sentences1") {
            if (dialogueManag.currentLineIndex >= scenePart2Sprites.Length) dialogueManag.currentLineIndex = 0;
            player.transform.Find("Ghost").gameObject.GetComponent<SpriteRenderer>().sprite = scenePart2Sprites[dialogueManag.currentLineIndex];
        }

        if (dialogueManag.startDiscussion && !dialogueManag.endDiscussion) {
            FindObjectOfType<joystickStats>().joystickAnimName = "joystickDisappear";
            
            discussAnimName = "appearMessage";
        }

        else if (!dialogueManag.startDialogue && !dialogueManag.startDiscussion && dialogueManag.endDiscussion) {
            
            if (dialogue.letter == "sentences0") {
                dialogue.letter = "sentences1";
            }

            else if (dialogue.letter == "sentences1") {
                FindObjectOfType<joystickStats>().joystickAnimName = "joystickAppear";
                ES3.Save("startAgain", false);
                Destroy(this);
            }
 
        }
        //______________ANIM NAMES______________
        discussion.GetComponent<Animator>().Play(discussAnimName);
        //______________ANIM NAMES______________
        
    }
}
