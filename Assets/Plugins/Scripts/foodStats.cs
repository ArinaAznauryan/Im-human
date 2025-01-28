using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tools.Tools;

public class foodStats : MonoBehaviour
{
    public GameObject scene;
    public Food[] food;

    string[] foodTypes = {"acorn", "apple"};

    void Start() {
        food = new Food[foodTypes.Length];
    }

    void Update() { 
        
        fillFood();
    }

    public void fillFood() {
        for (int i = 0; i < foodTypes.Length; i++) {
            var groupName = foodTypes[i]+"Group";
            
            var score = FindObject(scene, groupName);

            if (score) {
                food[i] = new Food();
                food[i].type = foodTypes[i];
                food[i].amount = score.transform.GetChild(0).gameObject.GetComponent<itemScore>().score;
            }
        }
    }

    public bool Has(string foodType) {
        if (food.Length > 0) {
            for (int i = 0; i < food.Length+1; i++) {
                Debug.Log("foodlength: " + food.Length + " fuck you foodType: " + foodType + " food types: " + food[i].type);
                if (foodType == food[i].type) {
                    Debug.Log("bro it HAS the apple type");
                    return true;
                }
                Debug.Log("bro it DOES NOT HAVE the apple type");
            }
            return false;
        }
        Debug.Log("tf am I even doing here");
        return false;
    }

    public Food GetFood(string foodType) {
        if (Has(foodType)) {
            for (int i = 0; i < food.Length+1; i++) {
                Debug.Log("wantedType: " + foodType + " type: " + food[i].type + " amount: " + food[i].amount);
                if (foodType == food[i].type) {
                    Debug.Log("apple bottom jeans");
                    return food[i];
                }
            }
            return new Food();
        }
        Debug.LogError("Player has no such food like '" + foodType + "'!");
        return new Food();
        
    }
}
