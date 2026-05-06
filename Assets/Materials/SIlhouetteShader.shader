Shader "Custom/SpriteTint"
{
    Properties
    {
        [MainTexture] _BaseMap ("Sprite Texture", 2D) = "white" {}
        [MainColor] _TintColor ("Tint Color", Color) = (1,0,0,1)
        _TintStrength ("Tint Strength", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "PreviewType" = "Plane"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "SpriteUnlit"
            Tags { "LightMode" = "Universal2D" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma prefer_hlslcc gles
            #pragma target 2.0
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            // Remove _BaseMap_ST from CBUFFER to avoid SRP Batcher issues
            // Instead, handle UV transformation manually
            CBUFFER_START(UnityPerMaterial)
                float4 _TintColor;
                float _TintStrength;
            CBUFFER_END

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Use UVs directly without TRANSFORM_TEX to avoid _ST properties
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half3 finalRGB = lerp(texColor.rgb, _TintColor.rgb, _TintStrength);
                return half4(finalRGB, texColor.a) * IN.color;
            }
            ENDHLSL
        }
    }
    
    // Fallback to built-in sprite shader for older pipelines
    FallBack "Sprites/Default"
}