Shader "Custom/MeltingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Albedo Color", Color) = (1,1,1,1)

        _MeltAmount ("Melt Amount", Range(0,1)) = 0
        _NoiseScale ("Noise Scale", Float) = 5
        _MeltStrength ("Melt Strength", Float) = 0.5

        _DripStrength ("Drip Strength", Float) = 0.6
        _DripScale ("Drip Scale", Float) = 3
        _DripSpeed ("Drip Speed", Float) = 1

        _UVFlowStrength ("UV Flow", Float) = 0.2

        _MeltTint ("Melt Tint Color", Color) = (0.2,0.2,0.2,1)
        _TintStrength ("Tint Strength", Range(0,1)) = 0.7
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque"}

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;

            float4 _Color;
            float _MeltAmount;
            float _NoiseScale;
            float _MeltStrength;

            float _DripStrength;
            float _DripScale;
            float _DripSpeed;

            float _UVFlowStrength;

            float4 _MeltTint;
            float _TintStrength;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float meltMask : TEXCOORD1;
            };

            // ---------- NOISE ----------
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1,311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i);
                float b = hash(i + float2(1,0));
                float c = hash(i + float2(0,1));
                float d = hash(i + float2(1,1));

                float2 u = f*f*(3.0-2.0*f);

                return lerp(a,b,u.x) +
                       (c-a)*u.y*(1.0-u.x) +
                       (d-b)*u.x*u.y;
            }

            // ---------- VERTEX STAGE ----------
            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // posición en mundo
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                float3 gravityDir = normalize(float3(0,-1,0));

                // ruido espacial en global
                float n = noise(worldPos.xz * _NoiseScale);

                // Altura relativa
                float3 objectOrigin = TransformObjectToWorld(float3(0,0,0));

                // Máscara de altura por gravedad
                float heightAlongGravity =
                    dot(worldPos - objectOrigin, -gravityDir);

                float heightMask = saturate(heightAlongGravity);

                float melt = n * _MeltAmount * _MeltStrength * heightMask;

                float dripNoise = noise(worldPos.xz * _DripScale + _Time.y * _DripSpeed);
                dripNoise = saturate(dripNoise - 0.6) * 2.5;
                
                float drip =
                    pow(dripNoise,2.5) *
                    _DripStrength *
                    _MeltAmount *
                    heightMask;

                melt += drip;

               // mover vértices en dirección elegida
                worldPos += gravityDir * melt;

                // transformar en de nuevo a object space
                OUT.positionHCS =
                    TransformWorldToHClip(worldPos);

                OUT.uv = TRANSFORM_TEX(IN.uv,_MainTex);

                // guardamos cuánto se ha derretido para el fragment shader
                OUT.meltMask = melt;

                return OUT;
            }

            // ---------- FRAGMENT ----------
            half4 frag(Varyings IN) : SV_Target
            {
                float meltFactor = saturate(IN.meltMask * 5);

                float2 uv = IN.uv;

                // UV FLOW
                uv.y -= meltFactor * _UVFlowStrength * _MeltAmount;

                // Albedo
                float4 tex = SAMPLE_TEXTURE2D(_MainTex,
                                              sampler_MainTex,
                                              uv);
                                              
                float3 color = tex.rgb * _Color.rgb;

                color = lerp(
                    color,
                    color * _MeltTint.rgb,
                    meltFactor * _TintStrength
                );

                return float4(color, tex.a);
            }
            ENDHLSL
        }
    }
}