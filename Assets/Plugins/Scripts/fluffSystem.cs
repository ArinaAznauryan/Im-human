using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using static Tools.Tools;

public class fluffSystem : MonoBehaviour
{    
    public TMP_Text fluffScore;
    public GameObject fluff, player, levelEnterObject;
    public Animator fluffTaskAnim;
    public int fluffNum = 0;
    public GameObject fluffTutText, fluffTerritory, fluffSpawn, fluffWarningText, scene;
    public int score = 0, isRunning = 1, fluffAppearWaitSpeed, fluffDisappearWaitSpeed, level;
    Vector3 newPos;
    public bool addScore = false, fluffTextDisappear = false, runTut = false, fluffRunning = false, firstFluff = false;
    public Tilemap floor;
    public int colorAlpha = 0, colorAlpha1 = 0, colorA = 0, tutEvent;

    void Start()
    {
        newPos = transform.position;

    }
    void Update()
    {

        if (fluffTaskAnim) {

            task1(fluffTaskAnim);
        }

        if (fluffNum == 1) {
            firstFluff = true;
        }
        else firstFluff = false;

        if(level==1) {
            if (firstFluff) {
                runTut = true;
            }
        
        }

        fluffScore.text = score.ToString();
        if (GameObject.Find("tutorial") && fluff.GetComponent<tutorialTrigger>()) {
            var terCollider = fluffTerritory.GetComponent<itemGrabTrigger>();
            if (terCollider.triggered) {
                Debug.Log("WHAT DO YOU WANT FROM ME");
                StartCycle();
            }
        }

        else StartCycle();

        if(Vector3.Distance(fluff.transform.position, player.transform.position) < 5f && fluff.activeSelf) {
            if(addScore) {
                fluffNum++;
                if (fluffNum == 1) {
                    isRunning = 1;
                } 
                if (fluff.GetComponent<tutorialTrigger>()) fluff.GetComponent<tutorialTrigger>().tutEvent = 2;
                fluff.SetActive(false);

                if (!fluffScore.gameObject.activeSelf) fluffScore.gameObject.SetActive(true);
                score+=10;
                addScore = false;
            }
        }

        fluff.transform.position = new Vector3(fluff.transform.position.x, fluff.transform.position.y, 0.002128601f);
    }

    void StartCycle() {
        if (isRunning == 1) {
            Debug.Log("STARTING CYCLE");
            StartCoroutine("Sleep"); 
            isRunning = 0;
        }
    }
    IEnumerator Sleep(){
        Debug.Log("SLEEPING");
        isRunning = 1;
        
        if (fluffSpawn.transform.position.x != fluff.transform.position.x && fluffSpawn.transform.position.y != fluff.transform.position.y) {
            if (fluffNum < 1) {
                if (fluff.GetComponent<tutorialTrigger>()) {
                    fluff.GetComponent<tutorialTrigger>().tutEvent = 1;
                    fluffAppearWaitSpeed = 10000;
                }
            }

            else {
                if (fluff.GetComponent<tutorialTrigger>()) {
                    fluff.GetComponent<tutorialTrigger>().tutEvent = 1;
                    fluffAppearWaitSpeed = 4;
                }
                
            }
        }
        

        yield return new WaitForSeconds(fluffAppearWaitSpeed);
        if (fluff.GetComponent<tutorialTrigger>()) fluff.GetComponent<tutorialTrigger>().tutEvent = 2;
        fluff.SetActive(false);
        
        
        yield return new WaitForSeconds(fluffDisappearWaitSpeed);
        
        appearInTerritory();
        fluff.SetActive(true);
        addScore = true;
        isRunning = 1;
        yield break;
    }


    void LateUpdate(){
        fluff.transform.position = new Vector3(fluff.transform.position.x, fluff.transform.position.y, 0.002128601f);
    }


    void task1(Animator anim) {
        if (score >= 100) {
            anim.Play("notify");
        }
    }

    void appearInTerritory() {
        Vector2 result = new Vector2(transform.position.x+Random.Range(-20f, 20f), transform.position.y+Random.Range(-20f, 20f));
        fluff.transform.position = result;
    }
}