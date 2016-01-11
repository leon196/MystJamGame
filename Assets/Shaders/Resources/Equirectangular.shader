Shader "Hidden/Equirectangular" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "foundation.cginc"
            #define PI 3.141592653589793

            sampler2D _MainTex;
            samplerCUBE _Cubemap;
            float _InputHorizontal;
            float _InputVertical;
            float _InputDepth;

            // from https://gist.github.com/Farfarer/5664694

            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert( appdata_img v )
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                float2 uv = v.texcoord.xy * 2 - 1;
                uv *= float2(PI, HALFPI);
                o.uv = uv;
                return o;
            }
    
            fixed4 frag(v2f i) : COLOR 
            {
                float2 uv = i.uv;
                uv *= _InputDepth;
                // uv = fmod(abs(uv + _Time), 1.0);
                float cosy = cos(uv.y);
                float3 normal = float3(0,0,0);
                normal.x = cos(uv.x) * cosy;
                normal.y = uv.y;
                normal.z = cos(uv.x - HALFPI) * cosy;
                float t = _Time * 10.0;
                normal = rotateX(normal, _InputVertical);
                normal = rotateY(normal, _InputHorizontal);
                float4 background = texCUBE(_Cubemap, normal);
                return background;
            }
            ENDCG
        }
    }
}