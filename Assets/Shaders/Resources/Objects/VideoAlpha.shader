Shader "Custom/VideoAlpha" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_InterpolationRatio ("Interpolation Ratio", Float) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Cull off
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

			// from https://www.shadertoy.com/view/MsS3DW
			// by casty

			float getAlpha(float treshold, float4 c){
				// First Vlahos assumption: Gf <= a2Bf	
				return 1.0 - treshold*(c.g-1.2*(max(c.r,c.b)));
			}

			float4 despill(float4 c){
				/// Second Vlahos assumption: max (Gf - Bf,0) <= max(Bf - Rf, 0)
				float sub = max(c.g - lerp(c.b, c.r, 0.45), 0.0);
				c.g -= sub;

				/// 
				c.a -= smoothstep(0.25, 0.5, sub*c.a);

				//restore luminance (kind of, I slightly reduced the green weight)
				float luma = dot(c.rgb, float3(0.350, 0.587,0.164));
				c.r += sub*c.r*2.0*.350/luma;
				c.g += sub*c.g*2.0*.587/luma;
				c.b += sub*c.b*2.0*.164/luma;

				return c;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.a = clamp(getAlpha(8.0, col), 0.0, 1.0);
				col = despill(col);
				col.a *= (1. - _InterpolationRatio);
				return col;
			}
			ENDCG
		}
	}
}
