Shader "Hidden/Planet" {
    Properties  {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader   {
        Cull Off ZWrite Off ZTest Always
        Pass        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "../Utils/foundation.cginc"
            #include "../Utils/Dither.cginc"
            #include "../Utils/Utils.cginc"
            #include "../Utils/Easing.cginc"
            
            sampler2D _MainTex;
            sampler2D _Equirectangle;
            float _InputHorizontal;
            float _InputVertical;
            float _InputDepth;
            float _InterpolationRatio;

            // from https://github.com/notlion/streetview-stereographic

            fixed4 frag (v2f_img i) : SV_Target {

                float2 rads = float2(PI * 2., PI);

                float ratio = _InterpolationRatio;
                // float ratio = sin(_Time * 5.0) * 0.5 + 0.5;
                // float ratio = 1.;//fmod(_Time * 5.0, 1.0);
                float t = 1.0 - smoothstep(0., 0.9, ratio);
                float scale = 0.65 + easeInCirc(t, 0.0, 1.0, 1.0) * 10.0;//t * 5.0;//
                // float scale = 1.0 + _InputDepth * 4.0;

                float2 pnt = (i.uv - float2(.5, .5)) * scale;
                pnt.x *= _ScreenParams.x / _ScreenParams.y;

                // Project to Sphere
                float x2y2 = pnt.x * pnt.x + pnt.y * pnt.y;
                float3 sphere_pnt = float3(2. * pnt, x2y2 - 1.) / (x2y2 + 1.);

                sphere_pnt = rotateY(sphere_pnt, PI);
                float rX = smoothstep(0., .9, ratio);
                sphere_pnt = rotateX(sphere_pnt, rX * PI * 0.5);
                // sphere_pnt = rotateY(sphere_pnt, _InputHorizontal);
                // sphere_pnt = rotateX(sphere_pnt, -_InputVertical);

                float r = length(sphere_pnt);
                float lon = atan2(sphere_pnt.y, sphere_pnt.x) + PI;
                float lat = acos(sphere_pnt.z / r);

                float rot = (1.0 - smoothstep(0.0, 0.95, ratio)) * 4.0;
                float2 uv = fmod(abs(float2(lon + rot, lat) / rads), 1.0);
                uv.y *= 1. + (0.2 * (1.0 - ratio));

                fixed4 planet = tex2D(_Equirectangle, uv);
                planet.rgb *= step(uv.y, 1.0);
                planet.rgb *= 1. - smoothstep(0.9, 1.0, uv.y);
                fixed4 background = tex2D(_MainTex, i.uv);

                float a = smoothstep(0.0, 0.25, ratio) - smoothstep(0.85, 1.0, ratio);
                fixed4 color = lerp(background, planet, a);

                return color;
            }
            ENDCG
        }
    }
}