using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Animation
{
    [CreateAssetMenu(fileName = "New Animation", menuName = "Animation Data", order = 0)]
    public class AnimDataScriptable : ScriptableObject
    {
        
        [Header("General Settings")]
        public string animName = "New Animation";
        [Tooltip("This is for when you for example have a 'Walk Left' animation and want to make it 'Walk Right' by flipping the animation.")]
        public bool invertAnimXAxis = false;
        public bool loop = true;
        
        [Header("Transition Settings")]
        [Tooltip("Enabling this will cause this anim to transition into the other one after a loop.")]
        public bool transitionAfterFinish = false;
        public string leadsInto = "";
        
        [Header("Anim Data")]
        public List<AnimFrame> animFrames = new List<AnimFrame>();

        private float frameTimer = 0.0f;
        private int frameIndex = 0;
        [HideInInspector] public bool firstFrame = false;


        public Mesh StartAnim()
        {
            frameIndex = 0;
            
            frameTimer = animFrames[frameIndex].frameDuration;
            frameTimer -= Time.deltaTime;
            firstFrame = true;
            
            return animFrames[frameIndex].frameMesh;
        }

        public Mesh TickAnimation()
        {
            frameTimer -= Time.deltaTime;
            firstFrame = false;
            if (frameTimer <= 0)
            {
                if (loop)
                    frameIndex = (frameIndex + 1) % animFrames.Count; // Clamp that shit so that it loops back to 0 if it goes beyond the frame count.
                else
                    frameIndex = Mathf.Min(frameIndex + 1, animFrames.Count - 1); // Limit it to repeat the last frame
                
                //New frame, let's parse the frames audio action
                firstFrame = true;
                
                frameTimer = animFrames[frameIndex].frameDuration;
            }

            return animFrames[frameIndex].frameMesh;
        }

        public AnimFrame GetCurrentFrame()
        {
            return animFrames[frameIndex];
        }
    }

    [Serializable]
    public struct AnimFrame
    {
        public enum AudioActions
        {
            None,
            PlayerStep,
            PlayerJump,
        }

        public Mesh frameMesh;
        public float frameDuration;
        public AudioActions audioAction;

        public AnimFrame(Mesh frameMesh, float frameDuration)
        {
            this.frameMesh = frameMesh;
            this.frameDuration = frameDuration;
            this.audioAction = AudioActions.None;
        }
    }
}
