using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tools.Tools;
using UnityEngine.Events;

public class dieAndRevive : MonoBehaviour
{
    public UnityEvent dieEvent;
    public Animation[] anims;
    int animsFinished = 0;
    public int start = 0; //0 - havent started yet; 1 - started; 2 - finished; 3 - disabled
    public GameObject scene, player;
    public bool backward = false, play = false;
    bool once = true, onceNextFrame = false, idle = false;

    //Backward parameters
    int backwardSpeed = 1;

    void Combine() {
        scene.GetComponent<Animator>().enabled = false;
        foreach (Animation anim in anims) { // _____________ATTENTION!!!!!!_________ALL ANIMATIONS GOTTA BE THE SAME LENGTH_________________
            anim.enabled = true;
            Debug.Log("ANIM CLIP: " + anim);
            if (onceNextFrame) {
                if (!anim.isPlaying) {
                    
                    start = 2;
                    idle = true;
                    
                    onceNextFrame = false;
                }
            }
            
            if (start == 1) {
                anim.Play(); 
            }
        }
        if (once) {
            onceNextFrame = true;
            once = false;
        }
    }

    public void DisableAnimators() {
        scene.GetComponent<Animator>().enabled = true;
        foreach (Animation anim in anims) { // _____________ATTENTION!!!!!!_________ALL ANIMATIONS GOTTA BE THE SAME LENGTH_________________
            //anim.enabled = false;
        }
    }

    void EnableAnimators() {
        once = true;
        
    }


    void playAnim() {
        if (start == 1) Combine();
        else EnableAnimators();

        if (backward) {
            backwardSpeed = -1;
            start = 1;
        }
    }

    void Update() {
       

        if (play) {
            dieEvent.Invoke();
            StartCoroutine(FindObjectOfType<levelEnter>().playDie(result => {
                Debug.Log("DEATH NOTE");
                if (result) {
                    play = false;
                    
                }
            }));
        }
    }

}
