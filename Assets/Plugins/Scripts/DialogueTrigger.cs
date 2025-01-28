using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject animal;
    //public bool thoughts;
    public Animator thoughtsMessage;
    public AudioSource animalSounds;

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, animal/*, thoughts, thoughtsMessage*/);
    }
}
