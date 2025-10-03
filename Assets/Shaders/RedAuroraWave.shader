Shader"UI/AuroraWaveRedSoft"
{
    Properties
    {
        [PerRendererData]_MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Base Color", Color) = (1, 0, 0, 1)
        _WaveSpeed("Wave Speed", Float) = 1.5
        _WaveStrength("Wave Strength", Float) = 0.03
        _Frequency("Wave Frequency", Float) = 8
        _AlphaStrength("Alpha Strength", Float) = 1.0
        _EdgeFade("Edge Fade", Float) = 1.0
        _VerticalFade("Vertical Fade", Float) = 1.0
    }

    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "False"
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

sampler2D _MainTex;
float4 _MainTex_ST;
float4 _Color;
float _WaveSpeed;
float _WaveStrength;
float _Frequency;
float _AlphaStrength;
float _EdgeFade;
float _VerticalFade;

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

v2f vert(appdata_t v)
{
    v2f o;
    float time = _Time.y;
    float waveOffset = sin((v.uv.x + time * _WaveSpeed) * _Frequency) * _WaveStrength;
    v.vertex.y += waveOffset;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float waveAlpha = sin((i.uv.x + _Time.y * _WaveSpeed) * _Frequency);
    waveAlpha = saturate(waveAlpha * _AlphaStrength);

                // Horizontal soft edges
    float edge = smoothstep(0.0, _EdgeFade, i.uv.x) * smoothstep(1.0, 1.0 - _EdgeFade, i.uv.x);

                // Vertical fade (from bottom up)
    float vertical = smoothstep(0.0, _VerticalFade, i.uv.y);

    fixed4 texCol = tex2D(_MainTex, i.uv);
    texCol *= _Color;

    texCol.a *= waveAlpha * edge * vertical;
    return texCol;
}
            ENDCG
        }
    }
}
