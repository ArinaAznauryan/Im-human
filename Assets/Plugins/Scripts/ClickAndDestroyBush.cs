using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDestroyBush : MonoBehaviour
{
    public GameObject target, targetCollider, eventTarget;
    public GameObject[] subEffects;
    public bool tapped, start = false, autoSave = false;
    
    public int tapNum;

    public string mode;

    void Start()
    {
        if (!targetCollider.GetComponent<BoxCollider2D>() || !targetCollider.GetComponent<itemGrabTrigger>() || !target.GetComponent<Animator>()) {
            Debug.LogError("Collider2D or ItemGrabTrigger or Animator is not attached to the object for it to be destroyed when you click it, biach: " + target.name);
        }

        if (autoSave) {
            if (ES3.KeyExists("bushDestroyed" + target.GetInstanceID())) {
                //ES3.LoadInto("itemGrabActives", this.transform);
                if (ES3.Load<bool>("bushDestroyed" + target.GetInstanceID())) Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (eventTarget.GetComponent<itemGrabTrigger>().startToTrigger) {
            eventTarget.GetComponent<itemGrabTrigger>().startToTrigger = false;

            foreach (GameObject subEffect in subEffects) {
                subEffect.SetActive(true);
            }
            
            start = true;
            Debug.Log("start is true, and bunny is deactivated");
        }
            
        if (start) {    
            Debug.Log("in start condition");

            var bushAnimator = target.GetComponent<Animator>();
            
            tapped = GameEventsManager.instance.inputEvents.tapped;
        
            bool animFinished = bushAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > bushAnimator.GetComponent<Animation>().clip.length;

            if (targetCollider.GetComponent<itemGrabTrigger>().triggered && tapped) {
                tapNum++;
                bushAnimator.Play("bushHit");

                
                if (tapNum > Random.Range(3, 6)) {
                    eventTarget.GetComponent<itemGrabTrigger>().startToTrigger = true;
                    Destroy(gameObject);

                    if (autoSave) ES3.Save("bushDestroyed" + target.GetInstanceID(), true);
                }
            }
            else {
                if (animFinished) bushAnimator.Play("idle");
            }
        }
    }
}
