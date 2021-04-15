using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Movement;

public class PlayerPush : MonoBehaviour
{
    [HideInInspector]
    public bool pushing;
    [HideInInspector]
    public Vector2 lastDelta = Vector2.zero;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        lastDelta = playerController.lastDelta;
    }

    #region InputCalls
    
    public void InputActionGrab(InputAction.CallbackContext action)
    {
        pushing = action.ReadValueAsButton();
    }

    #endregion
}
