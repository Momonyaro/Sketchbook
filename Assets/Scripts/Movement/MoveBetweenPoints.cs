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
        [Tooltip("TRUE: Will only start moving once the player has touched the object\n" +
            "FALSE: Will start moving as soon as the scene loads")]
        public bool moveWhenTouched = false;

        bool touched;

        Rigidbody rb;
        SplineWalker splineWalker;
        float moveHor1 = -1.0f, moveHor2 = 1.0f;
        float point1Pos, point2Pos;
        [HideInInspector]
        public float stunnedMultiplier = 1.0f, jumpingMultiplier = 1.0f;
        bool hasEnemyJump = false;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            splineWalker = GetComponent<SplineWalker>();
            touched = false;

            if (GetComponent<EnemyJump>() != null)
                hasEnemyJump = true;

            print(hasEnemyJump);

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

            if ((moveWhenTouched && touched) || !moveWhenTouched) //Jag hatar det här
            {
                if (moveTo1)
                {
                    if ((hasEnemyJump && jumpingMultiplier <= 0.0f && moveHor1 < 0.0f && point1Pos <= splineDist) || (hasEnemyJump && jumpingMultiplier <= 0.0f && moveHor1 > 0.0f && point1Pos >= splineDist))
                        moveTo1 = false;
                    else if ((!hasEnemyJump && moveHor1 < 0.0f && point1Pos <= splineDist) || (!hasEnemyJump && moveHor1 > 0.0f && point1Pos >= splineDist))
                        moveTo1 = false;
                    else
                        MoveAlongSplineHor(moveHor1, splineDist, currentSpline, position);
                }
                else
                {
                    if ((hasEnemyJump && jumpingMultiplier <= 0.0f && moveHor2 < 0.0f && point2Pos <= splineDist) || (hasEnemyJump && jumpingMultiplier <= 0.0f && moveHor2 > 0.0f && point2Pos >= splineDist))
                        moveTo1 = true;
                    else if ((!hasEnemyJump && moveHor2 < 0.0f && point2Pos <= splineDist) || (!hasEnemyJump && moveHor2 > 0.0f && point2Pos >= splineDist))
                        moveTo1 = true;
                    else
                        MoveAlongSplineHor(moveHor2, splineDist, currentSpline, position);
                }
            }
        }

        public void MoveAlongSplineHor(float movingDir, float splineDist, PathCreator currentSpline, Vector3 position)
        {
            movingDir *= Time.deltaTime * movementSpeed * stunnedMultiplier * jumpingMultiplier;
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist - movingDir, EndOfPathInstruction.Stop);

            //This is garbage but ok for testing - Sebastian
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;

            rb.MovePosition(splinePos);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                touched = true;
            }
        }
    }
}