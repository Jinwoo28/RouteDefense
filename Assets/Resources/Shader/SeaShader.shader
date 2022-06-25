Shader "Custom/SeaShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("NomalMap",2D) = "Bump"{}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, float2(IN.uv_MainTex.x+_Time.y*0.5,IN.uv_MainTex.y))* _Color;
            float3 n = UnpackNormal(tex2D(_BumpMap, float2(IN.uv_BumpMap.x + _Time.y * 0.5, IN.uv_BumpMap.y)));
            o.Normal = float3(n.x*0.5,n.y*0.5,n.z);

            o.Albedo = c.rgb;

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a*0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
