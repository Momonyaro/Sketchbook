using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Movement;

public class PlayerPush : MonoBehaviour
{
    [HideInInspector]
    public bool pushing;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region InputCalls
    
    public void InputActionGrab(InputAction.CallbackContext action)
    {
        pushing = action.ReadValueAsButton();
    }

    #endregion
}
