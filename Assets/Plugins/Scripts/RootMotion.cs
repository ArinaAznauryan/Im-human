using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : MonoBehaviour {
    public bool enableRootMotion;
    private Animator animator;

    private void OnAnimatorMove()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        if (enableRootMotion)
        {
            animator.ApplyBuiltinRootMotion();
        }
    }
}
