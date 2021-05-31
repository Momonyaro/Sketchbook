using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [Tooltip("The velocity the object will have when flying upwards")]
    float flyUpwardsVelocity = 3.0f;
    [Tooltip("The velocity the object will have when floating back down")]
    float floatDownwardsVelocity = -1f;

    [HideInInspector]
    public bool touchingGoat;

    Goat goat = null;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        touchingGoat = false;
    }

    void FixedUpdate()
    {
        if (touchingGoat && goat != null && goat.onFire)
        {
            rb.velocity = new Vector3(0.0f, flyUpwardsVelocity, 0.0f);
        }
        else
            rb.velocity = new Vector3(0.0f, floatDownwardsVelocity, 0.0f);
    }

    public void SetGoat(GameObject goatObject)
    {
        if (goatObject.GetComponent<Goat>() != null)
            goat = goatObject.GetComponent<Goat>();
    }
}
