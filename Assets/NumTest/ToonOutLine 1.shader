Shader "Custom/ToonOutLine"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OutLinePowerX("OutLinePowerZ",Range(-1,1)) = 0
        _OutLinePowerY("OutLinePowerY",Range(-1,1)) = 0
        _OutLinePowerZ("OutLinePowerZ",Range(-1,1)) = 0

        _OutLinePosX("OutLinePowerZ",Range(-1,1)) = 0
        _OutLinePosY("OutLinePowerY",Range(-1,1)) = 0
        _OutLinePosZ("OutLinePowerZ",Range(-1,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        cull front

        CGPROGRAM
        #pragma surface surf Nolight vertex:vert noshadow 

        #pragma target 3.0

         float _OutLinePowerX;
        float _OutLinePowerY;
        float _OutLinePowerZ;

        float _OutLinePosX;
        float _OutLinePosY;
        float _OutLinePosZ;
        void vert(inout appdata_full v)
        {
            v.vertex.xyz = float3(v.vertex.x+v.vertex.x * _OutLinePowerX+ _OutLinePosX,
                v.vertex.y + v.vertex.y * _OutLinePowerY+ _OutLinePosY,
                v.vertex.z + v.vertex.z * _OutLinePowerZ+ _OutLinePosZ);
        }


        struct Input
        {
            float4 color:COLOR;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
        }

        float4 LightingNolight(SurfaceOutput s, float3 lightDir, float atten) 
        {
            return float4(0,0,0,1);
        }

        ENDCG

        cull back
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            o.Alpha = c.a;
        }
        ENDCG


       cull front

       CGPROGRAM
       #pragma surface surf Standard fullforwardshadows

       #pragma target 3.0

       sampler2D _MainTex;
       

       struct Input
       {
           float2 uv_MainTex;
       };

       fixed4 _Color;

       void surf(Input IN, inout SurfaceOutputStandard o)
       {
           fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
           o.Albedo = c.rgb;

           o.Alpha = c.a;
       }
       ENDCG


    }
    FallBack "Diffuse"
}
