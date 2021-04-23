using System;
using UnityEngine;

namespace Shaders
{
    public class RenderShaderToScreen : MonoBehaviour
    {
        public Material screenMat;
        
        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, screenMat);
        }
    }
}
