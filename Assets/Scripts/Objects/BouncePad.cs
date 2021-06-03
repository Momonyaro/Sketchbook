using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using FMODUnity;

public class BouncePad : MonoBehaviour
{
    [Tooltip("The force that will launch the player upwards after bouncing on the enemy")]
    public float playerBounceDistance = 170.0f;
    public bool emitsAudio = false;
    public StudioEventEmitter bounceEmitter;

    PlayerController playerController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            if (!playerController.IsGrounded())
            {
                playerController.BounceUsingForce(playerBounceDistance);
                if (emitsAudio)
                {
                    bounceEmitter.Play();
                }
            }
        }
    }
}
