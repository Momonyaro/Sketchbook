using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Config;

namespace Movement
{
    public class PushBlock : MonoBehaviour
    {
        [Tooltip("The speed the block gets pushed or pulled a\n" +
            "SHOULD BE THE SAME AS THE MOVEMENT SPEED OF THE PLAYER WHILE THEY ARE PUSHING/PULLING (default movement speed right now)")]
        public float blockSpeed;

        [HideInInspector]
        public bool atEdge;
        bool canBePushed, tooClose, pushing;
        Rigidbody rb;
        SplineWalker splineWalker;
        GameObject player = null;
        PlayerPush playerPush = null;
        Vector2 lastDelta = Vector2.zero;

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            splineWalker = GetComponent<SplineWalker>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerPush != null)
            {
                pushing = playerPush.pushing;
                lastDelta = playerPush.lastDelta;
            }

            // Matchar spelarens movement, dålig lösning men det fungerar antar jag...
            if (atEdge && pushing)
                MoveAlongSplineHor(lastDelta.x, canBePushed);
        }

        // Just nu verkar inte spelarens speed påverkas av en public speed variabel, så den är basically identisk till player controller
        public void MoveAlongSplineHor(float horSpeed, bool inRange)
        {
            if (horSpeed == 0) return;

            float playerSpeedMultiplier = 1.0f;
            float blockSpeedMultiplier = 0.5f;
            

            var position = rb.position;

            PathCreator currentSpline = splineWalker.currentSpline;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);

            // If city 2
            if (currentSpline.path.GetClosestDistanceAlongPath(player.transform.position) > splineDist) {
                if (horSpeed < -0.001f) {
                    if (tooClose) return;
                    playerSpeedMultiplier = 0.5f;
                    blockSpeedMultiplier = 1.0f; }
                if (horSpeed > 0.001f && !inRange) return;
            }
            else {
                if (horSpeed > 0.001f) {
                    if (tooClose) return;
                    playerSpeedMultiplier = 0.5f;
                    blockSpeedMultiplier = 1.0f; }
                if (horSpeed < -0.001f && !inRange) return;
            }

            horSpeed *= Time.deltaTime * 10.0f * blockSpeedMultiplier;
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist - horSpeed, EndOfPathInstruction.Stop);

            //This is garbage but ok for testing - Sebastian
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;

            playerPush.Pushing(playerSpeedMultiplier);

            rb.MovePosition(splinePos);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerPush>() != null)
            {
                canBePushed = true;
                player = other.gameObject;
                playerPush = other.gameObject.GetComponent<PlayerPush>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerPush>() != null)
                canBePushed = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
                tooClose = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
                tooClose = false;
        }
    }
}
