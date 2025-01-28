using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependentActivity : MonoBehaviour
{
    public GameObject[] targets;
    public GameObject target, origin;

    public void Initialize(GameObject[] coTargets) {
        targets = coTargets;
    }
    public void Initialize(GameObject coTarget) {
        target = coTarget;
    }

    void Update() {
        if (targets.Length > 0) {
            foreach (GameObject tar in targets) {
                tar.SetActive(origin.activeSelf);
            }
        }
        else target.SetActive(origin.activeSelf);
    }
}
