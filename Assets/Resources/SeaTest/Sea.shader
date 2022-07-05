Shader "Custom/Sea"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _BumpMap("BumpMap",2D) = "Bump"{}
        _Cube("Cube",Cube) = ""{}

        _RimPower("RimPower",Range(0,10)) = 3

            _AlphaPower("AlphaPower",Range(0,1)) = 0.5

            _NomalY("NomalY",Range(0,10)) = 0.02


    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alpha:fade vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        float _NomalY;

        void vert(inout appdata_full v)
        {
            float movement;
            movement = sin(abs((v.texcoord.x * 2 - 1)*12)+_Time.y)*0.02;
            movement += sin(abs((v.texcoord.y * 2 - 1) * 12) + _Time.y) * 0.02;
            v.vertex.y = movement* _NomalY;
        }

        sampler2D _MainTex;
    sampler2D _BumpMap;
    samplerCUBE _Cube;

    float _RimPower;
    float _AlphaPower;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 worldRefl;
            float3 viewDir;
            INTERNAL_DATA
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
            //Normal term
            float3 normal1 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + _Time.x * 0.1));
            float3 normal2 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap - _Time.x * 0.1));

            o.Normal = (normal1 + normal2) / 2;
            float3 refcolor = texCUBE(_Cube, WorldReflectionVector(IN, o.Normal));

            fixed4 c;
                c = tex2D(_MainTex, float2(IN.uv_MainTex.x - _Time.y * 0.02, IN.uv_MainTex.y)) * _Metallic;
            
            //o.Albedo = c.rgb;

            //rim term
            float rim = saturate(dot(o.Normal, IN.viewDir));
            rim = pow(1 - rim, _RimPower);
            //o.Alpha = 1;

            o.Emission = refcolor*rim*2+c.rgb;
            o.Alpha = saturate(rim+ _AlphaPower);
        }
        ENDCG
    }
    FallBack "Legacy Shaders/Transparent/Vertexlit"
}
