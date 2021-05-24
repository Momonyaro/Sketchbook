using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class Wind : MonoBehaviour
{
    [Tooltip("The horizontal (left and right) force that pushes the player. Note that negative numbers work if you want the player to be pushed in the opposite direction")]
    public float horizontalWindForce = 10.0f;
    [Tooltip("The vertical (up and down) force that pushes the player. Note that negative numbers work if you want the player to be pushed downwards")]
    public float verticalWindForce = 0.0f;

    PlayerController playerController = null;
    bool touchingPlayer = false; // Used for removing the wind from the player correctly when the wind gets diasbled

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.horiWindForce += horizontalWindForce;
            playerController.vertWindForce += verticalWindForce;
            touchingPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && playerController != null)
        {
            playerController.horiWindForce -= horizontalWindForce;
            playerController.vertWindForce -= verticalWindForce;
            touchingPlayer = false;
        }
    }

    private void OnDisable()
    {
        if (playerController != null && touchingPlayer)
        {
            playerController.horiWindForce -= horizontalWindForce;
            playerController.vertWindForce -= verticalWindForce;
        }
    }
}
