
Shader "Mobile/MyShader" {
Properties {
        _Color("Color", Color) = (0,0,0,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
     //    _BumpMap("Normalmap", 2D) = "bump" {}
      //   _Glossiness("Smoothness", Range(0,2)) = 0.5
     //  _Cube("Reflection Cubemap", Cube) = "_Skybox" {}
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#pragma surface surf Lambert forwardadd
      sampler2D _MainTex;
      float4 _Color;
     // sampler2D _BumpMap;
    //  half _Glossiness;
   //   samplerCUBE _Cube;
    //  UNITY_INSTANCING_BUFFER_START(GPUInstancedOpaque)
     //     UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
//#define _Color_arr GPUInstancedOpaque
     //     UNITY_INSTANCING_BUFFER_END(GPUInstancedOpaque)

struct Input {
    float2 uv_MainTex;
        //  float2 uv_Normal;
        //  float3 worldRefl;
       //   INTERNAL_DATA
      };

void surf (Input IN, inout SurfaceOutput o) {
  //  float4 _Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
  //  o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_Normal));
  //  float3 em = texCUBE(_Cube, WorldReflectionVector(IN, o.Normal)).rgb * _Glossiness;
  //  o.Emission = em;
     }         
ENDCG
}

Fallback "Legacy Shaders/Diffuse"
}