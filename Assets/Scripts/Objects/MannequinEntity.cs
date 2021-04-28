using System;
using UnityEngine;

namespace Objects
{
    public class MannequinEntity : MonoBehaviour
    {
        public enum CallDuring
        {
            Update,
            FixedUpdate,
            LateUpdate
        }

        public GameObject objectToCopy;
        [Space]
        public bool copyPosition = true;
        public bool copyRotation = true;
        [Space]
        [Range(-100, 100)] public float yOffset = -30.0f;
        public CallDuring callDuring = CallDuring.FixedUpdate;
        public bool applyOffsetInEditor = false;

        private void OnValidate()
        {
            if (!applyOffsetInEditor) return;
            if (objectToCopy == null) return;

            if (copyPosition)
                transform.position = objectToCopy.transform.position + new Vector3(0, yOffset, 0);

            if (copyRotation)
                transform.rotation = objectToCopy.transform.rotation;
        }

        private void Update()
        {
            if (callDuring != CallDuring.Update) return;
        
            if (copyPosition)
                transform.position = objectToCopy.transform.position + new Vector3(0, yOffset, 0);

            if (copyRotation)
                transform.rotation = objectToCopy.transform.rotation;
        }

        private void FixedUpdate()
        {
            if (callDuring != CallDuring.FixedUpdate) return;
        
            if (copyPosition)
                transform.position = objectToCopy.transform.position + new Vector3(0, yOffset, 0);

            if (copyRotation)
                transform.rotation = objectToCopy.transform.rotation;
        }

        private void LateUpdate()
        {
            if (callDuring != CallDuring.LateUpdate) return;
        
            if (copyPosition)
                transform.position = objectToCopy.transform.position + new Vector3(0, yOffset, 0);

            if (copyRotation)
                transform.rotation = objectToCopy.transform.rotation;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, objectToCopy.transform.position);
        }
    }
}
