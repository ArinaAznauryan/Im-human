using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAdditiveScene : MonoBehaviour
{
    public GameObject scene;
    public bool reload = false;

    void Update()
    {
        if (reload) {
            if (!scene.activeSelf) scene.SetActive(true);
        }
    }
}
