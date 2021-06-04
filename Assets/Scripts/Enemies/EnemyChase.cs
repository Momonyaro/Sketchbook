﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Config;
using Animation;

namespace Movement
{
    public class EnemyChase : MonoBehaviour
    {
        [Tooltip("If these objects intersect with a wall with the 'Ground' layer, the enemy will turn around")]
        public Transform wallCheck1 = null, wallCheck2 = null;

        [Tooltip("The speed the object will move at")]
        public float movementSpeed = 10.0f;
        [Tooltip("The time (in seconds) the enemy will continue to run around when the flashlight is no longer on him")]
        public float stopTime = 5.0f;

        [Tooltip("The animator!")]
        public MeshAnimator meshAnimator = null;

        [Min(1.0f)] public float riseSpeed = 2.0f;
        [Min(1.0f)] public float fallSpeed = 2.7f;

        float movingDirection = 1.0f;
        bool animDirection;
        float splineDist, playerDist;

        GameObject player = null;
        Rigidbody rb;
        SplineWalker splineWalker;
        bool inLight, turningRound;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            splineWalker = GetComponent<SplineWalker>();

            if (meshAnimator != null)
                meshAnimator.StartAnimFromName("_brevIdle", animDirection);

            if (GameObject.FindGameObjectWithTag("Player") != null)
                player = GameObject.FindGameObjectWithTag("Player");
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
                velocity.y += Physics.gravity.y * (riseSpeed - 1.0f);
                rb.velocity = velocity;
            }

            var position = rb.position;

            PathCreator currentSpline = splineWalker.currentSpline;
            splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            if (player != null)
                playerDist = currentSpline.path.GetClosestDistanceAlongPath(player.transform.position);

            if (inLight)
                MoveAlongSplineHor(currentSpline, position);
        }

        public void MoveAlongSplineHor(PathCreator currentSpline, Vector3 position)
        {
            if (!turningRound)
            {
                if (Physics.Linecast(position, wallCheck1.position, 1 << LayerMask.NameToLayer("Ground"))
                    || Physics.Linecast(position, wallCheck2.position, 1 << LayerMask.NameToLayer("Ground")))
                {
                    print("ga");
                    movingDirection *= -1.0f;
                    animDirection = !animDirection;
                    StartCoroutine(CannotTurnAround());
                    turningRound = true;
                }
            }

            float movingDir = movingDirection * Time.deltaTime * movementSpeed;

            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist - movingDir, EndOfPathInstruction.Stop);

            //This is garbage but ok for testing - still Sebastian
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;

            rb.MovePosition(splinePos);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Light")
            {
                GameObject flash = other.gameObject;
                flash.transform.parent.GetComponent<FlashlighFlash>().onFlashOff.AddListener(CannotSeePlayer);
                inLight = true;
                if (player != null)
                {
                    if (playerDist > splineDist)
                    {
                        movingDirection = -1.0f;
                        animDirection = false;
                    }
                    else
                    {
                        movingDirection = 1.0f;
                        animDirection = true;
                    }
                }

                if (meshAnimator != null)
                    meshAnimator.StartAnimFromName("_brevAttack", animDirection);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Light")
                CannotSeePlayer();
        }

        public void CannotSeePlayer()
        {
            turningRound = false;
            StopAllCoroutines();
            StartCoroutine(StopChasing());
        }

        IEnumerator StopChasing()
        {
            yield return new WaitForSeconds(stopTime);
            inLight = false;
            if (meshAnimator != null)
                meshAnimator.StartAnimFromName("_brevIdle", animDirection);
        }

        IEnumerator CannotTurnAround() // This kinda dumbo but it's almost 16:00 and I wanna get this done, so simple dumb fix it is
        {
            yield return new WaitForSeconds(0.5f);
            turningRound = false;
        }
    }
}