using System;
using Godot;
using Range = Godot.Range;

namespace MIJGWJ54.Scripts;

public partial class NumberIndicator : Control
{
    [Export()] private RichTextLabel numbersText;
    [Export()] private AnimationPlayer _player;
    
    
    public void Animate(string text)
    {
        //translated from gdscript from this https://www.youtube.com/watch?v=zGng3u9Y6dg
        numbersText.Text = text;
        var tween = GetTree().CreateTween();
        var random = new Random();
        var end_pos = new Vector2(random.Next((int)Position.X - 5, (int)Position.X + 5), random.Next((int)Position.Y, (int)Position.Y + 10));
        tween.TweenProperty(this, "position", end_pos, _player.CurrentAnimationLength)
            .SetTrans(Tween.TransitionType.Cubic);
    }
}