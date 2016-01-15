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
			#include "../Utils/Dither.cginc"
			#include "../Utils/Utils.cginc"
			
			sampler2D _MainTex;
			sampler2D _UITexture;
			sampler2D _ItemsTexture;
			sampler2D _ItemsNextTexture;
			float _InterpolationRatio;

			fixed4 frag (v2f_img i) : SV_Target {
				float2 uv = i.uv;
				fixed4 background = tex2D(_MainTex, uv);
				// background = dither8x8(uv * _ScreenParams.xy, background);
				// uv.y = 1.0 - uv.y;
				fixed4 ui = tex2D(_UITexture, uv);
				fixed4 items = lerp(tex2D(_ItemsTexture, uv), tex2D(_ItemsNextTexture, uv), _InterpolationRatio);
				fixed4 color = lerp(background, items, items.a);//step(0.0, Luminance(items)));
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
