Shader "Custom/DissolveShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _NoiseTex ("Noise", 2D) = "gray" {}
		_MainTex("Texture", 2D) = "white" {}
		_NormalMap("Normal map", 2D) = "bump" {}
		_Metallic("Metallic", Range(0,1)) = 0
		_Glossiness("Glossiness", Range(0,1)) = 0

		[Header(Dissolve)]
		_DissolveColor("DissolveColor", Color) = (0, 0, 1, 1)
		_AlphaThreshold("AlphaThreshold", Range(0, 0.5)) = 0.5
		_DissolveRange("DissolveRange", Range(0, 1)) = 0
    }


    SubShader
    {
        Tags {"Queue" = "AlphaTest" "RenderType" = "TransparentCutout"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _NoiseTex;
		sampler2D _MainTex;
		sampler2D _NormalMap;

        struct Input
        {
            float2 uv_NoiseTex;
        };

        half _Glossiness;
        half _Metallic;
		half _AlphaThreshold;
		half _DissolveRange;
        fixed4 _Color;
		fixed4 _DissolveColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Normal = tex2D(_NoiseTex, IN.uv_NoiseTex);
			fixed4 c = tex2D(_MainTex, IN.uv_NoiseTex);

			float3 noiseValue = tex2D(_NoiseTex, IN.uv_NoiseTex);
			half dissolveClip = _AlphaThreshold - noiseValue.r;
			half edgeRamp = max(0, _DissolveRange - dissolveClip);
			o.Albedo = lerp(c, _DissolveColor, min(1, edgeRamp*100));
			clip(dissolveClip);
            
			
        }
        ENDCG
    }
    FallBack "Diffuse"
}
