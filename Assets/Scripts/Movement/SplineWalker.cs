using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;
using PathCreation;
using UnityEngine.InputSystem;

namespace Movement
{
    
    // Denna klassen ansvarar för att anknyta objekt till kartans spline.
    
    public class SplineWalker : MonoBehaviour
    {
        public enum RotAxis
        {
            X,
            Y,
            Z
        }
        
        [Range(0, 1)]
        public float debugSpeed = 0.5f;
        public bool assignSplineAtAwake = true;
        public PathCreator currentSpline;
        public bool rotateWithSpline = true;
        public float rotOffset = 90;
        public RotAxis rotAxis = RotAxis.Y;

        private Vector2 lastDelta = Vector2.zero;
        private new Rigidbody rigidbody;

        private void Awake()
        {
            // Här så sätter vi splinen som objektet ska följa om den inte redan är satt.
            if (assignSplineAtAwake && currentSpline == null)
                currentSpline = FindObjectOfType<PathCreator>();

            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            //Här sätter vi objektets position på splinen.
            AnchorXZToSpline();
        }

        private void Update()
        {
            //Oh god, oh fuck this is terrible but the new input system made me do it...
            if (Mathf.Abs(lastDelta.x) > 0.001f || Mathf.Abs(lastDelta.y) > 0.001f)
                MoveAlongSplineHor(lastDelta.x);
            
            AnchorXZToSpline();
            
            if (rotateWithSpline)
                AnchorRotToSpline();
        }


        //Detta teleporterar objektet till den närmsta positionen längst med splinen.
        private void AnchorXZToSpline()
        {
            var position = rigidbody.position;
            Vector3 splinePos = currentSpline.path.GetClosestPointOnPath(position);
            
            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;
            
            rigidbody.MovePosition(splinePos);
        }

        private void AnchorRotToSpline()
        {
            float rot = rotOffset;
            var position = transform.position;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            Vector3 splineRot = currentSpline.path.GetRotationAtDistance(splineDist).eulerAngles;
            
            Vector3 offset = Vector3.zero;
            switch (rotAxis)
            {
                case RotAxis.X:
                    offset.x = rot;
                    break;
                case RotAxis.Y:
                    offset.y = rot;
                    break;
                case RotAxis.Z:
                    offset.z = rot;
                    break;
            }

            splineRot += offset;
            
            transform.rotation = Quaternion.Euler(splineRot);
        }

        
        
        
        
        //Flytta detta till ett externt script
        
        //Här tar vi den (horizontella riktningen * hastigheten) och flyttar spelaren längst splinen beroende på det.
        public void MoveAlongSplineHor(float horSpeed)
        {
            //Varför ska vi röra oss?
            if (horSpeed == 0) return;

            horSpeed *= Time.deltaTime * 10.0f;
            
            var position = rigidbody.position;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist + horSpeed, EndOfPathInstruction.Stop);
            
            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;
            
            rigidbody.MovePosition(splinePos);
        }

        //This is working kinda weird, you can't hold the button down?
        public void DebugTestMoveHor(InputAction.CallbackContext action)
        {
            Vector2 delta = action.ReadValue<Vector2>();
            lastDelta = delta;
        }
    }
}
