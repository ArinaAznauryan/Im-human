using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chapterAnimator : MonoBehaviour
{

    void Update()
    {
        if (!FindObjectOfType<ghostRealisationScene>().isActiveAndEnabled) gameObject.GetComponent<Animator>().enabled = true;
    }
}
