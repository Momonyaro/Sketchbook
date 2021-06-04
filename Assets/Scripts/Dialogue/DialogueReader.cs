using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogue
{
    public class DialogueReader : MonoBehaviour
    {
        public DialogueScriptable dialogueData;
        public bool initOnStart = false;
        public GameObject allFather;
        
        private Queue<DialogueComponent> executionQueue = new Queue<DialogueComponent>();
        private bool dialogueRunning = false;
        private DialogueComponent lastComponent = null;
        
        [SerializeField] private InputActionAsset module;
        private InputActionMap inputActionMap;
        private InputAction submit;

        private void Awake()
        {
            inputActionMap = module.FindActionMap("UI");
            submit = inputActionMap.FindAction("Submit");
            submit.started += PassInputSubmit;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            if (initOnStart)
                StartDialogue();
        }

        private void Update()
        {
            if (!dialogueRunning) return;
            
            lastComponent.Update(out bool endOfLife);
            
            if (endOfLife) CycleComponent();
        }

        public void StartDialogue()
        {
            PopulateExecQueue();

            lastComponent = executionQueue.Dequeue();
            InitComponent(ref lastComponent);
            dialogueRunning = true;
        }

        private void CycleComponent()
        {
            if (executionQueue.Count == 0)
            {
                EndDialogue();
                return;
            }
            
            lastComponent = executionQueue.Dequeue();
            InitComponent(ref lastComponent);
        }

        private void PassInputSubmit(InputAction.CallbackContext context)
        {
            lastComponent?.OnSubmitInput(context);
        }

        private void EndDialogue()
        {
            dialogueRunning = false;
            lastComponent = null;
            Debug.Log($"Dialogue: End of Read");
            if (GetComponent<ChangeScene>() != null)
                GetComponent<ChangeScene>().StartSceneChange();
        }

        private void InitComponent(ref DialogueComponent component)
        {
            Debug.Log($"Initializing Component of type: {component.GetComponentType()}, with ref: {component.reference}");
            
            GameObject toInstantiate;
            component.Init(dialogueData, out toInstantiate);

            if (toInstantiate != null)
            {
                GameObject instance = Instantiate(toInstantiate, allFather.transform);
                RectTransform rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = component.screenPosition;
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                component.SetNewInstance(instance);
            }
        }

        private void PopulateExecQueue()
        {
            List<DialogueComponent> dialogueComponents = dialogueData.components;
            executionQueue.Clear();
            for (int i = 0; i < dialogueComponents.Count; i++)
            {
                executionQueue.Enqueue(dialogueComponents[i]);
            }
        }
    }
}
