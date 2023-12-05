using Godot;
using System;

public partial class MainMenu : Node
{
	[Export()] public Button ExitButton;
	
	public override void _Ready()
	{
		if (OS.GetDistributionName() == String.Empty)
		{
			ExitButton.Hide();
		}
	}

	public void Play()
	{
		
	}

	public void Exit()
	{
		GetTree().Quit();
	}
}
