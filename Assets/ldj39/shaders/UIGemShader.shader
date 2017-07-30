/**
 * Based on the Gem shader, this one simplifies the shader a bit so that it's always in color.
 */

Shader "Bitzawolf/UI Gem Surface"
{
    Properties
	{
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", color) = (1, 1, 1, 1)
    }

    SubShader
	{
		Tags { "RenderType" = "Transparent" }

		CGPROGRAM
		#pragma surface surf Custom

		half4 LightingCustom (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half lightness = (_LightColor0.b + _LightColor0.r) / 2;

			half NdotL = dot (s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half NdotV = dot(s.Normal, viewDir);

			half4 c;
			c.rgb = s.Albedo * (diff * atten) * lightness;

			// THIS IS GEM SHADER!
			half difference = abs(NdotL - NdotV);
			if (difference < 0.05)
			{
				half4 shine = half4(lightness,  lightness, lightness, 1) * .9;
				float shineRamp = (1 - difference - 0.95) / 0.05;
				c = lerp(c, shine, shineRamp);
			}
			// END EXCITEMENT

			c.a = s.Alpha;
			return c;
		}

		struct Input
		{
			float2 uv_MainTex;
		};
    
		sampler2D _MainTex;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
		}
		ENDCG
    }

    Fallback "Diffuse"
}