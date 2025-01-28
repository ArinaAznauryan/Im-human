using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Events;

public class levelEnter : MonoBehaviour
{
    public CinemachineVirtualCamera cineCamera;

    public GameObject player, fluff, camera, door, scene;
    public GameObject levelUI;
    public bool triggered = false, loadAdditiveScene = false, hit = false, isComingSoon = false, playOnce = true, saveScene = false, runFluffWarning = false, allTasksDone = false, runLevelAppear = false;
    public GameObject comingSoon;
    public LayerMask doorMask;
    public float distance = 5, cameraSpeed;
    public Vector2 dir;
    string animName = "levelAppearTransition";
    public int scoreMin, level;
    bool loadOnce = true;

    public bool onceDie = false;

    public UnityEvent additEvent;


    IEnumerator animFinished(float delay, System.Action<bool> callback) {
        yield return new WaitForSeconds(delay);
        callback(true);
    }

    void Update()
    {
        scene.GetComponent<Animator>().Play(animName);
        dir = player.GetComponent<playerMovement>().dir;
        hit = Physics2D.Raycast(player.transform.position, dir, distance, doorMask);

        if (level == 1) {
            if(playOnce) {
                animName = "levelAppearTransition";
                playOnce = false;
            }


            if (hit) triggered = true;
            else runFluffWarning = false;

            if (hit && player.GetComponent<fluffSystem>().score >= scoreMin) {
                runFluffWarning = false;
                GameEventsManager.instance.inputEvents.touchControls.Disable();
                cineCamera.Follow = door.transform;

                animName = "levelTransition";
                
                StartCoroutine(animFinished(scene.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
                    if(animFinish) {
                        SceneManager.LoadScene("level" + (level+1).ToString());
                        cineCamera.Follow = null;
                    }
                }));
            }
            else if (hit && player.GetComponent<fluffSystem>().score < scoreMin){
                runFluffWarning = true;
            }
            if (FindObjectOfType<fluffSystem>().runTut) {
                
                animName = "fluffTutAppear";
                FindObjectOfType<fluffSystem>().runTut = false;
            }
            if (runFluffWarning) {
                animName = "fluffWarnAppear";

                GameEventsManager.instance.inputEvents.touchControls.Enable();
            }
            
        }
        if (level != 1) {
            
            bool reload = FindObjectOfType<ReloadAdditiveScene>() ? FindObjectOfType<ReloadAdditiveScene>().reload : false;
            
            if (loadOnce && !allTasksDone && !reload) {
                animName = "levelAppearTransition";
                loadOnce = false;
            }

            if (animName == "levelAppearTransition") {
                StartCoroutine(animFinished(scene.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
                    if(animFinish) {
                        additEvent.Invoke();
                    }
                }));
            }

            else if (allTasksDone && reload) {
                animName = "cameraSuckOut";
                FindObjectOfType<ReloadAdditiveScene>().reload = false;
            }
            
            if (allTasksDone) {
                var darkGhost = FindObjectOfType<darkGhostManager>();
                if (hit) {
                    cineCamera.Follow = door.transform;
                    
                    FindObjectOfType<InputManager>().touchControls.Disable();
                    
                    string futureLevel;

                    if (isComingSoon) futureLevel = "comingSoon";
                    else futureLevel = "level" + (level+1).ToString();
                    

                    TransitionLevel(futureLevel, "levelTransition");
                }

                else if (FindObjectOfType<darkGhostManager>() && FindObjectOfType<darkGhostManager>().startAgain) {
                    FindObjectOfType<InputManager>().touchControls.Disable();

                    TransitionLevel("level1", "startAgain", false, true, player.GetComponent<Animator>(), "lastConfusion");
                    
                }

                else if (darkGhost && darkGhost.startChallenge && !reload) {
                    FindObjectOfType<darkGhostManager>().startChallenge = false;
                    FindObjectOfType<InputManager>().touchControls.Disable();

                    if (playOnce) TransitionLevel("puzzleScene", "levelTransition", true);

                }

                else if (loadAdditiveScene && !reload) {
                    FindObjectOfType<darkGhostManager>().startChallenge = false;
                    FindObjectOfType<InputManager>().touchControls.Disable();

                    if (playOnce) TransitionLevel("puzzleScene", "levelTransition", true);

                }

                else {
                    FindObjectOfType<InputManager>().touchControls.Enable();
                }
            }
        }
    }

    public void TransitionLevel(string sceneName, string curAnimName, bool additive = false, bool preAnim = false, Animator preAnimator = null, string lastAnim = null) {
        animName = curAnimName;

        Animator curAnimator;

        if (!preAnim) {
            curAnimator = scene.GetComponent<Animator>();
        }

        else {
            curAnimator = preAnimator;
        }

        StartCoroutine(animFinished(curAnimator.GetCurrentAnimatorStateInfo(0).length, animFinish => {
            if(animFinish) {
                if (preAnim) {
                    if (lastAnim.Length > 0 && curAnimator.GetCurrentAnimatorStateInfo(0).IsName(lastAnim)) {
                        cineCamera.Follow = null;

                        if (!additive) SceneManager.LoadScene(sceneName);
                        else {
                            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                            scene.SetActive(false);
                            playOnce = false;
                        }
                    }
                }
                
                else {
                    cineCamera.Follow = null;

                    if (!additive) SceneManager.LoadScene(sceneName);
                    else {
                        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                        scene.SetActive(false);
                        playOnce = false;
                    }
                }
            }
        }));
    }

    public IEnumerator playDie(System.Action<bool> callback) {
        GameEventsManager.instance.inputEvents.touchControls.Disable();
        Animator curAnimator = scene.GetComponent<Animator>();
        animName = "ghostDie";

        if (curAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ghostDieRespawn") {
            yield return StartCoroutine(animFinished(curAnimator.GetCurrentAnimatorStateInfo(0).length, animFinish => {
                if (animFinish) {
                    animName = "idle";
                    GameEventsManager.instance.inputEvents.touchControls.Enable();
                    callback(true);
                }
            }));
        } 
        else callback(false);
    }
}
