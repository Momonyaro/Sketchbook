using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Config;

namespace Movement
{
    public class Dog : MonoBehaviour
    {
        [Tooltip("The speed the object will move at")]
        public float movementSpeed = 5.0f;
        [Tooltip("TRUE: The object will move towards the start of the spline\n" +
            "FALSE: The object will move towards the end of the spline")]
        public bool movingTowardsStart = true;
        float movingDirection;

        float splineDist, playerDist;

        GameObject player = null;
        Rigidbody rb;
        SplineWalker splineWalker;

        // Start is called before the first frame update
        void Start()
        {
            if (movingTowardsStart)
                movingDirection = -1.0f;
            else
                movingDirection = 1.0f;

            rb = GetComponent<Rigidbody>();
            splineWalker = GetComponent<SplineWalker>();

            if (GameObject.FindGameObjectWithTag("Player") != null)
                player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var position = rb.position;

            PathCreator currentSpline = splineWalker.currentSpline;
            splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            if (player != null)
            {
                playerDist = currentSpline.path.GetClosestDistanceAlongPath(player.transform.position);
                print(playerDist - splineDist);
            }

            MoveAlongSplineHor(currentSpline, position);
        }

        public void MoveAlongSplineHor(PathCreator currentSpline, Vector3 position)
        {
            float movingDir = movingDirection * Time.deltaTime * movementSpeed;

            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist - movingDir, EndOfPathInstruction.Stop);

            //This is garbage but ok for testing - still Sebastian
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;

            rb.MovePosition(splinePos);
        }
    }
}

