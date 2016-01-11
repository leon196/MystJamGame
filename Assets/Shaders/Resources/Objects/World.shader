Shader "Unlit/World" {
   Properties {
      _Cube ("Environment Map", Cube) = "" {}
      _Rotation ("Rotation Degree", Float) = 0
   }
   SubShader {
      Tags { "Queue" = "Background" }
      
      Pass {   
         ZWrite On
         Cull Front

         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "../Utils/foundation.cginc"
         #include "../Utils/Utils.cginc"

         samplerCUBE _Cube;   
         samplerCUBE _NextCube;   
         float _InterpolationRatio;   
         float _IsUniverseTransition;   
         float _Rotation;   
         float _RotationNext;   
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
            // Sphere transition
            float ratio = dot(normalize(input.viewDir), _HoleDirection) * 0.5 + 0.5;
            // float ratio = dot(normalize(rotateY(input.viewDir, noiseIQ(input.pos * 0.02))), _HoleDirection) * 0.5 + 0.5;
            ratio = step(ratio, _InterpolationRatio);
            // ratio = step(ratio, 0.5);

            // Select between fade in/out transition and sphere transition
            ratio = lerp(_InterpolationRatio, ratio, _IsUniverseTransition);

            float3 normal = input.viewDir;
            float4 current = texCUBE(_Cube, rotateY(normal, _Rotation * PI / 180.));
            float4 next = texCUBE(_NextCube, rotateY(normal, _RotationNext * PI / 180.));

            return lerp(current, next, ratio);
         }
 
         ENDCG
      }
   }
}