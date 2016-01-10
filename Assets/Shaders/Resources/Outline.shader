Shader "Hidden/Outline" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			half4 frag (v2f_img i) : COLOR
			{
				// float depth = tex2D(_CameraDepthTexture, i.uv).r;
				// depth = Linear01Depth(depth);
				// fixed4 color = half4(depth, depth, depth, 1.0);
				fixed4 color = tex2D(_MainTex, i.uv);
				return color;
			}
			ENDCG
		}
	}
}
