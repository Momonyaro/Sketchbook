using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class Fan : MonoBehaviour
{
    [Tooltip("TRUE: Fan starts off being turned on\n" +
            "FALSE: Fan starts off being turned off")]
    public bool turnedOn = true;

    [Tooltip("The object with the Wind script attached to it")]
    public GameObject windObject;

    // Start is called before the first frame update
    void Start()
    {
        windObject.SetActive(turnedOn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flash")
        {
            print("gah");
            turnedOn = !turnedOn;
            windObject.SetActive(turnedOn);
        }
    }
}
