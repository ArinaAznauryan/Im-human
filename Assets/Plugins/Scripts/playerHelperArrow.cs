using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHelperArrow : MonoBehaviour
{
    public GameObject target;

    public GameObject helperArrow, helperArrowTut;

    bool once = true;

    

    void Update()
    {
        if (target) {

            if (once) {
                helperArrowTut.SetActive(true);
                once = false;
            }
            
            helperArrow.SetActive(true);
            helperArrow.GetComponent<LookAt>().target = target;
        }

        else helperArrow.SetActive(false);
    }
}
