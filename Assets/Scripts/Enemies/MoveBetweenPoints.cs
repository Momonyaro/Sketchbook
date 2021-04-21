using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Config;

namespace Movement
{
    public class MoveBetweenPoints : MonoBehaviour
    {
        [Tooltip("The speed the object will move at")]
        public float movementSpeed = 10.0f;
        [Tooltip("The two positions the object will move between")]
        public GameObject point1 = null, point2 = null;
        [Tooltip("TRUE: Will start off moving towards Point 1\n" +
            "FALSE: Will start off moving towards Point 2")]
        public bool moveTo1 = true;

        Rigidbody rb;
        SplineWalker splineWalker;
        float moveHor1 = -1.0f, moveHor2 = 1.0f;
        float point1Pos, point2Pos;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            splineWalker = GetComponent<SplineWalker>();

            var position = rb.position;

            PathCreator currentSpline = splineWalker.currentSpline;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            point1Pos = currentSpline.path.GetClosestDistanceAlongPath(point1.transform.position);
            point2Pos = currentSpline.path.GetClosestDistanceAlongPath(point2.transform.position);

            if (currentSpline.path.GetClosestDistanceAlongPath(point1.transform.position) > splineDist)
            {
                moveHor1 = -1.0f;
                moveHor2 = 1.0f;
            }
            else
            {
                moveHor1 = 1.0f;
                moveHor2 = -1.0f;
            }
        }

        // Update is called once per frame
        void Update()
        {
            var position = rb.position;

            PathCreator currentSpline = splineWalker.currentSpline;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);

            if (moveTo1)
            {
                if ((moveHor1 < 0.0f && point1Pos <= splineDist) || (moveHor1 > 0.0f && point1Pos >= splineDist))
                    moveTo1 = false;
                else
                    MoveAlongSplineHor(moveHor1, splineDist, currentSpline, position);
            }
            else
            {
                if ((moveHor2 < 0.0f && point2Pos <= splineDist) || (moveHor2 > 0.0f && point2Pos >= splineDist))
                    moveTo1 = true;
                else
                    MoveAlongSplineHor(moveHor2, splineDist, currentSpline, position);
            }
        }

        public void MoveAlongSplineHor(float movingDir, float splineDist, PathCreator currentSpline, Vector3 position)
        {
            movingDir *= Time.deltaTime * movementSpeed;
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist - movingDir, EndOfPathInstruction.Stop);

            //This is garbage but ok for testing - Sebastian
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;

            rb.MovePosition(splinePos);
        }
    }
}