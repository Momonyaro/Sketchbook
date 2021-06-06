﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogue
{
    public enum ComponentTypes
    {
        NONE,
        DIALOGUE_BOX,
        DESTROY,
        WAIT,
        SPAWN_OBJECT,
    }
    
    [CreateAssetMenu(fileName = "DialogueAsset", menuName = "Dialogue/DialogueAsset")]
    public class DialogueScriptable : ScriptableObject
    {
        public Speaker[] speakers = new Speaker[0];
        [SerializeReference] public List<DialogueComponent> components = new List<DialogueComponent>();

        public void DestroyComponentInstance(string reference)
        {
            DialogueComponent found = null;
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].reference.Equals(reference))
                {
                    found = components[i];
                    break;
                }
            }

            if (found == null) return; // Return if no component was found

            if (found.IsNull()) return; // Return if instance already was null
            Destroy(found.GetCurrentInstance());
            found.NullifyInstance();
        }

        public Speaker GetSpeakerFromReference(string reference, out bool success)
        {
            Speaker toReturn = new Speaker();
            success = false;
            for (int i = 0; i < speakers.Length; i++)
            {
                if (speakers[i].speakerReference.Equals(reference))
                {
                    toReturn = speakers[i];
                    success = true;
                    break;
                }
            }

            return toReturn;
        }
    }

    [System.Serializable]
    public abstract class DialogueComponent
    {
        //This is the base class for all future objects that interact with the system
        public string reference = "";
        public GameObject objectPrefab = null;
        public Vector2 screenPosition = Vector2.zero;
        private GameObject currentInstance = null;
        public int errorAttempts = 99;
        public bool reveal = true;

        public abstract void Init(DialogueScriptable parent, out GameObject componentPrefab);

        public abstract void Update(out bool endOfLife);

        public abstract void OnSubmitInput(InputAction.CallbackContext context);

        public abstract ComponentTypes GetComponentType();

        public void SetNewInstance(GameObject instance)
        {
            currentInstance = instance;
        }
        
        public void NullifyInstance()
        {
            currentInstance = null;
        }
        
        public GameObject GetCurrentInstance()
        {
            return currentInstance;
        }

        public bool IsNull()
        {
            return (currentInstance == null);
        }
    }

    [System.Serializable]
    public struct Speaker
    {
        public string speakerName;
        public string speakerReference;
        public Sprite speakerPhoto;

        public Speaker(string speakerName, string speakerReference, Sprite speakerPhoto)
        {
            this.speakerName = speakerName;
            this.speakerReference = speakerReference;
            this.speakerPhoto = speakerPhoto;
        }
    }
}
