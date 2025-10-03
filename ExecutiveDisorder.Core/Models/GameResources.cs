namespace ExecutiveDisorder.Core.Models;

public class GameResources
{
    public int Popularity { get; set; }
    public int Stability { get; set; }
    public int MediaTrust { get; set; }
    public int Economic { get; set; }

    public GameResources(int popularity, int stability, int mediaTrust, int economic)
    {
        Popularity = Clamp(popularity);
        Stability = Clamp(stability);
        MediaTrust = Clamp(mediaTrust);
        Economic = Clamp(economic);
    }

    public void ApplyEffects(int popularityEffect, int stabilityEffect, int mediaTrustEffect, int economicEffect)
    {
        Popularity = Clamp(Popularity + popularityEffect);
        Stability = Clamp(Stability + stabilityEffect);
        MediaTrust = Clamp(MediaTrust + mediaTrustEffect);
        Economic = Clamp(Economic + economicEffect);
    }

    private static int Clamp(int value) => Math.Max(0, Math.Min(100, value));

    public bool IsGameOver() => Popularity <= 0 || Stability <= 0 || MediaTrust <= 0 || Economic <= 0;
}
