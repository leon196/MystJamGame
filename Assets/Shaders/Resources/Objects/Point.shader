Shader "Custom/Point" {
	Properties {
		_MainTex ("Current World", 2D) = "white" {}
	}
	SubShader {
		ZWrite off
		Cull off
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
        float3 viewDir : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float3 _HoleDirection;
			float _IsSphereTransition;
			float _InterpolationRatio;
 
			v2f vert(appdata input) 
			{
				v2f output;

				float4x4 modelMatrix = _Object2World;
				output.viewDir = mul(modelMatrix, input.vertex).xyz - _WorldSpaceCameraPos;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);      
				output.uv = input.uv;
				return output;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
        // Sphere transition
        float ratio = dot(normalize(i.viewDir), _HoleDirection) * 0.5 + 0.5;
        ratio = step(ratio, _InterpolationRatio);

        // Select between fade in/out transition and sphere transition
        ratio = lerp(_InterpolationRatio, ratio, _IsSphereTransition);

				fixed4 col = tex2D(_MainTex, i.uv);
				col.a *= 1. - ratio;
				return col;
			}
			ENDCG
		}
	}
}
