Shader "Serebrennikov/LightBulb" {
	Properties {
		_Hue("Hue (0..1)", Range(0, 1)) = 0
		_Saturation("Saturation", Range(0, 1)) = 1
		_Value("Value", Range(0, 1)) = 1

		_Intensity("Intensity (can exceed 1)", Float) = 1
		_Threshold("Emit Threshold", Float) = 1
	}
	SubShader {
		Tags {
			"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
		}
		Pass {
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
			CBUFFER_START(UnityPerMaterial)
				float _Hue;
				float _Saturation;
				float _Value;
				float _Intensity;
				float _Threshold;
			CBUFFER_END
			float3 HsvToRgb(float3 hsv) {
				float  hue = hsv.x;
				float  sat = hsv.y;
				float  val = hsv.z;
				float3 k = float3(1.0, 2.0 / 3.0, 1.0 / 3.0);
				float3 p = abs(frac(hue + k) * 6.0 - 3.0);
				float3 rgb = saturate(p - 1.0);
				return val * lerp(float3(1.0, 1.0, 1.0), rgb, sat);
			}
			Varyings vert(Attributes IN) {
				Varyings OUT;
				OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				OUT.uv = IN.uv;
				return OUT;
			}
			half4 frag(Varyings IN) : SV_Target {
				float3 rgb = HsvToRgb(float3(_Hue, _Saturation, _Value));
				float baseScale = 0.0;
				if(_Threshold > 0.0001) baseScale = saturate(_Intensity / _Threshold);
				float  emission = max(_Intensity - _Threshold, 0.0);
				float  totalScale = baseScale + emission;
				float3 outColor = rgb * totalScale; // HDR when emission > 0
				return half4(outColor, 1.0h);
			}
			ENDHLSL
		}
	}
}