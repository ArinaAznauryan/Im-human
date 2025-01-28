using System.Collections;
 using UnityEngine;
 
 public class animationFlow : MonoBehaviour
 {
     public Animator animator;
     private AnimationClip[] clips;
 
     private void Start()
     {
         if (!this.GetAnimator())
         {
             Debug.LogWarning("Cannot find Animator compoment in " + this.gameObject.name + " !! Be sure to attach this script to a GO with Animator and Animator Controller or set it manually");
             return;
         }
         if (!this.GetClips())
         {
             Debug.LogWarning("No Animator Controller or clips found in " + this.gameObject.name);
             return;
         }
         //you can call this also extrenally if you dont want to play the clips at start.
         this.PlayClips();
     }
 
     private bool GetAnimator() => this.animator != null || (this.animator = base.gameObject.GetComponentInChildren<Animator>()) != null;
 
     private bool GetClips() => !(this.animator.runtimeAnimatorController == null || (this.clips = this.animator.runtimeAnimatorController.animationClips) == null || this.clips.Length == 0);
 
     public void PlayClips()
     {
         float cumulativeLength=0;
         foreach(AnimationClip clip in this.clips)
         {
             StartCoroutine(this.PlayClip(Animator.StringToHash(clip.name), cumulativeLength));            
             cumulativeLength += clip.length;
         }
     }
 
     IEnumerator PlayClip(int clipHash, float startTime)
     {        
         yield return new WaitForSeconds(startTime);
         // Remeber to set you Animator Animation state name same as the Clip name! 
         this.animator.Play(clipHash);              
     }
 }