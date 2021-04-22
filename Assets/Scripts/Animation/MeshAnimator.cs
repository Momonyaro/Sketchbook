using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Animation
{
    public class MeshAnimator : MonoBehaviour
    {
        // Så denna komponent fungerar genom att byta ut meshen vi ska använda likt hur sprites byts ut i klassisk
        // 2D animation. Alla meshes kommer att dela material (likt en spritesheet).

        [Header("General Settings")] 
        public MeshFilter meshFilter;

        public string debugAnimName = "_playerWalkRight";
        
        [Header("All animations of the character is stored here.")]
        public List<AnimDataScriptable> animationBranches = new List<AnimDataScriptable>();

        private int currentBranchIndex = 0;
        private Vector3 originalScale = Vector3.one;

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        private void Start()
        {
            StartAnimFromName(debugAnimName);
        }

        private void Update()
        {
            //Here we each frame update the active animation branch to get the next frame.
            PlayNextFrameFromActiveAnim();
        }

        public void StartAnimFromName(string animName)
        {
            for (int i = 0; i < animationBranches.Count; i++)
            {
                if (animationBranches[i].animName == animName)
                {
                    if (currentBranchIndex == i) return; //This would just reset the already playing animation.
                    currentBranchIndex = i;
                    meshFilter.mesh = animationBranches[i].StartAnim(); //Plays the anim from a beginning state.
                    return;
                }
            }
        }

        public void PlayNextFrameFromActiveAnim()
        {
            var current = animationBranches[currentBranchIndex];
            transform.localScale = current.invertAnimXAxis ? originalScale + new Vector3(-originalScale.x * 2, 0, 0): originalScale;
            meshFilter.mesh = current.TickAnimation();
        }
    }
}
