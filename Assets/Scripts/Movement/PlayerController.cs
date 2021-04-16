using System;
using Config;
using PathCreation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerController : MonoBehaviour
    {
        [Range(0, 5)] public float moveSpeed = 0.833f;
        [HideInInspector]
        public float pushSpeedMultiplier = 1.0f;

        public float pushOffForce = 50.0f;
        [Min(1.0f)] public float jumpSpeed = 2.0f;
        [Min(1.0f)] public float fallSpeed = 2.7f;

        [Tooltip("If these objects intersect with the ground, the player will be able to perform a jump")]
        public Transform GroundCheckLeft = null, GroundCheckRight = null;

        [Header("Spline Walker Settings")] 
        public bool assignSplineAtAwake = false;
        public SplineWalker splineWalker;
        [Header("Debug Settings")] 
        public bool drawGizmos = false;
        [Range(-1, 1)] public float groundedRayLength = 1.0f;
        
        private new Rigidbody rigidbody;
        [HideInInspector]
        public Vector2 lastDelta = Vector2.zero;

        private void OnValidate()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            if (assignSplineAtAwake)
                splineWalker = GetComponent<SplineWalker>();
            
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            //Kolla om movement deltan är större än en liten gräns
            if (Mathf.Abs(lastDelta.x) > 0.001f)
            {
                MoveAlongSplineHor(lastDelta.x);
            }

            if (rigidbody.velocity.y < 0) // Falling
            {
                Vector3 velocity = rigidbody.velocity;
                velocity.y += Physics.gravity.y * (fallSpeed - 1.0f);
                rigidbody.velocity = velocity;
            }
            else if (rigidbody.velocity.y > 0) // Jumping
            {
                Vector3 velocity = rigidbody.velocity;
                velocity.y += Physics.gravity.y * (jumpSpeed - 1.0f);
                rigidbody.velocity = velocity;
            }
        }


        //Här tar vi den (horizontella riktningen * hastigheten) och flyttar spelaren längst splinen beroende på det.
        public void MoveAlongSplineHor(float horSpeed)
        {
            //Varför ska vi röra oss?
            if (horSpeed == 0) return;

            horSpeed *= Time.deltaTime * 10.0f * pushSpeedMultiplier;
            
            var position = rigidbody.position;

            PathCreator currentSpline = splineWalker.currentSpline;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist + horSpeed, EndOfPathInstruction.Stop);
            
            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;
            
            rigidbody.MovePosition(splinePos);
        }
        
        //Här så gör vi så att karaktären kan hoppa. (ska vi använda AddForce?)
        public void JumpUsingForce()
        {
            if (!IsGrounded()) return;
            rigidbody.AddForce(new Vector3(0.0f, pushOffForce, 0.0f), ForceMode.Impulse);
            splineWalker.AnchorXZToSpline();
        }

        //Kolla om vi står på marken med ray casting
        public bool IsGrounded()
        {
            if (GroundCheckLeft != null && GroundCheckRight != null)
            {
                var position = rigidbody.position + new Vector3(0.0f, 0.25f, 0.0f);
                if (Physics.Linecast(position, GroundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
                    || Physics.Linecast(position, GroundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
                    return true;
            }

            return false;
        }


        #region Gizmos

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            Gizmos.color = Color.green;

            if (GroundCheckLeft != null && GroundCheckRight != null)
            {
                var position = rigidbody.position + new Vector3(0.0f, 0.25f, 0.0f);
                Gizmos.DrawLine(position, GroundCheckLeft.position);
                Gizmos.DrawLine(position, GroundCheckRight.position);
            }
        }

        #endregion

        #region InputCalls

        //This is working kinda weird, you can't hold the button down?
        public void InputActionMoveHor(InputAction.CallbackContext action)
        {
            Vector2 delta = action.ReadValue<Vector2>();
            lastDelta = delta;
        }

        public void InputActionJump(InputAction.CallbackContext action)
        {
            if (action.canceled) return;
            JumpUsingForce();
        }
        
        #endregion
    }
}
