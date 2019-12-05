Shader "Custom/WaterShader"
{
    Properties
    {
		_WaterDarkColor("Water Dark Color", Color) = (0, 0, 1, 1)
		_WaterLightColor("Water Light Color", Color) = (0, 0.2, 0.8, 1)
		_NormalMapFirst("First Normal map", 2D) = "bump" {}
		_NormalMapSecond("Second Normal map", 2D) = "bump" {}
		_FresnelPower("Fresnel Power", Range(1, 10)) = 1
		_WaterSpeed("WaterSpeed", Range(0.5,5)) = 1

        _Glossiness ("Smoothness", Range(0,1)) = 0.7
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		
        struct Input
        {
            float2 uv_NormalMapFirst;
			float2 uv_NormalMapSecond;
			float3 viewDir;
        };

		sampler2D _NormalMapFirst;
		sampler2D _NormalMapSecond;

		half _WaterSpeed;
		half _FresnelPower;
        half _Glossiness;

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
			
			/* Albedo */
			half fresnelCoeff = saturate(dot(normalize(IN.viewDir), o.Normal));
			fresnelCoeff = pow(fresnelCoeff, _FresnelPower);
			o.Albedo = lerp(_WaterDarkColor, _WaterLightColor, fresnelCoeff);

			/* Normals*/
			half2 waterDirection_FirstMap = half2(0.3, 1)*0.05;
			half2 offset_FirstMap = waterDirection_FirstMap * _Time[1] *_WaterSpeed;


			half2 waterDirection_SecondMap = half2(-0.3, -1) * 0.005;
			half2 offset_SecondMap = waterDirection_SecondMap * _Time[1] *_WaterSpeed;

			half3 firstNormal = UnpackNormal(tex2D(_NormalMapFirst, IN.uv_NormalMapFirst + offset_FirstMap));
			half3 secondNormal = UnpackNormal(tex2D(_NormalMapSecond, IN.uv_NormalMapSecond + offset_SecondMap));
			
			half3 normal = 0.8 * firstNormal + 0.2 * secondNormal;
			//half3 normal = 0.5 * (firstNormal + secondNormal);

			o.Normal = normal;


			/* Smoothness */
            o.Smoothness = _Glossiness;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
