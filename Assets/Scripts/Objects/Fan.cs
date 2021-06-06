using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using Animation;

public class Fan : MonoBehaviour
{
    [Tooltip("TRUE: Fan starts off being turned on\n" +
            "FALSE: Fan starts off being turned off")]
    public bool turnedOn = true;

    [Tooltip("The object with the Wind script attached to it")]
    public GameObject windObject;
    [Tooltip("The particle effect that displays the wind")]
    public GameObject particleEffect = null;
    [Tooltip("The animator!")]
    public MeshAnimator meshAnimator = null;

    // Start is called before the first frame update
    void Start()
    {
        EnableDisableWind(turnedOn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flash")
        {
            turnedOn = !turnedOn;
            EnableDisableWind(turnedOn);
        }
    }

    void EnableDisableWind(bool enabled)
    {
        if (windObject != null)
            windObject.SetActive(turnedOn);

        if (particleEffect != null)
            particleEffect.SetActive(turnedOn);

        ChangeAnimation(enabled);
    }

    void ChangeAnimation(bool enabled)
    {
        if (meshAnimator == null)
            return;
        if (enabled)
            meshAnimator.StartAnimFromName("_fanTurnedOn", false);
        else
            meshAnimator.StartAnimFromName("_fanTurnedOff", false);
    }
}
