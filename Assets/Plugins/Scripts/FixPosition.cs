using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPosition : MonoBehaviour
{
    public GameObject target;

    public Camera cam;
    Vector3 pos = new Vector3(200, 200, 0);

    void Start() {
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(FindObjectOfType<InputManager>().worldFollower.transform.position), Vector2.zero);
 
        if(hit.collider != null)
        {
            Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position + " " + hit.collider.gameObject.name);
        }
        transform.position = target.transform.position;
    }
}
