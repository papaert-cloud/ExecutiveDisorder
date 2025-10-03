Shader"UI/MovingCautionStripes"
{
    Properties
    {
        _ColorA("Stripe Color A (Yellow)", Color) = (1, 0.85, 0, 1)
        _ColorB("Stripe Color B (Black)", Color) = (0, 0, 0, 1)
        _StripeWidth("Stripe Width", Float) = 0.25
        _Speed("Scroll Speed", Float) = 0.5
        _Angle("Stripe Angle", Float) = 45
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
LOD 100
        Cull
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

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

fixed4 _ColorA;
fixed4 _ColorB;
float _StripeWidth;
float _Speed;
float _Angle;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
                // Rotate UVs to get diagonal stripes
    float angleRad = radians(_Angle);
    float2 dir = float2(cos(angleRad), sin(angleRad));
    float coord = dot(i.uv, dir);

                // Scroll the stripes
    coord += _Time.y * _Speed;

                // Create the stripe pattern
    float pattern = frac(coord / _StripeWidth);
    float stripe = step(0.5, pattern); // alternate

                // Choose color
    fixed4 color = lerp(_ColorA, _ColorB, stripe);
    color.a = 1;
    return color;
}
            ENDCG
        }
    }
}
