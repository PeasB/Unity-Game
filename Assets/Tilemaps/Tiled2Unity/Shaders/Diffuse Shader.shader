// Shader created with Shader Forge v1.16 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.16;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3315,x:32718,y:32739,varname:node_3315,prsc:2|diff-9889-RGB,normal-3150-RGB,clip-9889-A;n:type:ShaderForge.SFN_Tex2d,id:3150,x:32317,y:33002,ptovrint:False,ptlb:BumpMap,ptin:_BumpMap,varname:node_3150,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cb60a9bfa777940e795b619ba22161e2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:9889,x:32281,y:32655,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_9889,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:78061124eecc64820ba6dc6ccc95a35e,ntxv:0,isnm:False;proporder:3150-9889;pass:END;sub:END;*/

Shader "Tiled/TextureTintSnapNormal" {
	Properties{
		_BumpMap("BumpMap", 2D) = "bump" {}
	_MainTex("MainTex", 2D) = "white" {}
	[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}
		SubShader{
		Tags{
		"Queue" = "AlphaTest"
		"RenderType" = "TransparentCutout"
	}
		LOD 200
		Pass{
		Name "FORWARD"
		Tags{
		"LightMode" = "ForwardBase"
	}


		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_FORWARDBASE
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#pragma multi_compile_fwdbase_fullshadows
#pragma multi_compile_fog
#pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
#pragma target 3.0
		uniform float4 _LightColor0;
	uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
	uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
	struct VertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 texcoord0 : TEXCOORD0;
	};
	struct VertexOutput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float4 posWorld : TEXCOORD1;
		float3 normalDir : TEXCOORD2;
		float3 tangentDir : TEXCOORD3;
		float3 bitangentDir : TEXCOORD4;
		LIGHTING_COORDS(5,6)
			UNITY_FOG_COORDS(7)
	};
	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.normalDir = UnityObjectToWorldNormal(v.normal);
		o.tangentDir = normalize(mul(_Object2World, float4(v.tangent.xyz, 0.0)).xyz);
		o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
		o.posWorld = mul(_Object2World, v.vertex);
		float3 lightColor = _LightColor0.rgb;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
#ifdef PIXELSNAP_ON
		o.pos = UnityPixelSnap(o.pos);
#endif
		UNITY_TRANSFER_FOG(o,o.pos);
		TRANSFER_VERTEX_TO_FRAGMENT(o)
			return o;
	}
	float4 frag(VertexOutput i) : COLOR{
		i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	/////// Vectors:
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
	float3 normalLocal = _BumpMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
	float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
	clip(_MainTex_var.a - 0.5);
	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
	float3 lightColor = _LightColor0.rgb;
	////// Lighting:
	float attenuation = LIGHT_ATTENUATION(i);
	float3 attenColor = attenuation * _LightColor0.xyz;
	/////// Diffuse:
	float NdotL = max(0.0,dot(normalDirection, lightDirection));
	float3 directDiffuse = max(0.0, NdotL) * attenColor;
	float3 indirectDiffuse = float3(0,0,0);
	indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
	float3 diffuseColor = _MainTex_var.rgb;
	float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
	/// Final Color:
	float3 finalColor = diffuse;
	fixed4 finalRGBA = fixed4(finalColor,1);
	UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
	return finalRGBA;
	}
		ENDCG
	}
		Pass{
		Name "FORWARD_DELTA"
		Tags{
		"LightMode" = "ForwardAdd"
	}
		Blend One One


		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_FORWARDADD
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#pragma multi_compile_fwdadd_fullshadows
#pragma multi_compile_fog
#pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
#pragma target 3.0
		uniform float4 _LightColor0;
	uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
	uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
	struct VertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 texcoord0 : TEXCOORD0;
	};
	struct VertexOutput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float4 posWorld : TEXCOORD1;
		float3 normalDir : TEXCOORD2;
		float3 tangentDir : TEXCOORD3;
		float3 bitangentDir : TEXCOORD4;
		LIGHTING_COORDS(5,6)
	};
	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.normalDir = UnityObjectToWorldNormal(v.normal);
		o.tangentDir = normalize(mul(_Object2World, float4(v.tangent.xyz, 0.0)).xyz);
		o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
		o.posWorld = mul(_Object2World, v.vertex);
		float3 lightColor = _LightColor0.rgb;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
#ifdef PIXELSNAP_ON
		o.pos = UnityPixelSnap(o.pos);
#endif
		TRANSFER_VERTEX_TO_FRAGMENT(o)
			return o;
	}
	float4 frag(VertexOutput i) : COLOR{
		i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	/////// Vectors:
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
	float3 normalLocal = _BumpMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
	float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
	clip(_MainTex_var.a - 0.5);
	float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
	float3 lightColor = _LightColor0.rgb;
	////// Lighting:
	float attenuation = LIGHT_ATTENUATION(i);
	float3 attenColor = attenuation * _LightColor0.xyz;
	/////// Diffuse:
	float NdotL = max(0.0,dot(normalDirection, lightDirection));
	float3 directDiffuse = max(0.0, NdotL) * attenColor;
	float3 diffuseColor = _MainTex_var.rgb;
	float3 diffuse = directDiffuse * diffuseColor;
	/// Final Color:
	float3 finalColor = diffuse;
	return fixed4(finalColor * 1,0);
	}
		ENDCG
	}
		Pass{
		Name "ShadowCaster"
		Tags{
		"LightMode" = "ShadowCaster"
	}
		Offset 1, 1

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_SHADOWCASTER
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"
#include "Lighting.cginc"
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_shadowcaster
#pragma multi_compile_fog
#pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
#pragma target 3.0
		uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
	struct VertexInput {
		float4 vertex : POSITION;
		float2 texcoord0 : TEXCOORD0;
	};
	struct VertexOutput {
		V2F_SHADOW_CASTER;
		float2 uv0 : TEXCOORD1;
	};
	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
#ifdef PIXELSNAP_ON
		o.pos = UnityPixelSnap(o.pos);
#endif
		TRANSFER_SHADOW_CASTER(o)
			return o;
	}
	float4 frag(VertexOutput i) : COLOR{
		/////// Vectors:
		float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
		clip(_MainTex_var.a - 0.5);
		SHADOW_CASTER_FRAGMENT(i)
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
			CustomEditor "ShaderForgeMaterialInspector"
}
