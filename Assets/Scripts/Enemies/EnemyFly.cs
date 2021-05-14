using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [Tooltip("Determines the flight speed, needs to be over 0")]
    [Min(0.0f)] public float flySpeed = 10.0f;

    [Tooltip("The distance the enemy will fly upwards from its start position, needs to be 0 or above")]
    [Min(0.0f)] public float upOffset = 5.0f;
    [Tooltip("The distance the enemy will fly downwards from its start position, needs to be 0 or above")]
    [Min(0.0f)] public float downOffset = 5.0f;

    [Tooltip("Determines the distance between the enemy and one of its end points when it starts slowing down before changing directions. Higher numbers means that it starts slowing down earlier")]
    public float slowDownThreshold = 3.0f;

    [Tooltip("TRUE: Will start off flying upwards\n" +
            "FALSE: Will start off flying downwards")]
    public bool flyUp = true;

    float startYPos;

    float flightDirection = 1.0f;
    float speedModifier = 1.0f;
    [HideInInspector]
    public float stunnedMultiplier = 1.0f;

    float upDistance, downDistance;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startYPos = transform.position.y;

        if (flyUp)
            flightDirection = 1.0f;
        else
            flightDirection = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        upDistance = upOffset - (transform.position.y - startYPos);
        downDistance = -downOffset - (transform.position.y - startYPos);

        if (upDistance <= 0.0f)
        {
            flyUp = false;
            flightDirection = -1.0f;
        }
        else if (downDistance >= 0.0f)
        {
            flyUp = true;
            flightDirection = 1.0f;
        }

        if (Mathf.Abs(upDistance) <= slowDownThreshold)
            speedModifier = Mathf.Sqrt((Mathf.Abs(upDistance) / slowDownThreshold)) + 0.01f; // + 0.05f to keep it from going to 0 which would make the enemy just stop permanently
        else if (Mathf.Abs(downDistance) <= slowDownThreshold)
            speedModifier = Mathf.Sqrt((Mathf.Abs(downDistance) / slowDownThreshold)) + 0.01f;

        if (speedModifier > 1.0f)
            speedModifier = 1.0f;

        rb.velocity = new Vector3(rb.velocity.x, flySpeed * speedModifier * flightDirection * stunnedMultiplier, rb.velocity.z);
    }
}
