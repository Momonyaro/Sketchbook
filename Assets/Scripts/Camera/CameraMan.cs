using System;
using Movement;
using PathCreation;
using Unity.Mathematics;
using UnityEngine;

namespace Camera
{
    public class CameraMan : MonoBehaviour
    {
        [Header("Camera Settings")]
        public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
        public bool usePlayerRotation = true;
        [Space]
        public float lookAheadMultiplier = 1.0f;
        public AnimationCurve lookAheadSpeed = new AnimationCurve();
        private float lookAheadBuildup = 0;
        [Space] 
        public bool lerpY = true;
        public float lerpYSpeed = 0.5f;
        public bool lerpUsingDistance = false;
        [Space]
        public SplineWalker splineWalker;
        public PlayerController player;
        [Range(0, 1)] public float minLookAheadThreshold = 0.01f;
        private Transform camTransform;
        private Vector2 lastDelta = Vector2.zero;
        private float lastSign = 0;
        private float currentLookAhead = 0.0f;
        private float lastY = 0.0f;

        #region Unity Events
        
        private void OnValidate()
        {
            camTransform = transform.GetChild(0);
        }

        private void Awake()
        {
            splineWalker = GetComponent<SplineWalker>();
            player = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            CenterCameraOnPlayer();
            lastY = transform.position.y;
        }

        private void Update()
        {
            lastDelta = player.lastDelta;
            CameraBuildup();
            
            LookAheadOfPlayer();

            if (!usePlayerRotation) return;
            transform.rotation = player.transform.rotation;
        }
        
        #endregion

        private void CenterCameraOnPlayer()
        {
            float dist = splineWalker.currentSpline.path.GetClosestDistanceAlongPath(player.transform.position);
            Vector3 splinePos = splineWalker.currentSpline.path.GetPointAtDistance(dist);
            transform.position = splinePos;

            camTransform.position += positionOffset;
            Vector3 camRot = camTransform.rotation.eulerAngles;
            camTransform.rotation = Quaternion.Euler(camRot + rotationOffset);
        }

        private void LookAheadOfPlayer()
        {
            if (Mathf.Abs(lastDelta.x) > minLookAheadThreshold)
                lastSign = Mathf.Lerp(lastSign, Mathf.Sign(-lastDelta.x), 0.4f);

            float lookAheadAmount = lastSign * currentLookAhead * lookAheadMultiplier;
            
            float dist = splineWalker.currentSpline.path.GetClosestDistanceAlongPath(player.transform.position);
            Vector3 splinePos = splineWalker.currentSpline.path.GetPointAtDistance(dist + lookAheadAmount, EndOfPathInstruction.Stop);
            if (lerpY) splinePos = LerpToPlayerY(splinePos, player.transform.position.y);
            transform.position = splinePos;
        }

        private Vector3 LerpToPlayerY(Vector3 splinePos, float playerY)
        {
            float lerp = 0;
            if (!lerpUsingDistance)
            {
                lerp = Mathf.Lerp(lastY, playerY, lerpYSpeed);
            }
            else
            {
                lerp = Mathf.Lerp(lastY, playerY, (1 + lastY) / (1 + playerY));
            }

            lastY = lerp;
            splinePos.y = lerp;
            return splinePos;
        }

        private void CameraBuildup()
        {
            float absDelta = Mathf.Abs(lastDelta.x);

            if (absDelta > minLookAheadThreshold) {
                lookAheadBuildup = Mathf.Min(lookAheadBuildup + Time.deltaTime * absDelta, 
                                            lookAheadSpeed.keys[lookAheadSpeed.length - 1].time);
            } else {
                lookAheadBuildup = Mathf.Max(lookAheadBuildup - Time.deltaTime * 2, 
                                            lookAheadSpeed.keys[0].time);
            }
            
            currentLookAhead = lookAheadSpeed.Evaluate(lookAheadBuildup);
        }

        #region Gizmos
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 camPos = camTransform.position;
            Vector3 myPos = transform.position;
            Vector3 flatPos = new Vector3(camPos.x, myPos.y, camPos.z);
            
            Gizmos.DrawLine(camPos, flatPos);
            Gizmos.DrawLine(myPos, flatPos);
            Gizmos.DrawLine(myPos + new Vector3(2, 0, -2), myPos + new Vector3(-2, 0, -2));
            Gizmos.DrawLine(myPos + new Vector3(-2, 0, 2), myPos + new Vector3( 2, 0,  2));
            Gizmos.DrawLine(myPos + new Vector3(2, 0, -2), myPos + new Vector3( 2, 0,  2));
            Gizmos.DrawLine(myPos + new Vector3(-2, 0, -2), myPos + new Vector3(-2, 0, 2));
        }

        #endregion
        
    }
}
