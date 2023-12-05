using System;
using Godot;

namespace MIJGWJ54.Scripts;

public partial class GameEndText : Control
{
	[Export()] public RichTextLabel EndingText;
	[Export()] public RichTextLabel ThanksForPlayingText;

	private const string AmazingKarmaWinText = 
		$"{YouLostText}" +
		"[color=green]but before the thought of you spending the night in the rain sets in, a familiar person calls out to you. One of your customers." +
		"They let you into their humble shop and let you rest there for the evening, and as soon as the townsfolk hear about your evection; they all gather the next day to offer help for you.\n\n" +
		"The townsfolk grant you a smaller tower that resides within the center of the city, it's old and dusty but you no longer have to pay rent. \n\n" +
		"You spend the rest of your time helping the townsfolk like you used to, forgetting about money as they share their food with you as repayment for all the good you do. \n\n" +
		"You are happy.[/color]";
	
	private const string NormalBadEnding = 
		$"{YouLostText}" +
		"[color=red]you look around for shelter, but no one has an extra space to take you in.\n\n" +
		"Everyone is too busy looking after themselves, their kindness caged behind their worries.\n" +
		"And with an unwelcomed hand you raise a wand and cast a spell; It's time to reach to the Murraies. \n" +
		"Abandoned by the sea, a physical line seperating you like a bourgeoisie. \n" +
		"No ryhmes or rythm can be heard, an echo of your pleas. \n\n" +
		"You lost everything and have cursed an abandoned settlement by the sea.[/color]";
	
	private const string NormalGoodEndingText =
		"[center]Your hard work is paid off and you finally paid your rent!\n\n" +
		"Though, Mr.Capital did raise your rent and you'll have to pay double next year.\n" +
		"But luckily, business is booming as people seem to keep getting cursed reguraly in this town. \n" +
		"You don't really think about it, you just work everyday from your home..\n" +
		"..Until the cycle repeats itself again..\n\n" +
		"You have 4 walls and a roof to sleep under, and enough money to make sure your life is stable for a while. " +
		"But is that all? Is this your life from now on?[/center]";

	private const string LowKarma =
		"[color=red][center]The townsfolk looked up a tower, owned and funded by greed.\n" +
		"The source of their problems slept soundly, they agreed.\n" +
		"Armed with pitchforks and muskets, they went to quinch their needs.\n" +
		"A blind fit of justice. Surely, the curses will be lifted. It is guaranteed.\n" +
		"They loot and burn, and you are put trial for your deeds.\n" +
		"You try to reason with the crowd, but anger listens to no pleads.\n\n" +
		"You are left hanging, asking, what went wrong? " +
		"Aren't you a victim like them? A cursed pawn? " +
		"But before you can finish your song.. The shouts muffle and coldness washes over you.";
	
	private const string GreedEnding =
		"[center][color=red] The townsfolk looked up a tower, owned and funded by greed.\n" +
		"The source of their problems slept soundly, they agreed.\n" +
		"Armed with pitchforks and muskets, they went to quinch their needs.\n" +
		"A blind fit of justice. Surely, the curses will be lifted. It is guaranteed.[/color]\n\n" +
		"[color=yellow]..But..\n\n" +
		"Champion of greed, saved by their value.\n" +
		"An invalubale asset, the curse of greed had you.\n" +
		"He does not need to command you.\n" +
		"For he knows, Champion of greed, infinintly invaluable.\n\n" +
		"You've become Mr.Capital's champion. A demon of greed.";

	private const string YouLostText =
		"[center]You failed. Mr.Capital threw you out of your tower because you couldn't pay your rent on time.\n\n" +
		"You wander the streets aimlessly as you're ensure about what to do next, still trying to understand the fact that you won't be able to return back home. \n\n" +
		"As you continue walking, you notice the wind getting stronger and the sky getting darker; It is about to rain.\n";
	
	
	public void ReadEnding()
	{
		Tween tween = GetTree().CreateTween();
		EndingText.VisibleCharacters = 0;
		var timeToRead = 5;
		switch (CheckWhichEnding())
		{
			case EndingType.AmazingKarma:
				EndingText.Text = AmazingKarmaWinText;
				break;
			case EndingType.NormalGoodEnding:
				EndingText.Text = NormalGoodEndingText;
				break;
			case EndingType.NormalBadEnding:
				EndingText.Text = NormalBadEnding;
				timeToRead = 15;
				break;
			case EndingType.LowKarma:
				EndingText.Text = LowKarma;
				timeToRead = 25;
				break;
			case EndingType.Greed:
				EndingText.Text = GreedEnding;
				timeToRead = 30;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		GD.Print(CheckWhichEnding());
		tween.TweenProperty(EndingText, "visible_characters", EndingText.Text.Length + 4, timeToRead);
		MouseFilter = MouseFilterEnum.Stop;
	}

	private EndingType CheckWhichEnding()
	{
		if (Player.Player.Instance.Karma < -25 && Player.Player.Instance.Money > Game.Instance.WinConditionMoney) return EndingType.Greed;
		if (Player.Player.Instance.Karma <= 15) return EndingType.LowKarma;
		if (Player.Player.Instance.Money >= Game.Instance.WinConditionMoney) return EndingType.NormalGoodEnding;
		if (Player.Player.Instance.Money < Game.Instance.WinConditionMoney && Player.Player.Instance.Karma >= Game.Instance.WinConditionKarma) return EndingType.AmazingKarma;
		if (Player.Player.Instance.Money < Game.Instance.WinConditionMoney) return EndingType.NormalBadEnding;
		return EndingType.NormalBadEnding;
	}
	
	private enum EndingType
	{
		AmazingKarma,
		NormalGoodEnding,
		NormalBadEnding,
		LowKarma,
		Greed
	}
}