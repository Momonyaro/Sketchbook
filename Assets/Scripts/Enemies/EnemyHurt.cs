using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using PathCreation;

//Märkte nu i efterhand att EnemyHurt kanske inte var det bästa nämnet, men eh så kan det gå
public class EnemyHurt : MonoBehaviour
{
    [Tooltip("The amount of times the player needs to jump on the enemy before it dies")]
    public int hP = 1;
    [Tooltip("The amount of damage the enemy will deal to the player upon touch")]
    public int damage = 4;
    [Tooltip("Determines how fast the player will be knocked back when hurt")]
    public float horizontalKnockbackSpeed = 10.0f;
    [Tooltip("The force the pushes the player up when they get hurt")]
    public float verticalKnockbackForce = 75.0f;
    [Tooltip("Time (in seconds) the enemy will be stunned after being flashed")]
    public float stunnedTime = 5.0f;
    [Tooltip("The force that will launch the player upwards after bouncing on the enemy")]
    public float playerBounceDistance = 170.0f;
    [Tooltip("The duplicate enemy in the colored world. If this is left empty the duplicate will not be destroyed which causes errors")]
    public GameObject enemyDuplicate = null;

    PlayerController playerController;
    PlayerHurt playerHurt;

    MoveBetweenPoints moveBetweenPoints = null;
    EnemyFly enemyFly = null;
    [HideInInspector]
    public bool stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MoveBetweenPoints>() != null)
            moveBetweenPoints = GetComponent<MoveBetweenPoints>();

        if (GetComponent<EnemyFly>() != null)
            enemyFly = GetComponent<EnemyFly>();

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
            else if (other.gameObject.GetComponent<PlayerHurt>() != null)
            {
                int direction = 1;

                PathCreator currentSpline = GetComponent<SplineWalker>().currentSpline;
                float splineDist = currentSpline.path.GetClosestDistanceAlongPath(transform.position);
                float playerPos = currentSpline.path.GetClosestDistanceAlongPath(other.gameObject.transform.position);

                if (playerPos > splineDist)
                    direction = -1;

                playerHurt = other.gameObject.GetComponent<PlayerHurt>();
                HurtPlayer(damage, direction);
            }
        }
    }

    void HurtPlayer(int damage, int direction)
    {
        //Player gets hurt like a little bitch
        //Why did I write that - Albin 5 days or so later
        playerHurt.GettingHurt(damage, direction, horizontalKnockbackSpeed, verticalKnockbackForce);
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
        if (enemyDuplicate != null)
            Destroy(enemyDuplicate);
        Destroy(gameObject); //självklart en placeholder, nån fin dödseffekt får la spelas i fulla spelet
    }

    IEnumerator IsStunned()
    {
        stunned = true;
        if (moveBetweenPoints != null)
            moveBetweenPoints.stunnedMultiplier = 0.0f;
        if (enemyFly != null)
            enemyFly.stunnedMultiplier = 0.0f;

        yield return new WaitForSeconds(stunnedTime);
        stunned = false;
        if (moveBetweenPoints != null)
            moveBetweenPoints.stunnedMultiplier = 0.0f;
        if (enemyFly != null)
            enemyFly.stunnedMultiplier = 1.0f;
    }
}
