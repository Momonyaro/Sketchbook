using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleButtons : MonoBehaviour
{
    // This was made on a saturday, I don't want to work on a saturday, sad
    [Tooltip("The circle that gets drawn when the mouse is hovering over this object")]
    public SpriteRenderer circle;

    public ChangeScene changeScene;

    public float horizontalRange = 0.1f;
    public bool exitButton = false;

    bool targeted = false;
    bool clickin = false, previousClickin = false;

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        mousePosition -= new Vector2(Camera.main.WorldToViewportPoint(transform.position).x, Camera.main.WorldToViewportPoint(transform.position).y);

        if (mousePosition.y <= 0.06f && mousePosition.y >= -0.04f && mousePosition.x <= horizontalRange && mousePosition.x >= -horizontalRange)
            targeted = true;
        else
            targeted = false;

        circle.enabled = targeted;
    }

    public void Click(InputAction.CallbackContext context)
    {
        clickin = context.ReadValueAsButton(); // jag vet att det finns en OnButtonDown variant på nåt sätt, men det är lördag och jag har huvudverk så jag orkar inte kolla upp det, sad
        if (clickin && !previousClickin)
            JustClicked();
        previousClickin = clickin;
    }

    void JustClicked()
    {
        if (targeted)
        {
            if (exitButton)
                Application.Quit();
            if (changeScene != null)
                changeScene.StartSceneChange();
        }
    }
}
