// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WaterShader"
{
    Properties
    {
		[Header(Albedo)]
		_WaterDarkColor("Water Dark Color", Color) = (0, 0, 1, 1)
		_WaterLightColor("Water Light Color", Color) = (0, 0.2, 0.8, 1)
		_Glossiness("Smoothness", Range(0,1)) = 0.7
		_FresnelPower("Fresnel Power", Range(1, 10)) = 1
		_WaterSpeed("WaterSpeed", Range(2,10)) = 5

		[Header(Foam)]
		_FoamMap("Foam Map", 2D) = "white" {}
		_FoamNormalMap("Foam Normal Map", 2D) = "bump" {}
		_FoamOffset("Foam Offset", Range(0, 0.5)) = 0.0

		[Header(Tessellation)]
		_Tessellation("Tessellation", Range(1, 128)) = 1
		_MaxTessellationDistance("Maximum tessellation distance", Range(25, 100)) = 25

		[Header(Displacement)]
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
		_DisplacementMap("Displacement Map", 2D) = "gray" {}
		_WaveDirection("WaveDirection", Vector) = (0.3, 1, 0, 0)

		[Header(Normal)]
		_NormalMap("Normal map", 2D) = "bump" {}

		[Header(Reflection)]
		_ReflectionMap("Reflection Texture", 2D) = "white" {}

		[Header(Transparency)]
		_Alpha("Alpha", Range(0.1, 1)) = 0.76
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True"}
		Blend One OneMinusSrcAlpha
		//Cull Off
		ZWrite Off
        LOD 200
		

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert tessellate:tessDistance alpha

        #pragma target 4.6
		#include "Tessellation.cginc"

		float _Tessellation;
		float _MaxTessellationDistance;

		float4 tessDistance(appdata_full v0, appdata_full v1, appdata_full v2) {
			float minDist = 10.0;
			return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, _MaxTessellationDistance, _Tessellation);
		}


		sampler2D _DisplacementMap;
		float4 _DisplacementMap_TexelSize;
		float _Displacement;


		float3 calculateNormal(sampler2D heightMap, float4 uv, float texelSize, float texSize) {
			float4 h;
			h[0] = tex2Dlod(heightMap, uv + float4(texelSize * float2(0, -1 / texSize), 0, 0)).r * _Displacement;
			h[1] = tex2Dlod(heightMap, uv + float4(texelSize * float2(-1 / texSize, 0), 0, 0)).r * _Displacement;
			h[2] = tex2Dlod(heightMap, uv + float4(texelSize * float2(1 / texSize, 0), 0, 0)).r * _Displacement;
			h[3] = tex2Dlod(heightMap, uv + float4(texelSize * float2(0, 1 / texSize), 0, 0)).r * _Displacement;

			// Now compute normal using gradient
			float3 n;
			n.z = h[3] - h[0];
			n.x = h[2] - h[1];
			n.y = 2;

			return normalize(n);
		}


		half _WaterSpeed;
		half2 _WaveDirection;

		half2 waterOffset() {
			half2 waterDirection = _WaveDirection;
			return _Time[1] / 100 * waterDirection * _WaterSpeed;
		}


		float4 _DisplacementMap_ST;
		void vert(inout appdata_full v) {
			/* Displacement */
			float2 uv = TRANSFORM_TEX(v.texcoord + waterOffset(), _DisplacementMap);
			float d = tex2Dlod(_DisplacementMap, float4(uv, 0, 0)).r * _Displacement;
			v.normal = calculateNormal(_DisplacementMap, float4(uv, 0, 0), 50, _DisplacementMap_TexelSize.z);
			v.vertex.xyz += v.normal * d;
			return;
		}



		float smoothstep(float x) {
			x = saturate(x);
			return saturate(x * x * x * (x * (6 * x - 15) + 10));
		}

		struct Input {
			float4 screenPos;
			float3 viewDir;
			float3 worldPos;
			float2 uv_NormalMap;
			float2 uv_FoamMap;
			float2 uv_ReflectionMap;
			float3 worldNormal; INTERNAL_DATA
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	   // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	   // #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		sampler2D _ReflectionMap;
		sampler2D _NormalMap;
		sampler2D _FoamMap;
		sampler2D _FoamNormalMap;
		sampler2D_float _CameraDepthTexture;
		samplerCUBE _Cube;

		half _FresnelPower;
        half _Glossiness;
		half _Alpha;
		half _FoamOffset;
        fixed4 _WaterDarkColor;
		fixed4 _WaterLightColor;
		float4 _ReflectionMap_ST;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

			/* Depth */
			float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));
			depth = LinearEyeDepth(depth);

			float depthDifference = depth - (IN.screenPos.w - _FoamOffset);
			depthDifference = 1 - saturate(depthDifference);

			/* Normals*/
			float3 offsetNormal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap + waterOffset()));
			float3 foamNormal = UnpackNormal(tex2D(_FoamNormalMap, IN.uv_FoamMap + 0.1 * _CosTime[1]));
			float3 normal = o.Normal;
			float3 final_normal = normalize(lerp(0.15 * offsetNormal + 0.85 * normal, foamNormal, 0.15));
			o.Normal = final_normal;

			/* Albedo */
			half fresnelCoeff = saturate(dot(normalize(IN.viewDir), o.Normal));
			fresnelCoeff = pow(fresnelCoeff, _FresnelPower);

			float3 foamColor = lerp(tex2D(_FoamMap, IN.uv_FoamMap + 0.1*_CosTime[1]), _WaterDarkColor, 0.25);

			float3 normalWS = WorldNormalVector(IN, o.Normal);
			float2 screenPos = (IN.screenPos.xy / IN.screenPos.w);
			screenPos = float2(1 - screenPos.x, screenPos.y) + normalWS.xz* 0.15;

			//screenPos = TRANSFORM_TEX(screenPos, _ReflectionMap);
		
			float3 reflectionColor = tex2D(_ReflectionMap, screenPos);
			o.Emission = reflectionColor;
			o.Albedo = lerp(lerp(_WaterDarkColor, _WaterLightColor, fresnelCoeff), foamColor, depthDifference);
            o.Smoothness = _Glossiness;

			/* Transparency */
			o.Alpha = _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
