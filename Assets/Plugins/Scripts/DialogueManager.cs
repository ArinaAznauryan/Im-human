using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using System.Linq;


public class DialogueManager : MonoBehaviour
{
    public CinemachineVirtualCamera cineCamera;
   // public AudioSource animalSounds;
    public Queue<string> sentences;
    public string lastEl;
    string sent;
    public int currentLineIndex = 0;
    public bool startDiscussion = false, endDiscussion = false, startDialogue;
    public GameObject scene, /*animalMessage,*/ player, mouse;
    public int helpedAnimals = 0;

    bool tapped = false;
    //_____________________DONT FORGET!!!!!!!!!!!___________GET DIALOGUE AUTO-SAVE PARAMETERS BACK, IF YOU TEST IT OUT AND IT'S SHIT.
    void Start()
    {
        if (ES3.KeyExists("startAgain")) {
            if (ES3.Load<bool>("startAgain")) {
                if (ES3.Load<int>("level") == 1) player.GetComponent<ghostRealisationScene>().enabled = true;
                else ES3.Save("startAgain", false);
            }
        }

        sentences = new Queue<string>();
    }

    void Update() 
    {
        tapped = GameEventsManager.instance.inputEvents.tapped;
    }
    //______________________________________________________________--DIALOGUE TRIGGER.ANIMALSOUNDS PLAY SOMEHOW___________________________________----

    public void StartDialogue(Dialogue dialogue, GameObject animal/*, bool thoughts, Animator thoughtsMessage = null*/) {
        Debug.Log("automatic shit: " + dialogue.automatic);
        if ((animal.GetComponent<itemGrabTrigger>().triggered && tapped) || dialogue.automatic) {
            Debug.Log("in start dialogue....");
            var currentLetter = dialogue.sentences;

            dialogue.automatic = false;

            startDiscussion = true;
            endDiscussion = false;

            cineCamera.Follow = animal.transform;

            var letter = dialogue.letter;
            if (letter == "sentences0") {
                //sentences.Clear();
                Debug.Log("in the sentences0 bruh");
                currentLetter = dialogue.sentences;
                // foreach (string sentence in dialogue.sentences) {
                //     //Debug.Log("Count: " + sentences.Count());
                //     /*if (sentences.Count != dialogue.sentences.Length)*/ sentences.Enqueue(sentence);
                //     lastEl = sentence;
                // }
            }
            if (letter == "sentences1") {
                //sentences.Clear();
                
                currentLetter = dialogue.sentences1;
                // foreach (string sentence in dialogue.sentences1) {
                //     /*if (sentences.Count != dialogue.sentences.Length)*/ sentences.Enqueue(sentence);
                //     lastEl = sentence;
                // }
            }

            if (letter == "sentences2") {
                //sentences.Clear();
                currentLetter = dialogue.sentences2;
                // foreach (string sentence in currentLetter) {
                //     /*if (sentences.Count != dialogue.sentences.Length)*/ sentences.Enqueue(sentence);
                //     lastEl = sentence;
                // }
            }

            if (letter == "sentences3") {
                Debug.Log("in sentences 3...");
                //sentences.Clear();
                currentLetter = dialogue.sentences3;
            }

            if (letter == "tipLetter") {
                //sentences.Clear();
                Debug.Log("in the tip letter bruh...");
                currentLetter = dialogue.tipLetter[dialogue.letterIndex].letters;
                // foreach (string sentence in dialogue.sentences1) {
                //     /*if (sentences.Count != dialogue.sentences.Length)*/ sentences.Enqueue(sentence);
                //     lastEl = sentence;
                // }
            }

            foreach (string sentence in currentLetter) {
                    /*if (sentences.Count != dialogue.sentences.Length)*/ sentences.Enqueue(sentence);
                    lastEl = sentence;
                }

            DisplayNextSentence(dialogue, currentLetter/*, thoughts, thoughtsMessage*/);
        }
    }

    public void DisplayNextSentence(Dialogue dialogue, string[] dialogueSentence/*, bool thoughts, Animator thoughtsMessage = null*/) {
        Debug.Log("in display next...");
        if (sent == lastEl && sentences.Count > 1) {
            //if (dialogue.animalSounds) dialogue.animalSounds.Play();

            sent = sentences.Dequeue();
            sentences.Clear();
            if (dialogue.taskCompleted) {
                dialogue.endDiscuss = true;
                
                helpedAnimals++;
                dialogue.minusScore = true;
                dialogue.talkShit = true;
            }
        }

        else sent = sentences.Dequeue();
        
        if (sentences.Count == 0) {
            startDiscussion = false; 
            endDiscussion = true;
            Debug.Log("END DISCUSSION BRUH");
            startDialogue = false;
            cineCamera.Follow = null;
            dialogue.interactNum++;
            return;
        }
        
        // if (thoughts) {
        //     thoughtsMessage.Play("appearMessage");
        // }
        
        var lines = sentences.ToList();

        // for (int i = 0; i<lines.Count(); i++) {
        //     Debug.Log("line" + i + ": " + lines[i]);
        // }
        //Debug.Log(sentences.Count());

        //Debug.Log(lines);
        currentLineIndex = dialogueSentence.ToArray().ToList().IndexOf(sent);
        //Debug.Log("currentIndexLine: " + currentLineIndex);

        FindObject(scene, "animalMessageDescription").gameObject.GetComponent<TextMeshProUGUI>().text = sent;
        
    }

    void EndDialogue() {Debug.Log("End of conversaton");}


    public static GameObject FindObject(GameObject parent, string name) {

        Transform[] trs= parent.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs){
            if(t.name == name){
                return t.gameObject;
            }
        }
        return null;
    }

}
