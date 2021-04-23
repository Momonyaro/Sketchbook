using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

//Märkte nu i efterhand att EnemyHurt kanske inte var det bästa nämnet, men eh så kan det gå
public class EnemyHurt : MonoBehaviour
{
    [Tooltip("The amount of times the player needs to jump on the enemy before it dies")]
    public int hP = 1;
    [Tooltip("The amount of damage the enemy will deal to the player upon touch")]
    public int damage = 1;
    [Tooltip("Time (in seconds) the enemy will be stunned after being flashed")]
    public float stunnedTime = 5.0f;
    [Tooltip("The force that will launch the player upwards after bouncing on the enemy")]
    public float playerBounceDistance = 170.0f;

    PlayerController playerController;

    MoveBetweenPoints moveBetweenPoints;
    bool stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        moveBetweenPoints = GetComponent<MoveBetweenPoints>();
        stunned = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flash")
        {
            StopAllCoroutines();
            StartCoroutine(IsStunned());
        }
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            if (stunned && other.gameObject.transform.position.y >= transform.position.y && !playerController.IsGrounded())
            {
                playerController.BounceUsingForce(playerBounceDistance);
                TakeDamage();
            }
            else
                HurtPlayer(damage);
        }
    }

    void HurtPlayer(int damage)
    {
        //Player gets hurt like a little bitch
    }

    void TakeDamage()
    {
        //Maybe add a flash or another hurt animation here?
        hP--;
        if (hP <= 0)
            Die();
    }
    void Die()
    {
        //sad
        Destroy(gameObject); //självklart en placeholder, nån fin dödseffekt får la spelas i fulla spelet
    }

    IEnumerator IsStunned()
    {
        stunned = true;
        moveBetweenPoints.stunnedMultiplier = 0.0f;
        yield return new WaitForSeconds(stunnedTime);
        stunned = false;
        moveBetweenPoints.stunnedMultiplier = 1.0f;
    }
}
