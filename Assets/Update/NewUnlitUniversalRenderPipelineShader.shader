Shader "UI/RenderTextureDebugView_URP" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Channel ("Channel (0=R, 1=G, 2=B, 3=A)", Float) = 1
		_Intensity ("Intensity", Float) = 1
		_Invert ("Invert", Float) = 0
	}

	SubShader {
		Tags {
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"RenderPipeline"="UniversalPipeline"
		}

		Pass {
			Name "Forward"
			Tags {
				"LightMode"="UniversalForward"
			}

			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite Off
			ZTest Always

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			struct Attributes {
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct Varyings {
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			CBUFFER_START(UnityPerMaterial)
				float4 _MainTex_ST;
				float  _Channel;
				float  _Intensity;
				float  _Invert;
			CBUFFER_END
			Varyings vert(Attributes v) {
				Varyings o;
				o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			half4 frag(Varyings i) : SV_Target {
				float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				float  value;
				if(_Channel < 0.5) value = tex.r;
				else if(_Channel < 1.5) value = tex.g;
				else if(_Channel < 2.5) value = tex.b;
				else value = tex.a;
				value *= _Intensity;
				if(_Invert > 0.5) value = 1.0 - value;
				return half4(value, value, value, 1.0);
			}
			ENDHLSL
		}
	}
}