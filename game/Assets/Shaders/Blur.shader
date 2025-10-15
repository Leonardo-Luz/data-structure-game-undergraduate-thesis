Shader "UI/Blur"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Size ("Blur Size", Range(0, 10)) = 2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Size;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
                float2 offset = _Size * _MainTex_TexelSize.xy;

                // 9-sample blur
                col += tex2D(_MainTex, i.uv + float2(-offset.x, -offset.y));
                col += tex2D(_MainTex, i.uv + float2(0, -offset.y));
                col += tex2D(_MainTex, i.uv + float2(offset.x, -offset.y));
                col += tex2D(_MainTex, i.uv + float2(-offset.x, 0));
                col += tex2D(_MainTex, i.uv);
                col += tex2D(_MainTex, i.uv + float2(offset.x, 0));
                col += tex2D(_MainTex, i.uv + float2(-offset.x, offset.y));
                col += tex2D(_MainTex, i.uv + float2(0, offset.y));
                col += tex2D(_MainTex, i.uv + float2(offset.x, offset.y));

                col /= 9.0;
                return col;
            }
            ENDCG
        }
    }
}
