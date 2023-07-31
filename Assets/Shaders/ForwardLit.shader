Shader "CustomShadowMap/ForwardLit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			UNITY_DECLARE_SHADOWMAP(_LightShadowMapTexture);

			float4 _LightPosAndRadius;
			float4 _LightDirAndAngle;
			float4 _LightColor;
			float4x4 _LightViewProjTransform;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex));
				return o;
			}

			//

			float4 CalcSpotLight(float3 pos, float3 normal, float3 lightPos, float lightRadius, float3 lightDir, float lightAngle, float3 lightColor)
			{
				float3 pixelToLight = lightPos - pos;
				float d = length(pixelToLight);
				pixelToLight /= d;

				float NdotL = saturate(dot(pixelToLight, normal));

				float atten = saturate(1.0f - d * d / (lightRadius * lightRadius));
				atten *= atten;

				float spot = dot(-pixelToLight, lightDir.xyz);
				spot = saturate((spot - lightAngle) / (1.0f - lightAngle));
				atten *= spot;

				return float4(atten * NdotL * lightColor, 1.0f);
			}

			float CalcShadow(Texture2D_float shadowMapTexture, SamplerComparisonState samplershadowMapTexture, float4x4 lightViewProjTransform, float3 pos)
			{
				float4 pos_lightSpace = mul(lightViewProjTransform, float4(pos, 1.0f));
				pos_lightSpace /= pos_lightSpace.w;
				pos_lightSpace.xy *= 0.5f;
				pos_lightSpace.xy += 0.5f;
				pos_lightSpace.z += 0.0025f;

				return UNITY_SAMPLE_SHADOW(shadowMapTexture, float3(pos_lightSpace.xyz));
			}

			//

			fixed4 frag(v2f i) : SV_Target
			{
				float3 pos = i.worldPos.xyz;
				float3 normal = normalize(i.normal);

				float4 albedo = tex2D(_MainTex, i.uv);

				float4 lighting = CalcSpotLight(
					pos, normal,
					_LightPosAndRadius.xyz, _LightPosAndRadius.w,
					_LightDirAndAngle.xyz, _LightDirAndAngle.w,
					_LightColor.xyz
				);

				lighting *= CalcShadow(_LightShadowMapTexture, sampler_LightShadowMapTexture, _LightViewProjTransform, pos);

				return lighting * albedo;
			}

			ENDCG
		}
	}
}
