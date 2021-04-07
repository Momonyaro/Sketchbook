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
        public bool assignSplineAtAwake = true;
        public PathCreator currentSpline;

        private Vector2 lastDelta = Vector2.zero;

        private void Awake()
        {
            // Här så sätter vi splinen som objektet ska följa om den inte redan är satt.
            if (assignSplineAtAwake && currentSpline == null)
                currentSpline = FindObjectOfType<PathCreator>();
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
        }


        //Detta teleporterar objektet till den närmsta positionen längst med splinen.
        private void AnchorXZToSpline()
        {
            var position = transform.position;
            Vector3 splinePos = currentSpline.path.GetClosestPointOnPath(position);
            
            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;
            
            transform.position = splinePos;
        }

        //Här tar vi den (horizontella riktningen * hastigheten) och flyttar spelaren längst splinen beroende på det.
        public void MoveAlongSplineHor(float horSpeed)
        {
            //Varför ska vi röra oss?
            //if (horSpeed == 0) return;
            
            var position = transform.position;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist + horSpeed, EndOfPathInstruction.Stop);
            
            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;
            
            transform.position = splinePos;
        }

        //This is working kinda weird, you can't hold the button down?
        public void DebugTestMoveHor(InputAction.CallbackContext action)
        {
            Vector2 delta = action.ReadValue<Vector2>();
            lastDelta = delta;
        }
    }
}
