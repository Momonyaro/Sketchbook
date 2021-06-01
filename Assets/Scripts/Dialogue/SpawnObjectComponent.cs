using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogue
{
    public class SpawnObjectComponent : DialogueComponent
    {
        public override void Init(DialogueScriptable parent, out GameObject componentPrefab)
        {
            componentPrefab = objectPrefab;
        }

        public override void Update(out bool endOfLife)
        {
            endOfLife = true;
        }

        public override void OnSubmitInput(InputAction.CallbackContext context)
        {
            
        }

        public override ComponentTypes GetComponentType()
        {
            return ComponentTypes.SPAWN_OBJECT;
        }
    }
}
