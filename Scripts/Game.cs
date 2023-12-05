using System;
using Godot;
using MIJGWJ54.Scripts.Customer;
using MIJGWJ54.Scripts.Dia;

namespace MIJGWJ54.Scripts;

public partial class Game : Node3D
{
    public static Game Instance;
    [Export()] public Control GameUI;
    [Export()] public GameEndText GameEndingUI;
    [Export()] public Node3D World;
    [Export()] public Sprite3D Customer;
    [Export()] public AnimatedSprite3D CustomerMoodIndicator;
    [Export()] public AnimationPlayer AnimationPlayerForCustomer;
    [Export()] public ColorRect ShowHideShader;
    [Export()] public RichTextLabel ShowHideTranstionInfo;
    [Export()] public AudioStreamPlayer LeaveNoise;
    [Export()] public AudioStreamPlayer MoodNeturalNoise;
    [Export()] public AudioStreamPlayer MoodHappyNoise;
    [Export()] public AudioStreamPlayer MoodAngryNoise;
    
    [Export()] public AudioStreamPlayer TutorialMusic;
    [Export()] public AudioStreamPlayer GameMusic;

    private Tween customerInTween;
    
    public string PlayerNameFromPC { get; private set; }
    public int WinConditionMoney { get; private set; } = 17550;
    public int WinConditionKarma { get; private set; } = 150;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        if (OS.HasEnvironment("USERNAME"))
        {
            PlayerNameFromPC = OS.GetEnvironment("USERNAME");
        }
        else
        {
            PlayerNameFromPC = "Nemona";
        }
        SetupNextDay(StartTutorialDay);
    }

    public void SetupNextDay(Action NextDayCommand, float animTime = 7.5f)
    {
        ShowHideTranstionInfo.Text = TransationInfo();
        Timer timer = new Timer();
        timer.Timeout += NextDayCommand;
        AddChild(timer);
        timer.OneShot = true;
        timer.Start(animTime);
    }

    private void StartTutorialDay()
    {
        Tween newTween = GetTree().CreateTween();
        newTween.TweenProperty(ShowHideShader.Material, "shader_parameter/circle_size", 1, 0.25f);
        newTween.TweenProperty(ShowHideTranstionInfo, "modulate", new Color(0,0,0,0), 0.15f);
        newTween.Play();
        DiaUI.StartDia(GenericDialougeData.MrCapitalTutorialInfo);
        TutorialMusic.Play();
    }

    public void StartNextDay()
    {
        Tween newTween = GetTree().CreateTween();
        newTween.TweenProperty(ShowHideShader.Material, "shader_parameter/circle_size", 1, 0.35f);
        newTween.TweenProperty(ShowHideTranstionInfo, "modulate", new Color(0,0,0,0), 0.25f);
        newTween.Finished += () => newTween.Kill();
        newTween.Play();
        UIManager.Instance.GameplayStuff.Show();
        CustomerManager.Instance.PrepareNextCustomer();
        SunAnimator.Instance.AnimateSun();
        if (DayManager.Instance.CurrentDay == 0)
        {
            TutorialMusic.Stop();
            GameMusic.Play();
        }
    }

    public void EndDay()
    {
        UIManager.Instance.GameplayStuff.Hide();
        SetupNextDay(DayManager.Instance.SetupNextDay);
        Tween newTween = GetTree().CreateTween();
        newTween.TweenProperty(ShowHideShader.Material, "shader_parameter/circle_size", 0, 0.35f);
        newTween.TweenProperty(ShowHideTranstionInfo, "modulate", new Color(1,1,1,1), 0.25f);
        newTween.Finished += () => newTween.Kill();
        newTween.Play();
    }

    public void UpdateMoodIndicator()
    {
        switch (CustomerManager.Instance.CurrentCustomer.CustomerType)
        {
            case CustomerTypes.Evil:
                CustomerMoodIndicator.Play("Evil");
                return;
            case CustomerTypes.Grumpy:
                CustomerMoodIndicator.Play("Grumpy");
                return;
            case CustomerTypes.Impatient:
                CustomerMoodIndicator.Play("Impatient");
                return;
            default:
                CheckMoodNormally();
                break;
        }
    }

    private void CheckMoodNormally()
    {
        switch (CustomerManager.Instance.CustomerMood)
        {
            case CustomerManager.HappyStatus.Happy:
                CustomerMoodIndicator.Play("Happy");
                break;
            case CustomerManager.HappyStatus.Angry:
                CustomerMoodIndicator.Play("Angry");
                break;
            case CustomerManager.HappyStatus.Neutral:
                CustomerMoodIndicator.Play("Neutral");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnAnimationFinish()
    {
        CustomerManager.Instance.PrepareNextCustomer();
        LeaveNoise.Play();
    }

    public void PlayCustomerOutAnim()
    {
        if(customerInTween != null) customerInTween.Kill();
        customerInTween = null;
        customerInTween = GetTree().CreateTween();
        customerInTween.TweenProperty(Customer, "offset", new Vector2(350, 0), 0.35f);
        customerInTween.TweenProperty(Customer, "modulate", new Color(1,1,1,0), 0.25f);
        customerInTween.Finished += OnAnimationFinish;
        customerInTween.Play();
        UIManager.Instance.CustomerTimer.Stop();
    }

    public void PlayCustomerInAnim()
    {
        if(customerInTween != null) customerInTween.Kill();
        customerInTween = null;
        customerInTween = GetTree().CreateTween();
        Customer.Offset = Vector2.Zero;
        //ruber ducky time because this fucking shit is making me lsoe my god damn mind
        customerInTween.TweenProperty(Customer, //We're tweening the customer
                "modulate", //We're tweening their modulate
                new Color(1,1,1,1), //Their modulate is a color, and their color contains the alpha value which goes from 1 to 0 and vice versa.
                0.5f) //the duration of the tween
            .From(new Color(1,1,1,0)) //we tween it from 0 to 1
            .SetTrans(Tween.TransitionType.Cubic); //to make it look smoother
        customerInTween.Play();
    }

    private string TransationInfo()
    {
        var day = DayManager.Instance.CurrentDay + 1 == 8 ? "Final day" : $"Day {DayManager.Instance.CurrentDay+1} out of 7";
        return $"[center]{day}. \n Money: {Player.Player.Instance.Money} out of {WinConditionMoney} [/center]";
    }

    public void ShowCustomerMoodIndicator()
    {
        CustomerMoodIndicator.Show();
    }
}