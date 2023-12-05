using System.Collections.Generic;
using Godot;

namespace MIJGWJ54.Scripts.Dia;

public partial class DiaUI : RichTextLabel
{
    public static DiaUI Instance;
    [Export()] public AnimatedSprite2D SkipImage;

    private List<DiaInfo> currentLongDia = new List<DiaInfo>();
    private int currentIndex = -1;

    private Tween _tween;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustReleased("DiaMove") && SkipImage.Visible) ProcessNextDialouge();
    }

    public static void StartDia(List<DiaInfo> dia)
    {
        Instance.currentLongDia.Clear();
        Instance.currentLongDia = dia;
        Instance.currentIndex = -1;
        Instance.ProcessNextDialouge();
    }

    private void ProcessNextDialouge()
    {
        if (currentLongDia == null || currentLongDia.Count == 0) return;
        if (currentIndex != -1) currentLongDia[currentIndex].EventTOnTalkEnd?.Invoke();
        currentIndex++;
        if (currentIndex >= currentLongDia.Count)
        {
            Text = "";
            SkipImage.Hide();
            return;
        }

        Text = currentLongDia[currentIndex].TextToDisplay;
        if (currentLongDia[currentIndex].CanSkip) SkipImage.Show();
        currentLongDia[currentIndex].EventTOnTalk?.Invoke();
        VisibleCharacters = -1;
        UIManager.Instance.ClickNoise.Play();
        _tween = GetTree().CreateTween();
        _tween.TweenProperty(this, "visible_characters", currentLongDia[currentIndex].TextToDisplay.Length + 4, 0.55f);
    }
}