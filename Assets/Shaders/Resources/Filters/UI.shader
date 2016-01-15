Shader "Hidden/UI" {
	Properties 	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader 	{
		Cull Off ZWrite Off ZTest Always
		Pass 		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "../Utils/foundation.cginc"
			#include "../Utils/Utils.cginc"
			
			sampler2D _MainTex;
			sampler2D _UITexture;
			sampler2D _PanoramaNextTexture;
			float _InterpolationRatio;
			float _IsSphereTransition;

			fixed4 frag (v2f_img i) : SV_Target {
				float2 uv = i.uv;
				fixed4 panorama = tex2D(_MainTex, uv);
				fixed4 panoramaNext = tex2D(_PanoramaNextTexture, uv);
				fixed4 ui = tex2D(_UITexture, uv);
				fixed4 color = lerp(panorama, panoramaNext, lerp(_InterpolationRatio, 0., _IsSphereTransition));
				color = lerp(color, ui, ui.a);
				// color = clamp(color + ui, 0., 1.);
				// fixed4 color = clamp(background + ui, 0., 1.);
				// color = clamp(color + ui * lerp(1., -1., Luminance(color)), 0., 1.);
				return color;
			}
			ENDCG
		}
	}
}
