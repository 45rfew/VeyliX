Shader "Custom/Starfield"
{
    Properties
    {
        _TwinkleSpeed ("Twinkle Speed", Range(0, 10)) = 2.0
        _TwinkleAmount ("Twinkle Amount", Range(0, 1)) = 0.4
        _GlowSoftness ("Glow Softness", Range(0.1, 4)) = 1.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Blend One One          // additive: stars only ever brighten the background
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv         : TEXCOORD0; // quad corner, -1..1
                float2 uv2        : TEXCOORD1; // x = star size, y = twinkle phase seed
                float4 color      : COLOR;     // rgb = star color, a = twinkle-enabled random seed
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 corner      : TEXCOORD0;
                float4 color       : COLOR;
            };

            float _TwinkleSpeed;
            float _TwinkleAmount;
            float _GlowSoftness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // World-space star center
                float3 centerWS = TransformObjectToWorld(IN.positionOS);

                // Billboard: expand the quad toward camera right/up in world space
                float3 camRightWS = UNITY_MATRIX_V[0].xyz;
                float3 camUpWS    = UNITY_MATRIX_V[1].xyz;

                float size = IN.uv2.x;
                float3 offset = (camRightWS * IN.uv.x + camUpWS * IN.uv.y) * size;

                float3 posWS = centerWS + offset;
                OUT.positionHCS = TransformWorldToHClip(posWS);
                OUT.corner = IN.uv;

                // Twinkle: vary brightness per-star over time using its phase seed
                float twinklePhase = IN.uv2.y;
                float twinkle = 1.0;
                if (_TwinkleAmount > 0.0)
                {
                    twinkle = 1.0 - _TwinkleAmount * 0.5 * (1.0 + sin(_Time.y * _TwinkleSpeed + twinklePhase * 6.2831));
                }

                OUT.color = float4(IN.color.rgb * twinkle, 1.0);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // Soft circular falloff so each star is a glowing dot, not a hard square
                float dist = length(IN.corner);
                float falloff = saturate(1.0 - dist);
                falloff = pow(falloff, _GlowSoftness);

                float3 col = IN.color.rgb * falloff;
                return float4(col, falloff);
            }
            ENDHLSL
        }
    }
}
