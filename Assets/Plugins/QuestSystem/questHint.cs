using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using static Tools.Tools;

public class questHint : MonoBehaviour
{
    private AudioSource hintAudio;
    private TextMeshProUGUI hintText;
    private Animator hintAnim;
    private Transform hintPoint;
    private bool runHint = false, once = true;

    private string curDescription;

    void Awake() {
        hintAudio = transform.Find("questHintSound").GetComponent<AudioSource>();
        hintText = transform.Find("questHintText").GetComponent<TextMeshProUGUI>();
        hintAnim = gameObject.GetComponent<Animator>();
        hintPoint = GameObject.FindWithTag("hintDispencer").transform;
    }

    public void PlayHint(string description) {
        curDescription = "";
        runHint = true;
        once = true;
        curDescription = description;
       // hintAnim.SetFloat("startTime", 0f);
        hintAnim.Play("idle");
        
        Debug.Log("BELLYACHE");
    }

    private void Update() {
        if (runHint) {
            //var camera = FindObjectOfType<CinemachineVirtualCamera>();
            
            hintAnim.Play("run");
            hintText.text = curDescription;
            
            transform.position = Camera.main.WorldToScreenPoint(hintPoint.position);

            StartCoroutine(animFinished(hintAnim.GetCurrentAnimatorStateInfo(0).length, animFinish => {
                if(animFinish) {
                    //hintText.text = "";
                    runHint = false;   
                }
            }));
        }
    }
}
