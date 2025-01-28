using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    public bool taskCompleted = false, automatic = false, endDiscuss = false, minusScore = false, deleteMyself = false, talkShit = false, acceptYesNo=false;
    public int foodNum, interactNum, letterIndex;
    public GameObject foodScore, lostAnimal;
    public string nickName, foodType, letter = "sentences";
    public Vector2 messagePos;

    [TextArea]
    public string[] sentences, sentences1, sentences2, sentences3;
    //public string[][] tipLetter;

    public tipLetter[] tipLetter;

    void Start() {

     }

    

}

[System.Serializable]
public struct tipLetter
{
 	[SerializeField] [TextArea] public string[] letters;
}
