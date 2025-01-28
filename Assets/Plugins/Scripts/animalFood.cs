using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class animalFood: MonoBehaviour
{
    public Food food;
    public int foodAmountGoal;
    public GameObject[] foodTrees;

    public void EnableTrees() {
        foreach (GameObject foodTree in foodTrees) {
            for (int i = 0; i < foodTree.transform.childCount; i++) {
                foodTree.transform.GetChild(i).gameObject.GetComponent<itemGrabTrigger>().startToTrigger = true;
            }
        }
    }

    void Update() { 
        food.amount = FindObjectOfType<foodStats>().GetFood(food.type).amount;
    }
}
