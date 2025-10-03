Shader"UI/TransparentGrainOverlay"
{
    Properties
    {
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.3
        _Speed ("Noise Speed", Range(0.1, 10)) = 1
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane" 
            "CanUseSpriteAtlas"="False" 
        }

LOD 100

        Cull
Off
        Lighting
Off
        ZWrite
Off
        Blend
SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

float _NoiseIntensity;
float _Speed;

struct appdata_t
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

float random(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float2 noiseUV = i.uv * 480.0;
    float noise = random(noiseUV + _Time.y * _Speed);
    float grain = noise * _NoiseIntensity;

                // Output grayscale grain with matching alpha
    return fixed4(grain, grain, grain, grain);
}
            ENDCG
        }
    }

FallBack"UI/Default"
}
