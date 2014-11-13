// Shader created with Shader Forge Beta 0.30 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.30;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:True,lprd:True,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:1,fgcg:0.7705882,fgcb:0.4264706,fgca:1,fgde:1,fgrn:5.18,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32499,y:32686|normal-186-RGB,custl-289-OUT;n:type:ShaderForge.SFN_Tex2d,id:176,x:32939,y:32813,ptlb:node_176,ptin:_node_176,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Append,id:177,x:33156,y:32929|A-182-OUT,B-183-OUT;n:type:ShaderForge.SFN_Dot,id:182,x:33320,y:32743,dt:4|A-185-OUT,B-184-OUT;n:type:ShaderForge.SFN_Vector1,id:183,x:33338,y:32997,v1:0;n:type:ShaderForge.SFN_NormalVector,id:184,x:33578,y:32782,pt:False;n:type:ShaderForge.SFN_LightVector,id:185,x:33578,y:32617;n:type:ShaderForge.SFN_Tex2d,id:186,x:32929,y:32542,ptlb:node_186,ptin:_node_186,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Multiply,id:289,x:32750,y:32901|A-176-RGB,B-290-RGB;n:type:ShaderForge.SFN_Tex2d,id:290,x:33058,y:33081,ptlb:ramp,ptin:_ramp,ntxv:0,isnm:False|UVIN-177-OUT;n:type:ShaderForge.SFN_LightColor,id:318,x:32831,y:33154;proporder:176-186-290;pass:END;sub:END;*/

Shader "Shader Forge/Toon" {
    Properties {
        _node_176 ("node_176", 2D) = "bump" {}
        _node_186 ("node_186", 2D) = "bump" {}
        _ramp ("ramp", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #ifndef LIGHTMAP_OFF
                sampler2D unity_Lightmap;
                float4 unity_LightmapST;
                #ifndef DIRLIGHTMAP_OFF
                    sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _node_176; uniform float4 _node_176_ST;
            uniform sampler2D _node_186; uniform float4 _node_186_ST;
            uniform sampler2D _ramp; uniform float4 _ramp_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD7;
                #else
                    float3 shLight : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                #ifdef LIGHTMAP_OFF
                    o.shLight = ShadeSH9(float4(v.normal * unity_Scale.w,1)) * 0.5;
                #endif
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Normals:
                float2 node_354 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_node_186,TRANSFORM_TEX(node_354.rg, _node_186))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                #ifndef LIGHTMAP_OFF
                    float4 lmtex = tex2D(unity_Lightmap,i.uvLM);
                    #ifndef DIRLIGHTMAP_OFF
                        float3 lightmap = DecodeLightmap(lmtex);
                        float3 scalePerBasisVector = DecodeLightmap(tex2D(unity_LightmapInd,i.uvLM));
                        UNITY_DIRBASIS
                        half3 normalInRnmBasis = saturate (mul (unity_DirBasis, normalLocal));
                        lightmap *= dot (normalInRnmBasis, scalePerBasisVector);
                    #else
                        float3 lightmap = DecodeLightmap(tex2D(unity_Lightmap,i.uvLM));
                    #endif
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef DIRLIGHTMAP_OFF
                        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    #else
                        float3 lightDirection = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
                        lightDirection = mul(lightDirection,tangentTransform); // Tangent to world
                    #endif
                #else
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                #endif
////// Lighting:
                float2 node_177 = float2(0.5*dot(lightDirection,i.normalDir)+0.5,0.0);
                float3 finalColor = (tex2D(_node_176,TRANSFORM_TEX(node_354.rg, _node_176)).rgb*tex2D(_ramp,TRANSFORM_TEX(node_177, _ramp)).rgb);
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #ifndef LIGHTMAP_OFF
                sampler2D unity_Lightmap;
                float4 unity_LightmapST;
                #ifndef DIRLIGHTMAP_OFF
                    sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _node_176; uniform float4 _node_176_ST;
            uniform sampler2D _node_186; uniform float4 _node_186_ST;
            uniform sampler2D _ramp; uniform float4 _ramp_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Normals:
                float2 node_355 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_node_186,TRANSFORM_TEX(node_355.rg, _node_186))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float2 node_177 = float2(0.5*dot(lightDirection,i.normalDir)+0.5,0.0);
                float3 finalColor = (tex2D(_node_176,TRANSFORM_TEX(node_355.rg, _node_176)).rgb*tex2D(_ramp,TRANSFORM_TEX(node_177, _ramp)).rgb);
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
