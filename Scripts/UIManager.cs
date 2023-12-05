using System;
using Godot;
using MIJGWJ54.Scripts.Customer;

namespace MIJGWJ54.Scripts;

public partial class UIManager : Control
{
    public static UIManager Instance;
    [Export()] public Button DealButton;
    [Export()] public Button RejectDealButton;
    [Export()] public ProgressBar TimerBar;
    [Export()] public RichTextLabel CurrentMoney;
    [Export()] public RichTextLabel CurrentKarma;
    [Export()] public RichTextLabel CurrentPrice;
    [Export()] public RichTextLabel BasePriceText;
    [Export()] public RichTextLabel SellerTypeText;
    [Export()] public RichTextLabel MarketProfitText;
    [Export()] public RichTextLabel BuyerTypeText;
    [Export()] public SpinBox SpinBoxStuff;
    [Export()] public Control GameplayStuff;

    [Export()] public AudioStreamPlayer DecreasePriceNoise;
    [Export()] public AudioStreamPlayer IncreasePriceNoise;
    [Export()] public AudioStreamPlayer DealMadeNoiseMoney;
    [Export()] public AudioStreamPlayer NoMoneyMadeNoise;
    [Export()] public AudioStreamPlayer ClickNoise;
    [Export()] public AudioStreamPlayer ShowNoise;

    [Export()] public PackedScene IndicatorNumbersScene;

    [Export()] public Timer CustomerTimer;

    public int price { get; set; } = 5;
    public int previousPrice { get; set; } = 5;
    public int amountToAddOrRemoveToDeal { get; set; } = 5;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        SpinBoxStuff.Value = 5;
        CustomerManager.Instance.UpdateDealInfo += UpdateGeneralUI;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (CustomerTimer.IsStopped() == false) TikTok();
        if (Input.IsActionJustReleased("SpinUp"))
        {
            SpinBoxStuff.Value += 2;
            ClickNoise.Play();
        }

        if (Input.IsActionJustReleased("SpinDown"))
        {
            SpinBoxStuff.Value -= 2;
            ClickNoise.Play();
        }
    }

    public void UpdateGeneralUI()
    {
        BasePriceText.Text = $"Cost of Materials: {CustomerManager.Instance.CurrentCustomer.Curse.CursePrice}";
        BuyerTypeText.Text = $"Buyer Type: {Enum.GetName(typeof(CustomerTypes), CustomerManager.Instance.CurrentCustomer.CustomerType)}";
        MarketProfitText.Text = $"Market Profit: {CustomerManager.Instance.CurrentCustomer.Curse.CurseProfit}%";
        CurrentMoney.Text = $"[center]Money: {Player.Player.Instance.Money.ToString()}";
        CurrentKarma.Text = $"[center]Karma: {GetKarmaText()}";
        CurrentPrice.Text = $"[center]Asking Price: {price} ({GetProfit()})";
        EnableDealButtons();
    }

    private string GetKarmaText()
    {
        var color = "";
        if (Player.Player.Instance.Karma < 15) color = "red";
        if (Player.Player.Instance.Karma > Game.Instance.WinConditionKarma) color = "green";
        if (Player.Player.Instance.Karma <= Game.Instance.WinConditionKarma && Player.Player.Instance.Karma >= 15) color = "yellow";
        return $"[color={color}]{Player.Player.Instance.Karma.ToString()}[/color]";
    }
    
    private string GetProfit()
    {
        return ((price - previousPrice) / (double)previousPrice).ToString("p");
    }

    public void OnDeal()
    {
        GD.Print("click on deal");
        if (price > CustomerManager.Instance.CurrentCustomer.Curse.CursePrice) DealMadeNoiseMoney.Play();
        if (price <= CustomerManager.Instance.CurrentCustomer.Curse.CursePrice) NoMoneyMadeNoise.Play();
        CustomerManager.Instance.DealMade?.Invoke(price - CustomerManager.Instance.CurrentCustomer.Curse.CursePrice);
        CustomerTimer.Stop();
        DisableDealButtons();
    }
    
    public void OnNoDeal()
    {
        GD.Print("click on no deal");
        CustomerManager.Instance.DealRejected?.Invoke();
        DisableDealButtons();
    }

    private void DisableDealButtons()
    {
        DealButton.Disabled = true;
        RejectDealButton.Disabled = true;
    }
    
    private void EnableDealButtons()
    {
        DealButton.Disabled = false;
        RejectDealButton.Disabled = false;
    }

    public void AskForMoreMoney()
    {
        price += (int) SpinBoxStuff.Value;
        GD.Print($"click on more money. Price: {price}");
        CustomerManager.Instance.UpdateHappiness(price);
        CustomerManager.Instance.DealAltered?.Invoke();
        UpdateGeneralUI();
        IncreasePriceNoise.Play();
    }
    
    public void BeKind()
    {
        price -= (int) SpinBoxStuff.Value;
        GD.Print($"click on less money. Price: {price}");
        CustomerManager.Instance.UpdateHappiness(price);
        CustomerManager.Instance.DealAltered?.Invoke();
        UpdateGeneralUI();
        DecreasePriceNoise.Play();
    }

    public void StartCustomerTimer()
    {
        TimerBar.MaxValue = CustomerManager.Instance.CurrentCustomer.CustomerPatienceTime;
        CustomerTimer.Start(CustomerManager.Instance.CurrentCustomer.CustomerPatienceTime);
    }

    public void TikTok()
    {
        TimerBar.Value = CustomerTimer.TimeLeft;
    }

    public void Timeout()
    {
        OnNoDeal();
    }

    public void TutorialHideEverythingGameplayRelated()
    {
        foreach (var child in GameplayStuff.GetChildren())
        {
            if (child is Control c) c.Hide();
        }
    }

    public void TutorialShowNextGameplayUIStuff()
    {
        foreach (var child in GameplayStuff.GetChildren())
        {
            if (child is not Control c) continue;
            if (c.Visible) continue;
            c.Show();
            ShowNoise.Play();
            if (c is ProgressBar bar)
            {
                bar.Value = 30;
                bar.MaxValue = 30;
                CustomerTimer.Start(30);
            }
            return;
        }
    }

    public void ShowIndicator(int number, RichTextLabel onNode)
    {
        NumberIndicator indictator = IndicatorNumbersScene.Instantiate() as NumberIndicator;
        indictator.Position = onNode.GlobalPosition;
        AddChild(indictator);
        var numberToReport = number.ToString();
        if (number > 0) numberToReport += "+";
        indictator?.Animate(numberToReport);
    }
}