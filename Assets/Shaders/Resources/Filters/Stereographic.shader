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
            #include "../Utils/foundation.cginc"
            #include "../Utils/Dither.cginc"
            #include "../Utils/Utils.cginc"
            
            sampler2D _MainTex;
            sampler2D _Equirectangle;
            float _InputHorizontal;
            float _InputVertical;
            float _InputDepth;
            float _InterpolationRatio;
            float _CameraAngleY;

            // from https://github.com/notlion/streetview-stereographic

            fixed4 frag (v2f_img i) : SV_Target {

                float2 rads = float2(PI * 2., PI);

                // float timing = sin(_Time * 20.0) * 0.5 + 0.5;
                float scale = 0.1 + smoothstep(0.25, 0.75, _InterpolationRatio) * 20.0;
                // float scale = 1.0 + _InputDepth * 4.0;

                float2 pnt = (i.uv - float2(.5, .5)) * scale;
                pnt.x *= _ScreenParams.x / _ScreenParams.y;

                // Project to Sphere
                float x2y2 = pnt.x * pnt.x + pnt.y * pnt.y;
                float3 sphere_pnt = float3(2. * pnt, x2y2 - 1.) / (x2y2 + 1.);

                sphere_pnt = rotateY(sphere_pnt, PI);
                float rX = smoothstep(0.7, .9, 1.0 - _InterpolationRatio);
                // sphere_pnt = rotateX(sphere_pnt, 0);//-rX * PI * 0.5);
                // sphere_pnt = rotateY(sphere_pnt, _InputHorizontal);
                // sphere_pnt = rotateX(sphere_pnt, -_InputVertical);

                float r = length(sphere_pnt);
                float lon = atan2(sphere_pnt.y, sphere_pnt.x) + PI;
                float lat = acos(sphere_pnt.z / r);

                // float t = _Time * 4.0;
                float2 uv = fmod(abs(float2(lon, lat) / rads), 1.0);
                uv.x = 1.0 - uv.x;

                fixed4 planet = tex2D(_Equirectangle, uv);
                fixed4 background = tex2D(_MainTex, i.uv);
                fixed4 color = lerp(planet, background, .5);

                return color;
            }
            ENDCG
        }
    }
}