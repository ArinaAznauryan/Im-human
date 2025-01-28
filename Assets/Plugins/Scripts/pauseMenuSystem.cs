using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;

public class pauseMenuSystem : MonoBehaviour
{
    //public PostProcessVolume volume;
    GameObject pauseMenu;

    void Start(){
        //volume = GetComponent<PostProcessVolume>();
    }
    //SceneManager.LoadScene()
    void StartPauseMenu(string button) {
        switch (button) {
            case "pause":
                //volume.profile.GetSetting<DepthOfField>().enabled.value = true;
                pauseMenu.SetActive(true);
                break;
            
            case "home":
                //volume.profile.GetSetting<DepthOfField>().enabled.value = false;
                pauseMenu.SetActive(false);
                SceneManager.LoadScene("mainScene");
                break;
            
            case "resume":
                //volume.profile.GetSetting<DepthOfField>().enabled.value = false;
                pauseMenu.SetActive(false);
                break;
            
            default: break;
        }
    }


    void Update()
    {
        
    }
}
