Shader "Serebrennikov/MaskBlend" {
	Properties {
		_BaseTex ("Base (Original)", 2D) = "white" {}
		_DrawTex ("Drawn (Overlay)", 2D) = "white" {}
		_MaskTex ("Mask (R)", 2D) = "black" {}
	}
	SubShader {
		Tags {
			"RenderPipeline"="UniversalPipeline"
			"RenderType"="Opaque"
			"Queue"="Geometry"
		}
		Pass {
			Name "Unlit"
			Tags {
				"LightMode"="UniversalForward"
			}
			HLSLPROGRAM
			#pragma vertex Vertex
			#pragma fragment Fragment
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			TEXTURE2D(_BaseTex);
			SAMPLER(sampler_BaseTex);
			TEXTURE2D(_DrawTex);
			SAMPLER(sampler_DrawTex);
			TEXTURE2D(_MaskTex);
			SAMPLER(sampler_MaskTex);
			float4 _BaseTex_ST;
			float4 _DrawTex_ST;
			float4 _MaskTex_ST;
			struct VertextContext {
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct FragmentContext {
				float4 positionHCS : SV_POSITION;
				float2 uvBase : TEXCOORD0;
				float2 uvDraw : TEXCOORD1;
				float2 uvMask : TEXCOORD2;
			};
			FragmentContext Vertex(VertextContext input) {
				FragmentContext output;
				output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
				output.uvBase = TRANSFORM_TEX(input.uv, _BaseTex);
				output.uvDraw = TRANSFORM_TEX(input.uv, _DrawTex);
				output.uvMask = TRANSFORM_TEX(input.uv, _MaskTex);
				return output;
			}
			half4 Fragment(FragmentContext input) : SV_Target {
				half4 baseColor = SAMPLE_TEXTURE2D(_BaseTex, sampler_BaseTex, input.uvBase);
				half4 drawColor = SAMPLE_TEXTURE2D(_DrawTex, sampler_DrawTex, input.uvDraw);
				half  mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, input.uvMask).r;
				half4 result = lerp(baseColor, drawColor, mask);
				return result;
			}
			ENDHLSL
		}
	}
}