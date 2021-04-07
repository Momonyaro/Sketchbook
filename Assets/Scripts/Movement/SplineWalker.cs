using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;
using PathCreation;

namespace Movement
{
    
    // Denna klassen ansvarar för att anknyta objekt till kartans spline.
    
    public class SplineWalker : MonoBehaviour
    {
        public bool assignSplineAtAwake = true;
        public PathCreator currentSpline;

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

        //Detta teleporterar objektet till den närmsta positionen längst med splinen.
        private void AnchorXZToSpline()
        {
            var position = transform.position;
            Vector3 splinePos = currentSpline.path.GetClosestPointOnPath(position);
            
            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;
            
            transform.position = splinePos;
        }
    }
}
