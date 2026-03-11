Shader "Serebrennikov/BlizzardParticle" {
	Properties {
		[MainTexture] _BaseMap("Particle Texture", 2D) = "white" {}
		[MainColor] _BaseColor("Tint", Color) = (1, 1, 1, 1)
		_Intensity("Intensity", Float) = 1.0
	}
	SubShader {
		Tags {
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"IgnoreProjector"="True"
			"RenderPipeline"="UniversalPipeline"
		}
		Pass {
			Name "Forward"
			Tags {
				"LightMode"="UniversalForward"
			}
			Blend One One
			ZWrite Off
			Cull Off
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);
			CBUFFER_START(UnityPerMaterial)
				float4 _BaseMap_ST;
				float4 _BaseColor;
				float  _Intensity;
			CBUFFER_END
			struct Attributes {
				float3 positionOS : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};
			struct Varyings {
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};
			Varyings vert(Attributes input) {
				Varyings             output;
				VertexPositionInputs pos = GetVertexPositionInputs(input.positionOS);
				output.positionCS = pos.positionCS;
				output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
				output.color = input.color * _BaseColor;
				return output;
			}
			half4 frag(Varyings input) : SV_Target {
				half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
				half3 rgb = tex.rgb * input.color.rgb;
				half  alphaIntensity = tex.a * input.color.a;
				rgb *= (alphaIntensity * _Intensity);
				return half4(rgb, 1.0h);
			}
			ENDHLSL
		}
	}
	FallBack Off
}