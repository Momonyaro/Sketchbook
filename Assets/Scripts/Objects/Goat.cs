using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : MonoBehaviour
{
    [HideInInspector]
    public bool onFire = false;

    public void PutOut()
    {
        onFire = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Balloon>() != null)
        {
            other.gameObject.GetComponent<Balloon>().touchingGoat = true;
            other.gameObject.GetComponent<Balloon>().SetGoat(gameObject);
        }

        if (other.gameObject.tag == "Light")
        {
            GameObject flash = other.gameObject;
            flash.transform.parent.GetComponent<FlashlighFlash>().onFlashOff.AddListener(PutOut);
            onFire = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Balloon>() != null)
            other.gameObject.GetComponent<Balloon>().touchingGoat = false;

        if (other.gameObject.tag == "Light")
            PutOut();
    }
}
