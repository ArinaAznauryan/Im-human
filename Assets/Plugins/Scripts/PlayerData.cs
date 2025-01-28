using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public string lev;
    public int fluffNum, helpedAnimals, count;
    public int[] animalTasks;
    public PlayerData(Player player) { 
        lev = player.lev; 
        fluffNum = player.fluffNum;
        helpedAnimals = player.helpedAnimals;

        animalTasks = new int[player.animalTasks.Length];

        for (int i = 0; i < animalTasks.Length; i++) {
            animalTasks[i] = 0;
        }

        for (int i = 0; i < player.animalTasks.Length; i++) {
            animalTasks[i] = player.animalTasks[i];
        }
    }
}
