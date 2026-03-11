Shader "Custom/Snow Interactive" {
	Properties {
		_PathColorOut("Snow Path Color Out", 2D) = "gray" {}
		_MainTex("Snow Texture", 2D) = "white" {}
		_Normal("Snow Normal", 2D) = "bump" {}
		_MaxTessDistance("Max Tessellation Distance", Float) = 50
		_Tess("Tessellation", Range(1,500)) = 20
		_SnowHeight ("Snow Height", Float) = 0.3
		_PathBlending("Snow Path Blending", Float) = 0.3
		_SnowNormalStrength("Snow Normal Strength", Float) = 0.3
		_RimPower("Rim Power", Float) = 20
		_ShadowColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
		_RimColor("Rim Color Snow", Color) = (0.5,0.5,0.5,1)
	}
	HLSLINCLUDE
	// Includes
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
	#include "SnowTessellation.hlsl"
	#pragma vertex Vertext_Tessellation
	#pragma hull hull
	#pragma domain domain
	#pragma instancing_options renderinglayer
	// Keywords
	#pragma multi_compile _ _CLUSTER_LIGHT_LOOP
	#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
	#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
	#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
	#pragma multi_compile _ _SHADOWS_SOFT
	#pragma multi_compile_fog
	#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
	ControlPoint Vertext_Tessellation(Attributes2 v) {
		ControlPoint p;
		p.vertex = v.vertex;
		p.uv = v.uv;
		p.normal = v.normal;
		p.tangent = v.tangent;
		return p;
	}
	ENDHLSL
	SubShader {
		Tags {
			"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
		}
		Pass {
			Tags {
				"LightMode" = "UniversalForward"
			}
			HLSLPROGRAM
			// vertex happens in snowtessellation.hlsl
			#pragma require tessellation tessHW
			#pragma fragment frag
			#pragma target 4.0
			sampler2D _MainTex, _PathColorOut;
			float4    _RimColor;
			float     _RimPower;
			float     _PathBlending;
			float4    _ShadowColor;
			float     _SnowNormalStrength;
			half4     frag(Varyings2 IN) : SV_Target {
				float3 worldPosition = mul(unity_ObjectToWorld, IN.vertex).xyz; // Effects RenderTexture Reading
				float2 uv = IN.worldPos.xz - _Position.xz;
				uv /= (_OrthographicCamSize * 2);
				uv += 0.5;
				float4 effect = tex2D(_GlobalEffectRT, uv); // effects texture		
				effect *= smoothstep(0.99, 0.9, uv.x) * smoothstep(0.99, 0.9, 1 - uv.x); // mask to prevent bleeding
				effect *= smoothstep(0.99, 0.9, uv.y) * smoothstep(0.99, 0.9, 1 - uv.y);
				float3 snowtexture = tex2D(_MainTex, IN.worldPos.xz).rgb; // worldspace Snow texture
				float3 snownormal = UnpackNormal(tex2D(_Normal, IN.worldPos.xz)).rgb; // snow normal
				snownormal = snownormal.r * IN.tangent + snownormal.g * IN.bitangent + snownormal.b * IN.normal;
				//-----------------
				//-----------------
				//-----------------
				float3 pathColorOut = tex2D(_PathColorOut, IN.worldPos.xz).rgb;
				float3 asdf = lerp(snowtexture, pathColorOut, effect.g);
				float3 mainColors = lerp(asdf, snowtexture, _PathBlending);
				//-----------------
				//-----------------
				//-----------------
				float shadow = 0;
				half4 shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
				#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
				Light mainLight = GetMainLight(shadowCoord);
				shadow = mainLight.shadowAttenuation;
				#else
				Light mainLight = GetMainLight();
				#endif
				float3    extraLights;
				InputData inputData = (InputData)0; // forward and forward+ lights loop. Create inputdata struct to use in LIGHT_LOOP
				inputData.positionWS = IN.worldPos;
				inputData.normalWS = IN.normal;
				inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.worldPos);
				float4 screenPos = float4(IN.vertex.x, (_ScaledScreenParams.y - IN.vertex.y), 0, 0);
				inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(screenPos);
				uint lightsCount = GetAdditionalLightsCount();
				LIGHT_LOOP_BEGIN(lightsCount)
					Light  aLight = GetAdditionalLight(lightIndex, IN.worldPos, half4(1, 1, 1, 1));
					float3 attenuatedLightColor = aLight.color * (aLight.distanceAttenuation * aLight.shadowAttenuation);
					extraLights += attenuatedLightColor;
				LIGHT_LOOP_END
				float4 litMainColors = float4(mainColors, 1);
				extraLights *= litMainColors.rgb;
				//-----------------
				//-----------------
				// NORMAL
				//-----------------
				//-----------------
				// ambient and mainlight colors added
				half4 extraColors;
				extraColors.rgb = litMainColors.rgb * mainLight.color.rgb * (shadow + unity_AmbientSky.rgb);
				extraColors.a = 1;
				// colored shadows
				float3 coloredShadows = shadow + lerp(_ShadowColor, 0, shadow);
				litMainColors.rgb = litMainColors.rgb * mainLight.color * coloredShadows;
				// everything together
				float4 final = litMainColors + extraColors + float4(extraLights, 0);
				// add in fog
				final.rgb = MixFog(final.rgb, IN.fogFactor);
				return final;
			}
			ENDHLSL

		}
		Pass {
			Name "DepthOnly"
			Tags {
				"LightMode" = "DepthOnly"
			}
			ZWrite On
			ColorMask R
			Cull[_Cull]
			HLSLPROGRAM
			#pragma target 2.0
			// Shader Stages
			#pragma vertex DepthOnlyVertex
			#pragma fragment DepthOnlyFragment
			// Material Keywords
			#pragma shader_feature_local _ALPHATEST_ON
			#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
			// Unity defined keywords
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			// GPU Instancing
			#pragma multi_compile_instancing
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
			ENDHLSL
		}
		Pass {
			Name "ShadowCaster"
			Tags {
				"LightMode" = "ShadowCaster"
			}
			ZWrite On
			ZTest LEqual
			ColorMask 0
			HLSLPROGRAM
			#pragma target 3.0
			// Support all the various light  ypes and shadow paths
			#pragma multi_compile_shadowcaster
			// Unity defined keywords
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
			// Register our functions
			#pragma fragment frag
			// A custom keyword to modify logic during the shadow caster pass
			half4 frag(Varyings2 IN) : SV_Target {
				return 0;
			}
			ENDHLSL
		}
	}
}