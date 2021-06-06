using System;
using UnityEngine;

namespace Shaders
{
    public class RenderTextureSync : MonoBehaviour
    {
        public Camera mainCam;
        public RenderTexture secondCamRenderTex;
        public RenderTexture depthCamRenderTex;
        [Range(0, 1)] public float depthCamRenderScale = 0.5f;

        private void Start()
        {
            if (mainCam == null) return;
            
             Vector2Int mainRes = new Vector2Int(mainCam.scaledPixelWidth, mainCam.scaledPixelHeight);

            if (secondCamRenderTex != null)
            {
                secondCamRenderTex.Release();
                secondCamRenderTex.width = mainRes.x;
                secondCamRenderTex.height = mainRes.y;
                if (!secondCamRenderTex.Create())
                    Debug.Log("Failed to create new rendertex for: " + secondCamRenderTex.name);
            }
            
            if (depthCamRenderTex != null) // Mask is only rendered at half res since we're using a blur. Should be fine.
            {
                depthCamRenderTex.Release();
                depthCamRenderTex.width = Mathf.FloorToInt((float)mainRes.x * depthCamRenderScale);
                depthCamRenderTex.height = Mathf.FloorToInt((float)mainRes.y * depthCamRenderScale);
                if (!depthCamRenderTex.Create())
                    Debug.Log("Failed to create new rendertex for: " + depthCamRenderTex.name);
            }
        }
    }
}