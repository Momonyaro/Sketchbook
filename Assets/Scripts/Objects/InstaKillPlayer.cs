using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class InstaKillPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerHurt>() != null)
        {
            StartCoroutine(other.gameObject.GetComponent<PlayerHurt>().Die());
        }
    }
}
