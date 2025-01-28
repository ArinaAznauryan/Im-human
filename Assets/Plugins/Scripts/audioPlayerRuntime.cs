using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayerRuntime : MonoBehaviour
{
    public AudioSource audio;
    public bool play = false;

    void Update() {
        if (play) {
            audio.Play();
            play = false;
        }
    }
}
