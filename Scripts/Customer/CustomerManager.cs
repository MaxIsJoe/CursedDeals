using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace MIJGWJ54.Scripts.Customer;

public partial class CustomerManager : Node
{
    public static CustomerManager Instance;
    public Customer CurrentCustomer { get; private set; }
    public HappyStatus CustomerMood { get; private set; } = HappyStatus.Neutral;
    public int numberOfTimesDealChanged = 0;

    private Queue<Customer> _customersToday = new Queue<Customer>();


    public Action UpdateDealInfo;
    public Action DealAltered;
    public Action<int> DealMade; 
    public Action DealRejected;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        DealMade += MakeDeal;
        DealRejected += DealReject;
        DealAltered += OnDealAltered;
        //Timer timer = new Timer();
        //AddChild(timer);
        //timer.OneShot = true;
        //timer.Timeout += DebugDayGenerator;
        //timer.Start(0.5);
    }

    private void OnDealAltered()
    {
        numberOfTimesDealChanged++;
    }

    private void DebugDayGenerator()
    {
        DayManager.Instance.GenerateCustomersForToday(new List<CustomerTypes>()
        {
            CustomerTypes.Evil,
            CustomerTypes.Chill,
            CustomerTypes.Pure,
            CustomerTypes.Impatient,
            CustomerTypes.Simple,
            CustomerTypes.Rich,
        });
        PrepareNextCustomer();
    }

    public void PrepareNextCustomer()
    {
        if (_customersToday.Count <= 0)
        {
            Game.Instance.EndDay();
            return;
        }

        if (CurrentCustomer is { CustomerType: CustomerTypes.Pure })
        {
            var profit = GetProfit(UIManager.Instance.price, CurrentCustomer.Curse.CursePrice) * 100;
            if (profit >= 75) Player.Player.Instance.Karma -= 25;
        }
        
        GD.Print("Preparing Next Customer");
        var nextCustomer = _customersToday.Dequeue();
        CurrentCustomer = nextCustomer;
        CustomerMood = CurrentCustomer.StartingMood;
        UIManager.Instance.price = Instance.CurrentCustomer.Curse.CursePrice;
        if (UIManager.Instance.price > 0) UIManager.Instance.previousPrice = Instance.CurrentCustomer.Curse.CursePrice;
        numberOfTimesDealChanged = 0;
        UpdateDealInfo?.Invoke();
        UpdateHappiness(UIManager.Instance.price);
        UIManager.Instance.StartCustomerTimer();
        Game.Instance.PlayCustomerInAnim();
        Game.Instance.UpdateMoodIndicator();
    }

    public void QueuTodaysCustomers(List<Customer> customers)
    {
        Instance._customersToday.Clear();
        foreach (var customer in customers)
        {
            Instance._customersToday.Enqueue(customer);
        }
    }

    private void MakeDeal(int amount)
    {
        Player.Player.Instance.Money += amount;
        Player.Player.Instance.Karma += HappinessToKarma();
        UIManager.Instance.ShowIndicator(amount, UIManager.Instance.CurrentMoney);
        UIManager.Instance.ShowIndicator(HappinessToKarma(), UIManager.Instance.CurrentKarma);
        Game.Instance.PlayCustomerOutAnim();
    }

    public void UpdateHappiness(int price)
    {
        var profit = GetProfit(price, CurrentCustomer.Curse.CursePrice) * 100;
        if (CurrentCustomer.CustomerType is CustomerTypes.Simple or CustomerTypes.Grumpy)
        {
            GD.Print("Normal Type customers, using regular calculations.");
            
            if (profit > CurrentCustomer.Curse.CurseProfit)
            {
                CustomerMood = HappyStatus.Angry;
            }

            if (profit <= 0)
            {
                CustomerMood = HappyStatus.Happy;
            }
        
            if (profit > 0 && profit < CurrentCustomer.Curse.CurseProfit)
            {
                CustomerMood = HappyStatus.Neutral;
            }
            Game.Instance.UpdateMoodIndicator();
            PlayMoodNoise();
            return;
        }
        GD.Print("Special customer");
        CustomerMood = MoodFromType(profit);
        Game.Instance.UpdateMoodIndicator();
        //Game.Instance.PlayCustomerInAnim();
        PlayMoodNoise();
    }

    private void PlayMoodNoise()
    {
        if (CurrentCustomer.CustomerType == CustomerTypes.Grumpy) Game.Instance.MoodAngryNoise.Play();
        switch (CustomerMood)
        {
            case HappyStatus.Happy:
                Game.Instance.MoodHappyNoise.Play();
                break;
            case HappyStatus.Angry:
                Game.Instance.MoodAngryNoise.Play();
                break;
            case HappyStatus.Neutral:
                Game.Instance.MoodNeturalNoise.Play();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private HappyStatus MoodFromType(float profit)
    {
        switch (CurrentCustomer.CustomerType)
        {
            case CustomerTypes.Impatient:
                GD.Print($"Number of times deal changed = {numberOfTimesDealChanged}. {(numberOfTimesDealChanged >= 2 ? HappyStatus.Angry : HappyStatus.Neutral)}");
                return numberOfTimesDealChanged >= 2 ? HappyStatus.Angry : HappyStatus.Neutral;
            case CustomerTypes.Rich:
                GD.Print($"Rich gets angry if they're getting a deal that's worth 2 times the market profit. {profit >= Curse.PROFIT_CAP + Curse.PROFIT_CAP}");
                if (profit >= Curse.PROFIT_CAP + Curse.PROFIT_CAP) return HappyStatus.Angry; 
                return profit < 0 ? HappyStatus.Happy : HappyStatus.Neutral;
            case CustomerTypes.Chill:
                GD.Print($"Chill always neutral.");
                return profit < 0 ? HappyStatus.Happy : HappyStatus.Neutral;
            case CustomerTypes.Cheap:
                GD.Print($"Cheap is angry: {(profit > CurrentCustomer.Curse.CurseProfit / 2 ? HappyStatus.Angry : HappyStatus.Happy)}.");
                return profit >= CurrentCustomer.Curse.CurseProfit / 2 ? HappyStatus.Angry : HappyStatus.Happy;
            case CustomerTypes.Evil:
                return HappyStatus.Angry;
            case CustomerTypes.Pure:
                return HappyStatus.Happy;
            case CustomerTypes.Grumpy:
                break;
            case CustomerTypes.Simple:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return HappyStatus.Neutral;
    }

    private void DealReject()
    {
        Player.Player.Instance.Karma -= 5;
        Game.Instance.PlayCustomerOutAnim();
    }
    
    public static float GetProfit(int currentPrice, int previousPrice)
    {
        return (float)((currentPrice - previousPrice) / (double)previousPrice);
    }

    private int HappinessToKarma()
    {
        return CustomerMood switch
        {
            HappyStatus.Happy => 4,
            HappyStatus.Angry => -8,
            HappyStatus.Neutral => 1,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public enum HappyStatus
    {
        Happy,
        Angry,
        Neutral
    }
}