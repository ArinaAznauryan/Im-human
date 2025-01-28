using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Tools.Tools;

public class ChapterManager : MonoBehaviour
{
    public UnityEvent additEvent;

    void Update()
    {
        if (FindObjectOfType<ghostRealisationScene>()) {
            if (!FindObjectOfType<ghostRealisationScene>().isActiveAndEnabled) 
                gameObject.GetComponent<Animator>().enabled = true;
        }  

        else gameObject.GetComponent<Animator>().enabled = true; 

        StartCoroutine(animFinished(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, animFinish => {
            if(animFinish) {
                additEvent.Invoke();
            }
        }));
    }
}
