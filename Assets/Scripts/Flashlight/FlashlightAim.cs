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

    [Tooltip("TRUE: Flashlight will toggle between being turned on or off when the flashlight button is pressed\n" +
        "FALSE: Flashlight will only be turned on while the button is being held")]
    public bool toggleLight = false;
    bool previousFlashState = false;

    bool aimingUp;
    bool flashButton, flashing;
    float horDirection = -1.0f;
    float yRotation = 0.0f;

    // Bara för testing
    [Tooltip("JUST FOR TESTING")]
    public GameObject lightCone = null;

    // Welcome to if statement city
    void Update()
    {
        if (transform.parent != null)
            yRotation = transform.parent.eulerAngles.y;

        if (!aimingUp)
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, aimForwardRotation * horDirection));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, aimUpRotation * horDirection));


        // Detta får mig att vilja dö, finns det ingen variant av Input.GetButtonDown för ReadValueAsButton?
        if (toggleLight)
        {
            if (flashButton && flashButton != previousFlashState)
            {
                flashing = !flashing;
            }
            previousFlashState = flashButton;
        }
        else
            flashing = flashButton;

        // Bara för testing part 2
        if (lightCone != null)
        {
            if (flashing)
                lightCone.SetActive(true);
            else
                lightCone.SetActive(false);
        }
    }

    #region InputCalls

    public void InputActionFlashing(InputAction.CallbackContext action)
    {
        flashButton = action.ReadValueAsButton();
    }

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
