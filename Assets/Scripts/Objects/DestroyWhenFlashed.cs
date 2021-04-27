using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenFlashed : MonoBehaviour
{
    [Tooltip("The object that spawns once this current object is destroyed. If left blank, nothing gets spawned")]
    public GameObject spawnOnDestroy = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flash")
        {
            if (spawnOnDestroy != null)
                Instantiate(spawnOnDestroy, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
