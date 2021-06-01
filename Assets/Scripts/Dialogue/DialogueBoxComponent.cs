﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueBoxComponent : DialogueComponent
    {
        public List<DialogueBox> dialogueBoxes = new List<DialogueBox>();
        private int currentIndex = 0;
        private int currentChar = 0;
        private float buildTimer = 0;
        private bool canBuild = false;
        private bool skipBuild = false;
        private bool buildingText = false;

        private DialogueScriptable dialogueScriptable;
        
        public override void Init(DialogueScriptable parent, out GameObject componentPrefab)
        {
            errorAttempts = 99;
            currentIndex = 0;
            canBuild = true;
            SetNewInstance(null); // Nullify instance so that we don't have any ghosts spooking the code.

            dialogueScriptable = parent;
            
            componentPrefab = objectPrefab;
        }

        public override void Update(out bool endOfLife)
        {
            //Check if the instance is null before doing anything

            if (AtEndOfLife()) { endOfLife = true; return; }
            
            SlowWriteToReciever();
            
            endOfLife = false;
        }

        public override void OnSubmitInput(InputAction.CallbackContext context)
        {
            // Used for skipping the string building and for advancing to the next text block until endOfLife
            
            if (buildingText) // This means to skip the building and just write it out.
            {
                skipBuild = true;
            }
            else // Advance the currentIndex and flag buildable
            {
                currentIndex++;
                currentIndex = Mathf.Clamp(currentIndex, 0, dialogueBoxes.Count);
                canBuild = true;
                
                Debug.Log("Dialogue Box Component with ref: " + reference + " updated currentIndex to: " + currentIndex);
                
                if (IsNull()) return;
                DialogueBoxReciever reciever = GetCurrentInstance().GetComponent<DialogueBoxReciever>();
                if (reciever.continuePrompt != null)
                    reciever.continuePrompt.SetActive(false);
            }
        }

        private void SlowWriteToReciever()
        {
            if (IsNull()) return;
            if (dialogueBoxes[currentIndex].buildTime == 0)
                skipBuild = true;
            
            DialogueBoxReciever reciever = GetCurrentInstance().GetComponent<DialogueBoxReciever>();
            DialogueBox current = dialogueBoxes[currentIndex];
            Speaker currentSpeaker = GetCurrentSpeaker();
            if (reciever.speakerPhoto != null)
                reciever.speakerPhoto.sprite = currentSpeaker.speakerPhoto;
            if (reciever.speakerName != null)
                reciever.speakerName.text = currentSpeaker.speakerName;
            
            // How do make string builder? Me 2 dumb?
            if (skipBuild)
            {
                reciever.dialogueText.text = current.text;
                skipBuild = false;
                buildTimer = current.buildTime;
                currentChar = 0;
                buildingText = false;
                canBuild = false;
            }
            
            if (canBuild && !buildingText) // Set settings before going to work
            {
                reciever.dialogueText.text = "";
                buildTimer = current.buildTime;
                currentChar = 0;
                buildingText = true;
            }
            else if (buildingText)
            {
                buildTimer -= Time.deltaTime;
                if (buildTimer <= 0.0f)
                {
                    //Place character
                    reciever.dialogueText.text += current.text.ToCharArray()[currentChar];
                    currentChar++;

                    if (currentChar >= current.text.Length)
                    {
                        buildingText = false;
                        canBuild = false;
                        
                    }

                    buildTimer = current.buildTime;
                }
            }
            else
            {
                if (reciever.continuePrompt != null)
                    reciever.continuePrompt.SetActive(true);
            }
        }

        public override ComponentTypes GetComponentType()
        {
            return ComponentTypes.DIALOGUE_BOX;
        }

        private bool AtEndOfLife()
        {
            return (currentIndex >= dialogueBoxes.Count);
        }

        public Speaker GetCurrentSpeaker()
        {
            Speaker speaker = dialogueScriptable.GetSpeakerFromReference(dialogueBoxes[currentIndex].speakerReference, out bool success);
            if (!success) Debug.LogError($"Failed to fetch speaker using reference: {dialogueBoxes[currentIndex].speakerReference}, expect nonsense data");
            return speaker;
        }

        [System.Serializable]
        public struct DialogueBox
        {
            public string speakerReference;
            public string text;
            public float buildTime;

            public DialogueBox(Speaker speaker, string text)
            {
                this.speakerReference = speaker.speakerReference;
                this.text = text;
                this.buildTime = 0.02f;
            }
            
            public DialogueBox(string speakerReference, string text)
            {
                this.speakerReference = speakerReference;
                this.text = text;
                this.buildTime = 0.02f;
            }
        }
    }
}
