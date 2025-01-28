using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleAppearDisappearAnim : MonoBehaviour
{
    public UnityEventBool OnAppearCallback;


    float alpha = 0;
    bool once = true;
    public float speed;
    public bool appear = false, disappear = false;
    //_________PUT THE SCRIPT ON THE TARGET OBJECT!!!_____________
    void Update() {
        
        // if (appear) Appear(speed);
        // if (disappear) Disappear(speed);
    }

    public void Appear(float speed) {
        var spriteColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, speed);

        if (spriteColor.a >= 1) {
            OnAppearCallback.Invoke(false);
            appear = false;
        }
    }

    public void Disappear(float speed) {
        var spriteColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, speed);

        if (spriteColor.a <= 0) {
            OnAppearCallback.Invoke(true);
            disappear = false;
            //return true;
        }
        //return false;
    }
}
