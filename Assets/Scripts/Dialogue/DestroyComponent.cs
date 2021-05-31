using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogue
{
    public class DestroyComponent : DialogueComponent
    {
        public string refToDestroy = "";
        
        public override void Init(DialogueScriptable parent, out GameObject componentPrefab)
        {
            parent.DestroyComponentInstance(refToDestroy);
            
            componentPrefab = null;
        }

        public override void Update(out bool endOfLife)
        {
            //Instantly destruct, we're doing all the logic during init.
            endOfLife = true;
        }

        public override void OnSubmitInput(InputAction.CallbackContext context)
        {
            //We don't rely on inputs
        }

        public override ComponentTypes GetComponentType()
        {
            return ComponentTypes.DESTROY;
        }
    }
}
