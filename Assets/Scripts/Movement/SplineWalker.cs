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
        
        public bool assignSplineAtAwake = true;
        public PathCreator currentSpline;
        public bool rotateWithSpline = true;
        public float rotOffset = 90;
        public RotAxis rotAxis = RotAxis.Y;

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

        private void FuxedUpdate()
        {
            AnchorXZToSpline();
            
            if (rotateWithSpline)
                AnchorRotToSpline();
        }


        //Detta teleporterar objektet till den närmsta positionen längst med splinen.
        public void AnchorXZToSpline()
        {
            var position = rigidbody.position;
            Vector3 splinePos = currentSpline.path.GetClosestPointOnPath(position);
            
            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;
            
            rigidbody.MovePosition(splinePos);
        }

        public void AnchorRotToSpline()
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
    }
}
