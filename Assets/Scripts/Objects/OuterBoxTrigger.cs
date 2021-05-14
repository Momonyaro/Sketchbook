using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class OuterBoxTrigger : MonoBehaviour
{
    PushBlock block;
    
    // Start is called before the first frame update
    void Start()
    {
        block = transform.parent.GetComponent<PushBlock>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            block.atEdge = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerPush>() != null)
        {
            block.atEdge = false;
            other.gameObject.GetComponent<PlayerPush>().ChangeSpeedMultiplier(1.0f);
        }
    }
}
