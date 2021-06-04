using System;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
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

        public string debugAnimName = "";
        
        private bool _hasWalkEmitter = false;
        public StudioEventEmitter WalkEmitter = null;
        private bool _hasJumpEmitter = false;
        public StudioEventEmitter JumpEmitter = null;
        
        
        [Header("All animations of the character is stored here.")]
        public List<AnimDataScriptable> animationBranches = new List<AnimDataScriptable>();

        private int currentBranchIndex = 0;
        private Vector3 originalScale = Vector3.one;
        public bool invertXScale = false;

        private void Awake()
        {
            if (WalkEmitter != null)
                _hasWalkEmitter = true;
            if (JumpEmitter != null)
                _hasJumpEmitter = true;
            
            originalScale = transform.localScale;
            CreateInternalCopies();
        }

        private void Start()
        {
            StartAnimFromName(debugAnimName, false);
        }

        private void Update()
        {
            //Here we each frame update the active animation branch to get the next frame.
            PlayNextFrameFromActiveAnim();
        }

        public void StartAnimFromName(string animName, bool walkLeft)
        {
            invertXScale = walkLeft;
            for (int i = 0; i < animationBranches.Count; i++)
            {
                if (animationBranches[i].animName == animName)
                {
                    if (currentBranchIndex == i) return; //This would just reset the already playing animation.
                    currentBranchIndex = i;
                    meshFilter.mesh = animationBranches[i].StartAnim(); //Plays the anim from a beginning state.
                    ParseAudioAction(animationBranches[i]);
                    return;
                }
            }
        }

        public void PlayNextFrameFromActiveAnim()
        {
            var current = animationBranches[currentBranchIndex];
            transform.localScale = invertXScale ? originalScale + new Vector3(-originalScale.x * 2, 0, 0): originalScale;
            meshFilter.mesh = current.TickAnimation();
            if (current.firstFrame)
            {
                current.firstFrame = false;
                ParseAudioAction(current);
            }
        }

        private void CreateInternalCopies()
        {
            var copies = new AnimDataScriptable[animationBranches.Count];
            for (int i = 0; i < copies.Length; i++)
            {
                copies[i] = (AnimDataScriptable) ScriptableObject.CreateInstance(typeof(AnimDataScriptable));
                copies[i].loop = animationBranches[i].loop;
                copies[i].animFrames = animationBranches[i].animFrames;
                copies[i].animName = animationBranches[i].animName;
                copies[i].leadsInto = animationBranches[i].leadsInto;
                copies[i].transitionAfterFinish = animationBranches[i].transitionAfterFinish;
            }
            
            animationBranches = new List<AnimDataScriptable>(copies);
        }

        private void ParseAudioAction(AnimDataScriptable current)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////
            //    Plan of attack: Fetch the wanted clip from the AudioManager, set it as the clip to play &    //
            //                 finally signalling the audio source to play it's sound.                         //
            /////////////////////////////////////////////////////////////////////////////////////////////////////
            
            if (!_hasWalkEmitter) return;
            if (!_hasJumpEmitter) return;
            
            AnimFrame.AudioActions action = current.GetCurrentFrame().audioAction;
            string path = "";
            switch (action)
            {
                case AnimFrame.AudioActions.PlayerStep: // event:/SFX/Player/Player_Walking
                    if (AudioManager.events["event:/SFX/Player/Player_Walking"].getPath(out path) == RESULT.OK)
                    {
                        WalkEmitter.Event = path;
                        WalkEmitter.Play();
                    }
                    return;
                
                case AnimFrame.AudioActions.PlayerJump: // event:/SFX/Player/Player_Jump
                    if (AudioManager.events["event:/SFX/Player/Player_Jump"].getPath(out path) == RESULT.OK)
                    {
                        JumpEmitter.Event = path;
                        JumpEmitter.Play();
                    }
                    return;
                
                case AnimFrame.AudioActions.RoachWalk: // event:/SFX/Enemy/Scarface (cockroach)/Enemy_Scarface_Move_Walk
                    if (AudioManager.events["event:/SFX/Enemy/Scarface (cockroach)/Enemy_Scarface_Move_Walk"].getPath(out path) == RESULT.OK)
                    {
                        WalkEmitter.Event = path;
                        WalkEmitter.Play();
                    }
                    return;
                
                case AnimFrame.AudioActions.RoachFly: // event:/SFX/Enemy/Scarface (cockroach)/Enemy_Scarface_Move_Fly
                    if (AudioManager.events["event:/SFX/Enemy/Scarface (cockroach)/Enemy_Scarface_Move_Fly"].getPath(out path) == RESULT.OK)
                    {
                        WalkEmitter.Event = path;
                        WalkEmitter.Play();
                    }
                    return;
                
                case AnimFrame.AudioActions.PostWalk: // event:/SFX/Enemy/PostMord/Enemy_Postmord_Walk
                    if (AudioManager.events["event:/SFX/Enemy/PostMord/Enemy_Postmord_Walk"].getPath(out path) == RESULT.OK)
                    {
                        WalkEmitter.Event = path;
                        WalkEmitter.Play();
                    }
                    return;
                
                default:
                    return;
            }
        }

        public Mesh GetCurrentPlayingMesh()
        {
            return meshFilter.mesh;
        }

        public Vector3 GetCurrentAnimScale()
        {
            return transform.localScale;
        }
    }
}
