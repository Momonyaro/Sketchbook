using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightAim : MonoBehaviour
{
    bool aimingUp;
    float yRotation = 0.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            yRotation = transform.parent.eulerAngles.y;
            print(yRotation);
        }

        if (aimingUp)
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, -45.0f));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, -90.0f));
    }

    public void AimUp(InputAction.CallbackContext context)
    {
        aimingUp = context.ReadValueAsButton();
    }
}
