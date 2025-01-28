using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Tilemaps;
using static Tools.Tools;
using UnityEngine.SceneManagement;


public class itemGrab : MonoBehaviour
{

    [System.Serializable]
    public struct inventoryItems
    {
        [SerializeField] public string type;
        [SerializeField] public int amount;
    }

    [System.Serializable]
    public struct OtherGroups
    {
        [SerializeField] public GameObject[] group;
    }

    public string[] inventItemTypes;
    public Tilemap natureTilemap;
    public Tile bigTreeTile;
    public List<GameObject> items;
    public List<GameObject> wantedAnimalGroup;
    public GameObject[] acornGroups, appleGroups, hintGroup;
    public GameObject mouse, player, highlighter, scene, score;
    public OtherGroups[] otherGroups;
    public inventoryItems[] inventory;
    public Vector2 worldMousePos;
    List<float> itemDistances;
    int closestItemIndex, foodJ = 0, animalJ = 0, distanceAdder, acornNumber, appleNumber;
    public bool[] itemsActive;
    public bool save = false;
    string itemGrabActivesFile;

    void Start()
    {

        if (acornGroups.Length > 0) {
            foreach (GameObject group in acornGroups) {
                acornNumber += group.transform.childCount;
            }
        }

        if (appleGroups.Length > 0) {
            foreach (GameObject group in appleGroups) {
                appleNumber += group.transform.childCount;
            }
        }

        items = new List<GameObject>();
        
        foreach (GameObject group in acornGroups) {
                for (int j = 0; j < group.transform.childCount; j++) {
                    items.Add(group.transform.GetChild(j).gameObject);
                }
        }

        if (otherGroups.Length > 0) {
            foreach (OtherGroups group in otherGroups) {
                foreach (GameObject subGroup in group.group) {
                    items.Add(subGroup);
                }
            }
        }

        if (appleGroups.Length > 0) {
            foreach (GameObject group in appleGroups) {
                    for (int j = 0; j < group.transform.childCount; j++) {
                        items.Add(group.transform.GetChild(j).gameObject);
                    }
            }
        }

        if (hintGroup.Length > 0) {
            foreach (GameObject group in hintGroup) {
                        items.Add(group);
            }
        }

        if (save) {
            itemGrabActivesFile = "itemGrabActives"+SceneManager.GetActiveScene().name;

            if (ES3.KeyExists(itemGrabActivesFile)) {
                itemsActive = ES3.Load<bool[]>(itemGrabActivesFile);
                for (int i = 0; i<items.Count; i++) {
                    items[i].SetActive(itemsActive[i]);
                }
            }
            else itemsActive = new bool[items.Count];
        }
        
        else itemsActive = new bool[items.Count];
    }

    void Update() {
        fillInventory();
    }

    public int GetItemNumber(string tarItem) {
        foreach (inventoryItems item in inventory) {
            if (item.type == tarItem) return item.amount;
        }
        return 0;
    }

    void LateUpdate() {
       worldMousePos = Camera.main.ScreenToWorldPoint(new Vector2(GameEventsManager.instance.inputEvents.joyMovePos.x, GameEventsManager.instance.inputEvents.joyMovePos.y));
        

        //_____________WANTED ANIMALS____________________________________//
        for (int i = 0; i<wantedAnimalGroup.Count; i++) {
            if (wantedAnimalGroup[i].activeSelf) {
                var wantedAnimalTrigger = wantedAnimalGroup[i].GetComponent<itemGrabTrigger>();
                var wantedAnimalAdditionalEvent = wantedAnimalGroup[i].GetComponent<wantedAnimalTrigger>().additionalEvent;
                
                if (wantedAnimalTrigger.OnClick() || wantedAnimalAdditionalEvent == 3) {
                    animalJ = i;
                    
                    addAnimalScore(animalJ);
                    break;
                }
            }
        }
        //_____________WANTED ANIMALS____________________________________//

        //_____________INVENTORY____________________________________//
        for (int i = 0; i<items.Count; i++) {
            if (items[i].activeSelf) {
                var itemTrigger = items[i].GetComponent<itemGrabTrigger>();
                
                if (itemTrigger.OnClick() || itemTrigger.additEvent) {
                    items[i].GetComponent<itemGrabTrigger>().additEvent = false;
                    foodJ = i;
                    
                    addInventoryScore(foodJ);
                }
            }   
        }
    }

    void fillInventory() {
        for (int i = 0; i < inventItemTypes.Length; i++) {
            var groupName = inventItemTypes[i]+"Group";
            
            var score = FindObject(scene, groupName);

            if (score) {
                inventory[i] = new inventoryItems();
                inventory[i].type = inventItemTypes[i];
                inventory[i].amount = score.transform.GetChild(0).gameObject.GetComponent<itemScore>().score;
            }

        }
    }

    void addInventoryScore(int index, bool playOnce = true) {
        if (playOnce) {
            var groupName = items[index].tag+"Group";
            score = FindObject(scene, groupName);

            if (!score.activeSelf) score.SetActive(true);

            score.transform.GetChild(0).gameObject.GetComponent<itemScore>().score+=items[index].GetComponent<itemGrabTrigger>().scoreAdder;
            items[index].SetActive(false);

            for (int i = 0; i < items.Count; i++) {
                itemsActive[i] = items[i].activeSelf;
            }
            if (save) ES3.Save(itemGrabActivesFile, itemsActive);

            playOnce = false;
        }
    }

    void addAnimalScore(int index, bool playOnce = true) {
        if (playOnce) {
            FindObject(scene, wantedAnimalGroup[index].tag+"Group").SetActive(true);
            score = FindObject(scene, wantedAnimalGroup[index].tag+"Group")/*.GetComponent<TextMeshPro>()*/;
            score.transform.GetChild(0).gameObject.GetComponent<itemScore>().score+=wantedAnimalGroup[index].GetComponent<itemGrabTrigger>().scoreAdder;
            score.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = score.transform.GetChild(0).gameObject.GetComponent<itemScore>().score.ToString();
             
            playOnce = false;
            wantedAnimalGroup.RemoveAt(index);
        }
    }

    void addFoodScore(int index, bool playOnce = true) {
        if (playOnce) {
            var groupName = items[index].tag+"Group";
            

            score = FindObject(scene, groupName);
            score.SetActive(true);

            score.transform.GetChild(0).gameObject.GetComponent<itemScore>().score+=items[index].GetComponent<itemGrabTrigger>().scoreAdder;
            items[index].SetActive(false);

            
                for (int i = 0; i < items.Count; i++) {
                    itemsActive[i] = items[i].activeSelf;
                    
                }
                if (save) ES3.Save(itemGrabActivesFile, itemsActive);
            

            playOnce = false;
        }
    }

}
