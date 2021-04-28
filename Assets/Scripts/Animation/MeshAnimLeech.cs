using UnityEngine;

namespace Animation
{
    public class MeshAnimLeech : MonoBehaviour
    {
        
        //Simple script, just fetch the animation data from another animator and apply it on this object
        // as well.
        
        public MeshAnimator host;
        public MeshFilter meshFilter;
        
        void Update()
        {
            LeechOffHost();
        }

        //Vi basically bara tar mesh och skalan på host och sätter det till vår egna.
        void LeechOffHost()
        {
            meshFilter.mesh = host.GetCurrentPlayingMesh();
            transform.localScale = host.GetCurrentAnimScale();
        }
    }
}
