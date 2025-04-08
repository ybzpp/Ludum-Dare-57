Shader "Custom/TopMostRendererAdvanced"
{
    Properties
    {
        [Header(Main Settings)]
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        [Toggle(_ALPHACLIP)] _AlphaClip ("Alpha Clip", Float) = 0
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5

        [Header(Rim Effect)]
        [Toggle(_RIM_EFFECT)] _RimToggle ("Enable Rim", Float) = 0
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0.1, 10)) = 2
        _RimIntensity ("Rim Intensity", Range(0, 1)) = 0.2
        _RimThreshold ("Rim Threshold", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Overlay+5000"
            "ForceNoShadowCasting" = "True"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHACLIP
            #pragma shader_feature _RIM_EFFECT

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Cutoff;

            #if _RIM_EFFECT
            fixed4 _RimColor;
            float _RimPower;
            float _RimIntensity;
            float _RimThreshold;
            #endif

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                #if _ALPHACLIP
                clip(col.a - _Cutoff);
                #endif

                #if _RIM_EFFECT
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float rim = 1.0 - saturate(dot(normalize(i.normal), viewDir));
                rim = pow(rim, _RimPower);
                rim = smoothstep(_RimThreshold, 1.0, rim) * _RimIntensity;
                col.rgb += rim * _RimColor.rgb * col.a;
                #endif

                return col;
            }
            ENDCG
        }
    }
    FallBack "Hidden/InternalErrorShader"
    CustomEditor "ShaderEditor.TopMostRendererAdvancedEditor"
}