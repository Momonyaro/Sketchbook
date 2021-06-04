using System;
using FMODUnity;
using UnityEngine;

namespace Objects
{
    public class FmodAudioTrigger : MonoBehaviour
    {
        public StudioEventEmitter emitter;
        
        [Header("On Trigger Enter:")]
        
        public bool setOnTriggerEnter = true;
        public string triggerEnterParamTitle = "";
        public float triggerEnterValue = 0.0f;
        
        [Header("On Trigger Exit:")]
        
        public bool setOnTriggerExit = true;
        public string triggerExitParamTitle = "";
        public float triggerExitValue = 0.0f;

        [Header("On Destroy:")]

        public bool setOnDestroy = true;
        public string destroyTitle = "";
        public float destroyValue = 0.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (!setOnTriggerEnter) return;
            if (other.CompareTag("Player"))
            {
                Debug.Log("Roligt litet medelande");
                emitter.SetParameter(triggerEnterParamTitle, triggerEnterValue);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!setOnTriggerExit) return;
            if (other.CompareTag("Player"))
            {
                emitter.SetParameter(triggerExitParamTitle, triggerExitValue);
            }
        }

        private void OnDestroy()
        {
            if (!setOnDestroy) return;
            emitter.SetParameter(destroyTitle, destroyValue);
        }
    }
}