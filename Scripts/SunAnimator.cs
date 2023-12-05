using Godot;

namespace MIJGWJ54.Scripts;

public partial class SunAnimator : DirectionalLight3D
{
    public static SunAnimator Instance;
    [Export()] public float DayTime { get; set; } = 85;
    
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public void AnimateSun()
    {
        Tween newTween = GetTree().CreateTween();
        newTween.TweenProperty(this, "rotation_degrees", new Vector3(0,0,0), DayTime).From(new Vector3(-35, 108.2f, -162.5f)).SetTrans(Tween.TransitionType.Cubic);
        newTween.Finished += OnEnd;
        newTween.Play();
    }

    public void OnEnd()
    {
        Game.Instance.EndDay();
    }
}