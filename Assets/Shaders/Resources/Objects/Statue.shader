Shader "Custom/Statue" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_TeleportationRatio ("Teleportation Ratio", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0
		#include "../Utils/foundation.cginc"

		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _TeleportationRatio;

		struct Input {
			float2 uv_MainTex;
			float3 normal;
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.normal = v.normal;

			v.vertex.xyz = rotateY(v.vertex.xyz, _TeleportationRatio * (v.vertex.y * 100.));
			v.vertex.xyz = lerp(v.vertex.xyz, float3(0., v.vertex.y, 0.), _TeleportationRatio);
			v.vertex.y *= (1. + _TeleportationRatio);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = lerp(c.rgb, IN.normal * 0.5 + 0.5, _TeleportationRatio);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
