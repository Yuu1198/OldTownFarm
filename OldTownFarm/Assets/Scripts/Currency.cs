using UnityEngine;
using System;
using Unity.VisualScripting;

[Serializable]
public struct Currency
{
    public int gold;
    public int silver;
    public int copper;

    private const int CopperPerSilver = 100;
    private const int SilverPerGold = 100;

    public Currency(int  gold, int silver, int copper)
    {
        this.gold = gold;
        this.silver = silver;
        this.copper = copper;
        Normalize();
    }

    // Convert overflow copper/silver into higher currencies
    private void Normalize()
    {
        if (copper >= CopperPerSilver)
        {
            silver += copper / CopperPerSilver;
            copper %= CopperPerSilver;
        }

        if (silver >= SilverPerGold)
        {
            gold += silver / SilverPerGold;
            silver %= SilverPerGold;
        }
    }

    public void Add(Currency other)
    {
        gold += other.gold;
        silver += other.silver;
        copper += other.copper;
        Normalize();
    }

    public void Substract(Currency other)
    {
        int totalCopper = ToCopper() - other.ToCopper();

        if (totalCopper < 0)
        {
            totalCopper = 0;
        }

        FromCopper(totalCopper);
    }

    public int ToCopper()
    {
        return (gold * SilverPerGold * CopperPerSilver) + (silver * CopperPerSilver) + copper;
    }

    public void FromCopper(int totalCopper)
    {
        gold = totalCopper / (SilverPerGold * CopperPerSilver);
        totalCopper %= SilverPerGold * CopperPerSilver;

        silver = totalCopper / CopperPerSilver;
        copper = totalCopper % CopperPerSilver;
    }

    public string GetMoneyString()
    {
        return gold.ToString() + "g" + silver.ToString() + "s" + copper.ToString() + "c";
    }
}
