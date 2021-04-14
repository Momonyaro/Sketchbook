using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlighFlash : MonoBehaviour
{
    [Tooltip("TRUE: Flashlight will toggle between being turned on or off when the flashlight button is pressed\n" +
        "FALSE: Flashlight will only be turned on while the button is being held")]
    public bool toggleLight = false;
    bool previousFlashState = false;

    bool flashButton, flashing;

    // Bara för testing
    [Tooltip("JUST FOR TESTING")]
    public GameObject lightCone = null;
    
    // Update is called once per frame
    void Update()
    {
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

    #endregion
}
