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

			float ratio = _TeleportationRatio;

			v.vertex.xyz = rotateY(v.vertex.xyz, ratio * (v.vertex.y * 100.));
			// v.vertex.xz *= lerp(1., 0., ratio);
			v.vertex.xz *= lerp(1., (1. - smoothstep(0.5, 1., ratio)) / max(v.vertex.y * 100., 1.), ratio);
			v.vertex.y *= (1. + ratio);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			float ratio = _TeleportationRatio;

			o.Albedo = lerp(c.rgb, IN.normal * 0.5 + 0.5, ratio);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
