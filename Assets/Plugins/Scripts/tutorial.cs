using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tools.Tools;
using UnityEngine.Events;

public class tutorial : MonoBehaviour
{
    public UnityEvent[] additEvents;

    public GameObject[] targets;
    public Animator tutAnimator;
    public bool[] events;
    public bool oneTime = true, fixedArrow = false;
    Vector2 randPos;
    [SerializeField] public GameObject tap, curTarget, player;
    public Vector2 appearDistance;
    public int layerIndex, tutType = 0;

    void Start()
    {
        randPos = new Vector2(0f, 0f);

        tutAnimator.SetLayerWeight(layerIndex, 1);
    }

    void Update()
    {
        var copyTargets = targets;
        
        foreach (GameObject target in targets) {
            if (target.GetComponent<tutorialTrigger>().tutEvent == 1) {
                curTarget = target;
                break;
            }
        }
        //if (targets.Length >= 1) {
            for (int i = 0; i < targets.Length; i++) {
                if (targets[i] && targets[i].GetComponent<tutorialTrigger>()) {

                    //FindObjectOfType<playerHelperArrow>().target = availableClosestAnimals[closestIndex];

                    if (targets[i] && targets[i].GetComponent<tutorialTrigger>().tutEvent == 1) {
                    //if (oneTime) {
                        
                        if (tutType == 0) { //0 - appear near target and disappear after

                            gameObject.GetComponent<LookAt>().target = targets[i];

                            Vector2 screenTargetPos = Camera.main.WorldToScreenPoint(targets[i].transform.position);
                            transform.position = new Vector2(screenTargetPos.x+randPos.x, screenTargetPos.y+randPos.y); 

                            if (oneTime) {
                                if (!fixedArrow) {
                                    randPos = new Vector2(randomChoice(-1*appearDistance.x, appearDistance.x), randomChoice(-1*appearDistance.y, appearDistance.y));
                                }

                                else {
                                    randPos = new Vector2(appearDistance.x, appearDistance.y);
                                }
                                
                                oneTime = false;

                                if (tap) tap.SetActive(true);

                                tutAnimator.Play("appear");
                            }
                            break;
                        }

                        else if (tutType == 1) { //1 - follow the arrow to the target, disappear after
                            additEvents[0].Invoke();
                            FindObjectOfType<playerHelperArrow>().target = targets[i];
                            isFollowArrowReached();
                        }
                    }

                    else if (targets[i].GetComponent<tutorialTrigger>().tutEvent == 2) {
                        int targetIndex = i;
                        if (tutType == 0) {
                            oneTime = true;
                            tutAnimator.SetBool("disap", true);

                            if (tap) tap.SetActive(false);

                            var finish = false;

                            StartCoroutine(animFinished(tutAnimator.GetCurrentAnimatorStateInfo(0).length, animFinish => {
                                if(animFinish) {
                                    gameObject.GetComponent<LookAt>().target = null;
                                    Debug.Log("Index: " + targets[targetIndex]);
                                    
                                    finish = true;
                                }
                            }));

                            if(finish) {
                                Destroy(targets[targetIndex].GetComponent<tutorialTrigger>());
                                    RemoveAt(ref targets, targetIndex);
                                    break;
                            };
                        }

                        else if (tutType == 1) {
                            additEvents[1].Invoke();
                            
                            if (FindObjectOfType<EventFinishManager>().done) {
                                FindObjectOfType<playerHelperArrow>().target = null;
                                Destroy(targets[i].GetComponent<tutorialTrigger>());
                                RemoveAt(ref targets, i); 
                                FindObjectOfType<EventFinishManager>().done = false;
                                break;
                                
                            }
                        }
                    }

                    
                }
            }
        //}
    }

    public void isFollowArrowReached() {
        if (Vector2.Distance(player.transform.position, curTarget.transform.position) < 70f) {
            curTarget.GetComponent<tutorialTrigger>().tutEvent = 2;
            curTarget = null;
        }
    }

    public void additEventRun() {
        if (curTarget)
        curTarget.GetComponent<tutorialTrigger>().tutEvent = 1;
    }   
}
