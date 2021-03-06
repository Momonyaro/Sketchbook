using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Movement;

public class PlayerPush : MonoBehaviour
{
    [HideInInspector]
    public bool pushing, pushLastFrame;
    [HideInInspector]
    public Vector2 lastDelta = Vector2.zero;
    [HideInInspector]
    public float playerSpeed;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        Pushing(1.0f);
        playerSpeed = playerController.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        lastDelta = playerController.lastDelta;

        if (!pushing && pushLastFrame)
            Pushing(1.0f);
        pushLastFrame = pushing;
    }

    public void Pushing(float speed)
    {
        playerController.pushSpeedMultiplier = speed;
    }

    #region InputCalls
    
    public void InputActionGrab(InputAction.CallbackContext action)
    {
        pushing = action.ReadValueAsButton();
    }

    #endregion
}
