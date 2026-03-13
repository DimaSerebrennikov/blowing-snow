Shader "Custom/URP/ParticleRadialGradient_AdditiveSoft" {
	SubShader {
		Tags {
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"RenderPipeline"="UniversalPipeline"
		}
		Pass {
			Name "Forward"
			Tags {
				"LightMode"="UniversalForward"
			}
			Blend One One
			ZWrite Off
			Cull Back
			ColorMask RGB
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
			static const float4 kCenterColor = float4(1, 1, 1, 1);
			static const float4 kEdgeColor = float4(0, 0, 0, 1);
			static const float  kFalloff = 2.0;
			static const float  kRadius = 0.5;
			struct Attributes {
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};
			struct Varyings {
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float4 screenPos : TEXCOORD1;
				float  viewZ : TEXCOORD2;
			};
			Varyings vert(Attributes IN) {
				Varyings OUT;
				float3   positionWS = TransformObjectToWorld(IN.positionOS.xyz);
				float3   positionVS = TransformWorldToView(positionWS);
				OUT.positionHCS = TransformWorldToHClip(positionWS);
				OUT.uv = IN.uv;
				OUT.color = IN.color;
				OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
				OUT.viewZ = -positionVS.z;
				return OUT;
			}
			half4 frag(Varyings IN) : SV_Target {
				float2 centeredUV = IN.uv - float2(0.5, 0.5);
				float  dist = length(centeredUV);
				float  radial = saturate(1.0 - dist / max(kRadius, 0.0001));
				radial = pow(radial, kFalloff);
				float4 col = lerp(kEdgeColor, kCenterColor, radial);
				col *= IN.color;
				float2 uv = IN.screenPos.xy / IN.screenPos.w;
				float  sceneRawDepth = SampleSceneDepth(uv);
				float  sceneEyeDepth = LinearEyeDepth(sceneRawDepth, _ZBufferParams);
				float  partEyeDepth = IN.viewZ;
				float  fade = saturate(sceneEyeDepth - partEyeDepth);
				col.a *= fade;
				col.rgb *= col.a;
				return col;
			}
			ENDHLSL
		}
	}
}