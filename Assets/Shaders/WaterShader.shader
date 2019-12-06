Shader "Custom/WaterShader"
{
    Properties
    {
		[Header(Albedo)]
		_WaterDarkColor("Water Dark Color", Color) = (0, 0, 1, 1)
		_WaterLightColor("Water Light Color", Color) = (0, 0.2, 0.8, 1)
		_Glossiness("Smoothness", Range(0,1)) = 0.7
		_FresnelPower("Fresnel Power", Range(1, 10)) = 1
		_WaterSpeed("WaterSpeed", Range(0.5,5)) = 1

		[Header(Displacement)]
		_DisplacementMap("Displacement Map", 2D) = "gray" {}
		_NoiseMap("Voronoi Map", 2D) = "gray"{}
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
		_WaveDirection("WaveDirection", Vector) = (0.3, 1, 0, 0)

		[Header(Normal)]
		_NormalMap("Normal map", 2D) = "bump" {}
		_NormalMapTiling("Normal map Tiling", Vector) = (5, 5, 0 , 0)

		[Header(Tessellation)]
		_Tessellation("Tessellation", Range(1, 128)) = 1
		_MaxTessellationDistance("Maximum tessellation", Range(25, 100)) = 25

		[Header(Transparency)]
		_Alpha("Alpha", Range(0.1, 1)) = 0.76
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True"}
		Blend One OneMinusSrcAlpha
		ZWrite Off
        LOD 200
		

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert tessellate:tessDistance alpha:fade

        #pragma target 4.6
		#include "Tessellation.cginc"

		float _Tessellation;
		float _MaxTessellationDistance;

		float4 tessDistance(appdata_full v0, appdata_full v1, appdata_full v2) {
			float minDist = 10.0;
			return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, _MaxTessellationDistance, _Tessellation);
		}


		sampler2D _DisplacementMap;
		sampler2D _NoiseMap;
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
			half2 waterDirection = _WaveDirection*0.05;
			return waterDirection * _SinTime[1] *_WaterSpeed;
		}

		void vert(inout appdata_full v) {
			/* Displacement */
			float d = tex2Dlod(_DisplacementMap, float4(v.texcoord.xy, 0, 0)).r * _Displacement;

			float noise = (tex2Dlod(_NoiseMap, float4(v.texcoord.xy*5 + _Time[1]/10.0f, 0, 0)).r - 0.5)*0.1f;
			v.normal = calculateNormal(_DisplacementMap, float4(v.texcoord.xy, 0, 0), _DisplacementMap_TexelSize.x, _DisplacementMap_TexelSize.z);


			v.vertex.xyz += v.normal * d + float3(noise, 0, 0);
			return;
		}

		struct Input {
			float3 viewDir;
			float3 worldPos;
		};

		sampler2D _NormalMap;
		float2 _NormalMapTiling;

		half _FresnelPower;
        half _Glossiness;
		half _Alpha;

        fixed4 _WaterDarkColor;
		fixed4 _WaterLightColor;

		fixed2 _NormalMapScale;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			/* Normals*/
			half2 uv = IN.worldPos.xz*_NormalMapTiling/20.0f;
			float3 offsetNormal = UnpackNormal(tex2D(_NormalMap, uv - _Time[1] / 10.0f));
			float3 normal = o.Normal;
			o.Normal = 0.35 * offsetNormal + 0.75 * normal;

			/* Albedo */
			half fresnelCoeff = saturate(dot(normalize(IN.viewDir), o.Normal));
			fresnelCoeff = pow(fresnelCoeff, _FresnelPower);
			o.Albedo = lerp(_WaterDarkColor, _WaterLightColor, fresnelCoeff);

			// o.Normal = calculateNormal(_DisplacementMap, float4(IN.uv_NormalMapFirst, 0, 0), 1, 1024);

			/*half2 waterDirection_FirstMap = half2(0.3, 1)*0.05;
			half2 offset_FirstMap = waterDirection_FirstMap * _Time[1] *_WaterSpeed;


			half2 waterDirection_SecondMap = half2(-0.3, -1) * 0.005;
			half2 offset_SecondMap = waterDirection_SecondMap * _Time[1] *_WaterSpeed;


			half3 secondNormal = UnpackNormal(tex2D(_NormalMapSecond, uv + offset_SecondMap));
			
			half3 normal = 0.8 * firstNormal + 0.2 * secondNormal;

			o.Normal = normal;*/


			/* Smoothness */
            o.Smoothness = _Glossiness;

			o.Alpha = _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
