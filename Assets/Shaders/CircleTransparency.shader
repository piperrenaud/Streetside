Shader "Custom/CircleTransparency"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        
        _CutoutRadius("Circle Radius", Range(0.0, 0.5)) = 0.15
        _Feather("Circle Edge Softness", Range(0.01, 0.2)) = 0.03
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

            uniform float4 _PlayerOneScreenPos;
            uniform float4 _PlayerTwoScreenPos;
            uniform float _PlayerOneOccluded;
            uniform float _PlayerTwoOccluded;

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"

            Varyings vert(Attributes input)
            {
                return LitPassVertex(input);
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                half4 finalColour;
                LitPassFragment(IN, finalColour);
                float2 screenUV = IN.positionCS.xy / _ScreenParams.xy;
                float aspectRatio = _ScreenParams.x / _ScreenParams.y;
                float2 aspcetCorrectedUV = float2(screenUV.x * aspectRatio, screenUV.y);
                float2 p1Target = float2(_PlayerOneScreenPos.x * aspectRatio, _PlayerOneScreenPos.y);
                float2 p2Target = float2(_PlayerTwoScreenPos.x * aspectRatio, _PlayerTwoScreenPos.y);

                float distToP1 = distance(aspcetCorrectedUV, p1Target);
                float distToP2 = distance(aspcetCorrectedUV, p2Target);

                float circle1 = smoothstep(_CutoutRadius, _CutoutRadius - _Feather, distToP1) * _PlayerOneOccluded;
                float circle2 = smoothstep(_CutoutRadius, _CutoutRadius - _Feather, distToP2) * _PlayerTwoOccluded;

                float combinedMask = saturate(circle1 + circle2);

                finalColour.a = lerp(1.0, _SeeThroughAlpha, combinedMask);

                return finalColour;
            }
            ENDHLSL
        }
    }
    Fallback "Universal Forward"
}
