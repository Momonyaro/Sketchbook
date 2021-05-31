Shader "Custom/DepthSample"
{
    Properties
    {
        _MainTex      ("Texture", 2D) = "white" {}
        _SecondCamTex ("Color World Texture", 2D) = "white" {}
        _MaskTex      ("Depth Texture", 2D) = "black" {}
        _OverlayTex   ("Overlay", 2D) = "black" {}
        
        _OverlayOpacity ("Overlay Opacity", Range(0.0, 1.0)) = 0.4
        _GaussAmountX ("Blur Amount X", Range(0.0, 0.1)) = 0.05
        _GaussAmountY ("Blur Amount Y", Range(0.0, 0.1)) = 0.05
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "HLSLSupport.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_DEPTH(o.depth);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _SecondCamTex;
            sampler2D _MaskTex;
            sampler2D _OverlayTex;

            float _OverlayOpacity;
            float _GaussAmountX;
            float _GaussAmountY;
            
            fixed4 gaussianBlurDepth(float2 uv)
            {
                float3 kernel[] =
                {
                    float3( 0.0, 0.0, 0.2006 ),
                    float3( 1.0, 0.0, 0.1770 ),
                    float3(-1.0, 0.0, 0.1770 ),
                    float3( 2.0, 0.0, 0.1216 ),
                    float3(-2.0, 0.0, 0.1216 ),
                    float3( 3.0, 0.0, 0.0651 ),
                    float3(-3.0, 0.0, 0.0651 ),
                    float3( 4.0, 0.0, 0.0271 ),
                    float3(-4.0, 0.0, 0.0271 ),
                    float3( 5.0, 0.0, 0.0088 ),
                    float3(-5.0, 0.0, 0.0088 ),
                };
                
                fixed4 tempX = fixed4(0.0, 0.0, 0.0, 1.0);
                fixed4 tempY = fixed4(0.0, 0.0, 0.0, 1.0);

                //return tex2D(_MaskTex, uv).r;
                
                //Sample and mix X-axis

                for (int i = 0; i < 11; i++)
                {
                    tempX += tex2D(_MaskTex, uv + float2(kernel[i].x * _GaussAmountX, kernel[i].y)) * kernel[i].z;
                }
                
                for (int i = 0; i < 11; i++)
                {
                    tempY += tex2D(_MaskTex, uv + float2(kernel[i].y, kernel[i].x * _GaussAmountY)) * kernel[i].z;
                }
                
                //Finally blend the two together
                return 0.5 * (tempX + tempY);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,1);
                fixed4 mask = gaussianBlurDepth(i.uv);
                
                //return mask; // For Debugging

                fixed4 sampleMain = tex2D(_MainTex, i.uv);
                fixed4 sampleSecond = tex2D(_SecondCamTex, i.uv);
                fixed4 sampleOverlay = tex2D(_OverlayTex, i.uv);

                col = lerp(sampleMain, sampleSecond, mask.r);
                col = lerp(col, col * sampleOverlay, _OverlayOpacity);
                
                return col;
            }
            ENDCG
        }
    }
}
