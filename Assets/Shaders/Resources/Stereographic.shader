Shader "Hidden/Stereographic" {
    Properties  {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader   {
        Cull Off ZWrite Off ZTest Always
        Pass        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "foundation.cginc"
            #include "Dither.cginc"
            #include "Utils.cginc"
            
            sampler2D _MainTex;
            sampler2D _Equirectangle;
            float _InputHorizontal;
            float _InputVertical;
            float _InputDepth;

            fixed4 frag (v2f_img i) : SV_Target {

                float2 rads = float2(PI * 2., PI);

                float timing = sin(_Time * 20.0) * 0.5 + 0.5;
                float scale = 1.0 + smoothstep(0.25, 1.0, timing) * 20.0;
                // float scale = 1.0 + _InputDepth * 4.0;

                float2 pnt = (i.uv - float2(.5, .5)) * scale;
                pnt.x *= _ScreenParams.x / _ScreenParams.y;

                // Project to Sphere
                float x2y2 = pnt.x * pnt.x + pnt.y * pnt.y;
                float3 sphere_pnt = float3(2. * pnt, x2y2 - 1.) / (x2y2 + 1.);
                // sphere_pnt *= transform;
                sphere_pnt = rotateY(sphere_pnt, PI);

                float rX = smoothstep(0.25, 0.75, 1.0 - timing);
                sphere_pnt = rotateX(sphere_pnt, -rX * PI * 0.5);
                // sphere_pnt = rotateY(sphere_pnt, _InputHorizontal);
                // sphere_pnt = rotateX(sphere_pnt, -_InputVertical);

                float r = length(sphere_pnt);
                float lon = atan2(sphere_pnt.y, sphere_pnt.x) + PI;
                float lat = acos(sphere_pnt.z / r);

                float t = _Time * 4.0;
                float2 uv = fmod(abs(float2(lon + t, lat) / rads), 1.0);

                fixed4 color = tex2D(_Equirectangle, uv);
                /*
                float2 uv = i.uv;
                fixed4 background = tex2D(_MainTex, uv);
                uv.y = 1.0 - uv.y;
                fixed4 ui = tex2D(_UITexture, uv);
                fixed4 items = tex2D(_ItemsTexture, uv);
                fixed4 color = lerp(background, items, items.a);
                color = lerp(color, ui, ui.a);
                */
                // color = clamp(color + ui, 0., 1.);
                // fixed4 color = clamp(background + ui, 0., 1.);
                // color = clamp(color + ui * lerp(1., -1., Luminance(color)), 0., 1.);
                return color;
            }
            ENDCG
        }
    }
}