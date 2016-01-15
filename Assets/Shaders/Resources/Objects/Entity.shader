Shader "Unlit/Entity"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", Range(0,1)) = 1
	}
	SubShader
	{
		Cull off
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Utils/foundation.cginc"
			#include "../Utils/Utils.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
        float3 viewDir : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _InterpolationRatio;
			float3 _HoleDirection;
			float _IsUniverseTransition;
			float _Alpha;
 
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
        ratio = lerp(dentDeScie(_InterpolationRatio), ratio, _IsUniverseTransition);

				fixed4 col = tex2D(_MainTex, i.uv);
				col.a *= 1. - ratio;
				col.a *= _Alpha;
				return col;
			}
			ENDCG
		}
	}
}
