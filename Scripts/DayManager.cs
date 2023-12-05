using System;
using System.Collections.Generic;
using Godot;
using MIJGWJ54.Scripts.Customer;
using Range = Godot.Range;

namespace MIJGWJ54.Scripts;

public partial class DayManager : Node
{
    public static DayManager Instance;
    public int CurrentDay = -1;
    private Random _random = new Random();

    public override void _Ready()
    {
        Instance = this;
        base._Ready();
    }


    public void SetupNextDay()
    {
        CurrentDay++;
        if (CurrentDay >= 8)
        {
            void DisplayEnding()
            {
                Game.Instance.GameEndingUI.ReadEnding();
            }
            Game.Instance.GameUI.Hide();
            Game.Instance.ShowHideTranstionInfo.Hide();
            Game.Instance.GameEndingUI.Show();
            Timer newTimer = new Timer();
            newTimer.Timeout += DisplayEnding;
            AddChild(newTimer);
            newTimer.Start(4);
            Game.Instance.GameMusic.Stop();
        }
        switch (CurrentDay)
        {
            case 0:
                GenerateCustomersForToday(new List<CustomerTypes>() { CustomerTypes.Simple });
                break;
            case 1:
                GenerateCustomersForToday(new List<CustomerTypes>() { CustomerTypes.Simple , CustomerTypes.Impatient});
                break;
            case 2:
                GenerateCustomersForToday(new List<CustomerTypes>()
                {
                    CustomerTypes.Simple , CustomerTypes.Impatient, CustomerTypes.Chill
                });
                break;
            case 3:
                GenerateCustomersForToday(new List<CustomerTypes>()
                {
                    CustomerTypes.Simple , CustomerTypes.Evil, CustomerTypes.Grumpy
                });
                break;
            case 4:
                GenerateCustomersForToday(new List<CustomerTypes>()
                {
                    CustomerTypes.Simple , CustomerTypes.Impatient, 
                    CustomerTypes.Chill, CustomerTypes.Evil, 
                    CustomerTypes.Grumpy, CustomerTypes.Pure
                });
                break;
            default:
                GenerateCustomersForToday(new List<CustomerTypes>()
                {
                    CustomerTypes.Simple , CustomerTypes.Impatient, 
                    CustomerTypes.Chill, CustomerTypes.Evil, 
                    CustomerTypes.Grumpy, CustomerTypes.Pure, CustomerTypes.Cheap, CustomerTypes.Rich
                });
                break;
        }

        Game.Instance.StartNextDay();
    }

    public void GenerateCustomersForToday(List<CustomerTypes> allowedTypes, int numberOfCustomers = 65)
    {
        var customerList = new List<Customer.Customer>();
        for (int i = 0; i < numberOfCustomers; i++)
        {
            var newCustomer = new Customer.Customer(new Curse(GetRandomCurse(), 
                _random.Next(5,35)), GetRandomCustomerType(allowedTypes), GetRandomMood());
            customerList.Add(newCustomer);
        }

        CustomerManager.Instance.QueuTodaysCustomers(customerList);
    }

    private CurseType GetRandomCurse()
    {
        Array curseValues = Enum.GetValues(typeof(CurseType));
        return (CurseType)curseValues.GetValue(_random.Next(curseValues.Length))!;
    }

    private CustomerManager.HappyStatus GetRandomMood()
    {
        Array customerTypeValues = Enum.GetValues(typeof(CustomerManager.HappyStatus));
        return (CustomerManager.HappyStatus)customerTypeValues.GetValue(_random.Next(customerTypeValues.Length))!;
    }

    private CustomerTypes GetRandomCustomerType(List<CustomerTypes> allowedTypes)
    {
        return (CustomerTypes) allowedTypes.ToArray().GetValue(_random.Next(allowedTypes.Count))!;
    }
}