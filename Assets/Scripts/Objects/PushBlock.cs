using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PathCreation;
using Config;

namespace Movement
{
    public class PushBlock : MonoBehaviour
    {
        [Tooltip("The speed the block gets pushed or pulled a\n" +
            "SHOULD BE THE SAME AS THE MOVEMENT SPEED OF THE PLAYER WHILE THEY ARE PUSHING/PULLING (default movement speed right now)")]
        public float blockSpeed;

        bool canBePushed, pushing;
        Rigidbody rb;
        SplineWalker splineWalker;
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
            // Matchar spelarens movement, dålig lösning men det fungerar antar jag...
            if (canBePushed && pushing)
                MoveAlongSplineHor(lastDelta.x);
        }

        // Just nu verkar inte spelarens speed påverkas av en public speed variabel, så den är basically identisk till player controller
        public void MoveAlongSplineHor(float horSpeed)
        {
            if (horSpeed == 0) return;

            horSpeed *= Time.deltaTime * 10.0f;

            var position = rb.position;

            PathCreator currentSpline = splineWalker.currentSpline;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist + horSpeed, EndOfPathInstruction.Stop);

            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;

            rb.MovePosition(splinePos);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                canBePushed = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
                canBePushed = false;
        }

        // Kommer ändra så att input sitter i ett spelarskript

        #region InputCalls

        public void InputActionMoveHor(InputAction.CallbackContext action)
        {
            Vector2 delta = action.ReadValue<Vector2>();
            lastDelta = delta;
        }

        public void InputActionGrab(InputAction.CallbackContext action)
        {
            pushing = action.ReadValueAsButton();
        }

        #endregion
    }
}
