Shader "Custom/CircleTransparency"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        
        _CutoutRadius("Circle Radius", Range(0.1, 10.0)) = 3.0
        _Feather("Circle Edge Softness", Range(0.1, 3.0)) = 0.5
        _SeeThroughAlpha("Inside Circle Alpha", Range(0.0, 1.0)) = 0.2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ZTest LEqual
            
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            
            CBUFFER_START(CustomCircleTransparency)
                half _CutoutRadius;
                half _Feather;
                half _SeeThroughAlpha;
            CBUFFER_END

            uniform float4 _CutoutTargetWS1;
            uniform float4 _CutoutTargetWS2;

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"

            Varyings vert(Attributes input)
            {
                return LitPassVertex(input);
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                half4 finalColour;
                LitPassFragment(IN, finalColour);

                // Reconstruct the real 3D World Position of this pixel using internal matrix arrays
                float2 screenUV = IN.positionCS.xy / _ScreenParams.xy;
                float3 pixelWorldPos = ComputeWorldSpacePosition(screenUV, IN.positionCS.z, UNITY_MATRIX_I_VP);

                // Calculate straight-line 3D distances from this wall pixel to the impact positions
                float distToP1 = distance(pixelWorldPos, _CutoutTargetWS1.xyz);
                float distToP2 = distance(pixelWorldPos, _CutoutTargetWS2.xyz);

                // Sphere masks calculated directly in 3D world space
                float mask1 = smoothstep(_CutoutRadius, _CutoutRadius - _Feather, distToP1) * _CutoutTargetWS1.w;
                float mask2 = smoothstep(_CutoutRadius, _CutoutRadius - _Feather, distToP2) * _CutoutTargetWS2.w;
                
                float combinedMask = saturate(mask1 + mask2);

                // Fade out the alpha exclusively inside the intersection zone
                finalColour.a = lerp(1.0, _SeeThroughAlpha, combinedMask);

                return finalColour;
            }
            ENDHLSL
        }
    }
    Fallback "Universal Forward"
}
