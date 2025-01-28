using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class settings : MonoBehaviour
{
    public int levelNum;
    public int minScore;
    public int helpedAnimals;
    public Sprite[] ghostState;
    public bool isAnimFinished = false, playOnce = true;


    void Awake() {
        ES3.Save("level", levelNum);
    }

    public void RemoveAt<T>(ref T[] arr, int index) {
        arr[index] = arr[arr.Length - 1];
        Array.Resize(ref arr, arr.Length - 1);
    }

    IEnumerator animFinished(float delay, System.Action<bool> callback) {
        yield return new WaitForSeconds(delay);
        callback(true);
    }

}
