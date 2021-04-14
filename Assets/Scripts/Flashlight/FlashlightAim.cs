using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightAim : MonoBehaviour
{
    [Tooltip("Default rotation of the flashlight (in degrees)")]
    public float aimForwardRotation = 90.0f;
    [Tooltip("Rotation of the flashlight while the player is aiming up (in degrees)")]
    public float aimUpRotation = 45.0f;

    bool aimingUp;
    float horDirection = -1.0f;
    float yRotation = 0.0f;

    // Welcome to if statement city
    void Update()
    {
        if (transform.parent != null)
            yRotation = transform.parent.eulerAngles.y;

        if (!aimingUp)
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, aimForwardRotation * horDirection));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, aimUpRotation * horDirection));
    }

    #region InputCalls

    public void InputActionAimUp(InputAction.CallbackContext action)
    {
        if (action.ReadValue<Vector2>().y > 0.001f)
            aimingUp = true;
        else
            aimingUp = false;
    }

    public void InputActionChangeHorDirection(InputAction.CallbackContext action)
    {
        if (action.ReadValue<Vector2>().x > 0.001f)
            horDirection = 1.0f;
        else if (action.ReadValue<Vector2>().x < -0.001f)
            horDirection = -1.0f;
    }

    #endregion
}
