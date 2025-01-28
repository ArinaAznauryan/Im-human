using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checkingBruh : MonoBehaviour
{
    public bool bruh = false;
    void Update()
    {
        if (bruh) SceneManager.LoadScene("nothing");
    }
}
