Shader "Unlit/World" {
	Properties {
		_MainTex ("Current World", 2D) = "white" {}
		_NextTex ("Next World", 2D) = "white" {}
		_InterpolationRatio ("Interpolation Ratio", Float) = 0
	}
	SubShader {
		Cull front
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			sampler2D _NextTex;
			float4 _MainTex_ST;
			float _InterpolationRatio;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.normal = -v.normal;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				o.uv.x = 1.0 - o.uv.x;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = lerp(tex2D(_MainTex, i.uv), tex2D(_NextTex, i.uv), _InterpolationRatio);
				return col;
			}
			ENDCG
		}
	}
}
