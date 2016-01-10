Shader "Unlit/Cubmap" {
   Properties {
      _Cube ("Environment Map", Cube) = "" {}
      _NextCube ("Next Environment Map", Cube) = "" {}
      _InterpolationRatio ("Interpolation Ratio", Float) = 0
      _HoleDirection ("Hole Direction", Vector) = (0, 1, 0, 1)
   }
   SubShader {
      Tags { "Queue" = "Background" }
      
      Pass {   
         ZWrite On
         Cull Front

         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "foundation.cginc"

         samplerCUBE _Cube;   
         samplerCUBE _NextCube;   
         float _InterpolationRatio;   
         float3 _HoleDirection;   
 
         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 viewDir : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            output.viewDir = mul(modelMatrix, input.vertex).xyz - _WorldSpaceCameraPos;
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);      
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float ratio = dot(normalize(input.viewDir), _HoleDirection) * 0.5 + 0.5;
            ratio = step(ratio, _InterpolationRatio);
            return lerp(texCUBE(_Cube, input.viewDir), texCUBE(_NextCube, input.viewDir), ratio);
         }
 
         ENDCG
      }
   }
}