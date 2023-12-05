using Godot;

namespace MIJGWJ54.Scripts.Customer;

public class Curse
{
    public CurseType CurseType;
    public int CursePrice;
    public float CurseProfit;

    public Curse(CurseType type, float curseProfit)
    {
        CurseType = type;
        CursePrice = type == CurseType.Luck ? 15550 : (int) type * 25 / 2;
        CurseProfit = Mathf.Clamp((curseProfit / 100) * CursePrice, MINIMUM_PROFIT, PROFIT_CAP);
    }

    public const int PROFIT_CAP = 55;
    public const int MINIMUM_PROFIT = 5;
}


public enum CurseType
{
    Luck = 0,
    Love = 1,
    Pig = 2,
    Frog = 4,
    Greed = 8,
    Hunger = 16,
    Drought = 32,
    Fear = 64,
    OnlineBlogger = 128,
    BadLuck = 256,
}