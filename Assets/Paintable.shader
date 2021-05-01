Shader "Unlit/Paintable" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset] _MaskTexture ("Texture", 2D) = "black" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _PaintTexture;
            sampler2D _MaskTexture;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float NdotL = dot(_WorldSpaceLightPos0, i.normal);
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed4 paint = tex2D(_PaintTexture, i.uv);
                //float mask = saturate(paint.r + paint.g + paint.b);
                //float mask = max(paint.r, max(paint.g, paint.b));
                float mask = tex2D(_MaskTexture, i.uv).r;

                col.rgb = lerp(col.rgb, paint, mask);

                col.rgb *= NdotL * _LightColor0.xyz;

                return col;
            }
            ENDCG
        }
    }
}
