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
    FlashlighFlash flashlightFlash = null; // Märkte precis att skriptet "FlashlighFlash" stavar "Flashlight" fel...

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        ChangeSpeedMultiplier(1.0f);
        playerSpeed = playerController.moveSpeed;

        if (FindObjectOfType<FlashlighFlash>() != null)
            flashlightFlash = FindObjectOfType<FlashlighFlash>();
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
        ChangeSpeedMultiplier(speed);
        print("what");
        if (flashlightFlash != null)
            flashlightFlash.flashing = false;
    }

    public void ChangeSpeedMultiplier(float speed) // Görs utanför Pushing så att andra skript ska kunna ändra speeden utan att spelaren puttar
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
