Shader "Water/Ocean_01_SpriteCompatible" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FoamTexture ("Foam Texture", 2D) = "white" {}
        _Addittionalfoam ("Addittional foam", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        [Space(10)]
        _WaterColor ("Water Color", Color) = (0.4926471,0.6011156,1,1)
        _WaterOpacity ("Water Opacity", Range(0, 1)) = 0.5
        _FoamOpacity ("Foam Opacity", Range(0, 2)) = 2
        _WaterDepth ("Water Depth", Float ) = 10
        _EdgeFoamLevel ("Edge Foam Level", Float ) = 2
        _EdgeFoamLevelDestination ("Edge Foam Level Destination", Float ) = 3
        _EdgeFoamPower ("Edge Foam Power", Range(0, 1)) = 1
        _WaterOrientation ("Water Orientation", Range(0, 4)) = 2
        _Noiselevel ("Noise level", Range(0, 0.12)) = 0.1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }
        LOD 100
        Pass {
            Name "FORWARD"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _FoamTexture; float4 _FoamTexture_ST;
            sampler2D _Addittionalfoam; float4 _Addittionalfoam_ST;
            sampler2D _Noise; float4 _Noise_ST;
            float4 _WaterColor;
            float _WaterOpacity;
            float _FoamOpacity;
            float _WaterDepth;
            float _EdgeFoamLevel;
            float _EdgeFoamLevelDestination;
            float _EdgeFoamPower;
            float _WaterOrientation;
            float _Noiselevel;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float time = _Time.y;

                float2 rotUV = i.uv - 0.5;
                float cosA = cos(_WaterOrientation);
                float sinA = sin(_WaterOrientation);
                rotUV = float2(
                    rotUV.x * cosA - rotUV.y * sinA,
                    rotUV.x * sinA + rotUV.y * cosA
                ) + 0.5;

                float4 baseColor = tex2D(_MainTex, rotUV + float2(0, time * 0.05));
                float4 foam = tex2D(_FoamTexture, rotUV + float2(0, time * 0.02)) * _FoamOpacity;
                float4 noise = tex2D(_Noise, rotUV);
                float4 additionalFoam = tex2D(_Addittionalfoam, rotUV + float2(0, time * 0.01));

                float3 waterBlend = lerp(baseColor.rgb * additionalFoam.rgb, foam.rgb, noise.r);
                waterBlend = max(waterBlend, float3(0.05, 0.05, 0.05)); // 최소 밝기 제한

                float alpha = saturate(_WaterOpacity + noise.r);
                return float4(waterBlend * _WaterColor.rgb, alpha);
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}