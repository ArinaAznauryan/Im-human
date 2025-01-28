using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip startClip;
    public AudioClip loopClip;
    public float endVoidSeconds = 1.2f;

    void Start()
    {
        var audio = GetComponent<AudioSource>();
        audio.clip = startClip;
        audio.Play();
        
    }

    void Update(){
        var audio = GetComponent<AudioSource>();
        if (audio.clip==startClip && !audio.isPlaying) {
            audio.clip = loopClip;
            GetComponent<AudioSource>().loop = true;
            audio.Play();
        }

    }

}
