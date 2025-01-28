using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class itemScore : MonoBehaviour
{
    public int score;
    void Start()
    {
        
    }

    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
