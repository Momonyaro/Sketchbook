using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class EnemyJump : MonoBehaviour
{
    [Tooltip("If these objects intersect with the ground, the enemy will be able to perform a jump")]
    public Transform GroundCheckLeft = null, GroundCheckRight = null;
    [Tooltip("The force that pushes the enemy upwards when it jumps")]
    public float jumpForce = 50.0f;
    [Min(1.0f)] public float jumpSpeed = 2.0f;
    [Min(1.0f)] public float fallSpeed = 2.7f;
    [Tooltip("The time (in seconds) the enemy waits before jumping after landing")]
    public float waitTime = 1.0f;

    Rigidbody rb;
    SplineWalker splineWalker;
    MoveBetweenPoints moveBetweenPoints;
    EnemyHurt enemyHurt;

    bool aboutToJump;

    void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        aboutToJump = false;
        moveBetweenPoints = GetComponent<MoveBetweenPoints>();
        rb = GetComponent<Rigidbody>();
        splineWalker = GetComponent<SplineWalker>();
        enemyHurt = GetComponent<EnemyHurt>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < 0) // Falling
        {
            Vector3 velocity = rb.velocity;
            velocity.y += Physics.gravity.y * (fallSpeed - 1.0f);
            rb.velocity = velocity;
        }
        else if (rb.velocity.y > 0) // Jumping
        {
            //airCurrentTimer = airTimer;
            Vector3 velocity = rb.velocity;
            velocity.y += Physics.gravity.y * (jumpSpeed - 1.0f);
            rb.velocity = velocity;
        }

        if (enemyHurt.stunned)
        {
            StopAllCoroutines();
            moveBetweenPoints.jumpingMultiplier = 0.0f;
            aboutToJump = false;
        }
        else if (IsGrounded() && !aboutToJump && rb.velocity.y < 0.0f)
        {
            StopAllCoroutines();
            StartCoroutine(WaitBeforeJumping());
            aboutToJump = true;
        }
    }

    //Här så gör vi så att karaktären kan hoppa. (ska vi använda AddForce?)
    public void JumpUsingForce()
    {
        if (!IsGrounded()) return;
        moveBetweenPoints.jumpingMultiplier = 1.0f;
        aboutToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        rb.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.Impulse);
        splineWalker.AnchorXZToSpline();
    }

    public bool IsGrounded()
    {
        if (GroundCheckLeft != null && GroundCheckRight != null)
        {
            var position = rb.position + new Vector3(0.0f, 0.25f, 0.0f);
            if (Physics.Linecast(position, GroundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
                || Physics.Linecast(position, GroundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"))
                || Physics.Linecast(GroundCheckLeft.position, GroundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
                return true;
        }

        return false;
    }

    IEnumerator WaitBeforeJumping()
    {
        moveBetweenPoints.jumpingMultiplier = 0.0f;
        yield return new WaitForSeconds(waitTime);
        JumpUsingForce();
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (GroundCheckLeft != null && GroundCheckRight != null)
        {
            var position = rb.position + new Vector3(0.0f, 0.25f, 0.0f);
            Gizmos.DrawLine(position, GroundCheckLeft.position);
            Gizmos.DrawLine(position, GroundCheckRight.position);
            Gizmos.DrawLine(GroundCheckLeft.position, GroundCheckRight.position);
        }
    }

    #endregion
}
