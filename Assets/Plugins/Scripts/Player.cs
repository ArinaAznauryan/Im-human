using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using System.IO;

public class Player : MonoBehaviour
{
    public string lev = "mainScene", curButton;
    public int fluffNum = 0, helpedAnimals, count = 0;
    public bool load = false;

    public List<GameObject> animals;
    public int[] animalTasks;

    private const string IsSceneOpened = "IsSceneOpened";


    void Update()
    {
        

        try {
            if (EventSystem.current.currentSelectedGameObject != null) curButton = EventSystem.current.currentSelectedGameObject.name;
            
        }
        catch (NullReferenceException e) {
            
        }
        
    }

    public void DeleteSaveFile(string saveFilePath)
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("Save file not found.");
        }
    }

    public void SaveOrLoad() {
        try {
            if(EventSystem.current.currentSelectedGameObject.name == ("continueButton") || EventSystem.current.currentSelectedGameObject.name == "startButton") {
                if (PlayerPrefs.GetInt(IsSceneOpened) == 0) {
                    PlayerPrefs.SetInt(IsSceneOpened, 1);
                    StartCoroutine(LoadSceneAsync("level1"));
                }
                else {
                    LoadPlayer();
                }
            }

            if(EventSystem.current.currentSelectedGameObject.name == "newGameButton") {
                    DeleteSaveFile(Application.persistentDataPath + "SaveFile.es3");
                    DeleteSaveFile(Application.persistentDataPath + "SaveFile.txt");
                    
                    PlayerPrefs.SetInt(IsSceneOpened, 1);
                    StartCoroutine(LoadSceneAsync("level1"));
            }
        }

        catch (NullReferenceException e){}
    }

    public void LoadPlayer() {
        var levelNum = ES3.Load<int>("level"); 
        StartCoroutine(LoadSceneAsync("level" + levelNum));
        load = true;
    }

    IEnumerator LoadSceneAsync(string levName){
        SceneManager.LoadSceneAsync(levName);
        AsyncOperation operation = SceneManager.LoadSceneAsync(levName);
        if (!GameObject.Find("Canvas").transform.Find("LoadScreen")) {
            Debug.LogError("There's no load screen in the scene, or its name is not \"LoadScreen\"");
        }
        else GameObject.Find("Canvas").transform.Find("LoadScreen").gameObject.SetActive(true);
        while (!operation.isDone) {
            yield return null;
        }
    }
}