Shader "Custom/WorldAlpha" {
   Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Rotation ("Rotation Degree", Float) = 0
   }
   SubShader {
      Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
      
      Pass {   
         Blend SrcAlpha OneMinusSrcAlpha
         ZWrite Off
         Cull Front

         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "../Utils/foundation.cginc"
         #include "../Utils/Utils.cginc"

         sampler2D _MainTex;
         float4 _MainTex_ST;

         struct v2f {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float3 normal : NORMAL;
         };
         
         v2f vert (appdata_full v)
         {
            v2f o;
            o.normal = -v.normal;
            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
            o.uv.x = 1.0 - o.uv.x;
            return o;
         }
 
         float4 frag(v2f i) : COLOR
         {
            fixed4 col = tex2D(_MainTex, i.uv);
            return col;
         }
 
         ENDCG
      }
   }
}