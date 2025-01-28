using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject target;
    public bool screenObject = false;
    public float speed;

    public float rotationModifier;

    void Update()
    {
        if (target) {
            if (screenObject) {

                /////////////////////////NEW VERSION///////////////////////////
                    Vector3 vectorToTarget = Camera.main.WorldToScreenPoint(target.transform.position) - transform.position; 
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier; 
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward); 
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime*speed);
                /////////////////////////NEW VERSION//////////////////////////

            //OLD VERSION_____________________
                // Vector3 _lookRotation = target.transform.position - Camera.main.ScreenToWorldPoint(transform.position);
                // transform.rotation = Quaternion.FromToRotation(Vector3.up, _lookRotation);
                // Debug.Log(transform.rotation);
                // transform.rotation = new Quaternion(0f, 0f, transform.rotation.z, transform.rotation.w);
            //OLD VERSION_____________________

            }

            else {
                float rotationDegree = Mathf.Atan2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y)*Mathf.Rad2Deg;
                Debug.Log(Mathf.Atan2((target.transform.position.x - transform.position.x), (target.transform.position.y - transform.position.y))*Mathf.Rad2Deg);
                
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, -1f*rotationDegree));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
            
            }
        }
    }
}
