using System;
using UnityEngine;

namespace Shaders
{
    public class RenderTextureSync : MonoBehaviour
    {
        public Camera mainCam;
        public RenderTexture secondCamRenderTex;
        public RenderTexture depthCamRenderTex;
        public float depthCamRenderScale = 0.5f;

        private void Awake()
        {
            if (mainCam == null) return;
            
             Vector2Int mainRes = new Vector2Int(mainCam.scaledPixelWidth, mainCam.scaledPixelHeight);

            if (secondCamRenderTex != null)
            {
                secondCamRenderTex.width = mainRes.x;
                secondCamRenderTex.height = mainRes.y;
            }
            
            if (depthCamRenderTex != null) // Mask is only rendered at half res since we're using a blur. Should be fine.
            {
                depthCamRenderTex.width = Mathf.FloorToInt((float)mainRes.x * depthCamRenderScale);
                depthCamRenderTex.height = Mathf.FloorToInt((float)mainRes.y * depthCamRenderScale);
            }
        }
    }
}