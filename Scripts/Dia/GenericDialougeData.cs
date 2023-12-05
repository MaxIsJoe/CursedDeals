using System.Collections.Generic;

namespace MIJGWJ54.Scripts.Dia;

public static class GenericDialougeData
{
    public static List<string> RandomGeneticCriticalNoText = new List<string>()
    {
        "That's a terrible price!",
        "I could find a better deal elsewhere!",
        "You witches are all the same! Evil!",
        "I have a family to feed! Think of them!",
        "Is this some prank by the court jester?",
        "A robbery would feel much better than what you're proposing..",
        "This should be illegal!",
    };
    
    public static List<string> RandomGenericNoText = new List<string>()
    {
        "Can you give me a discount since its my first time?",
        "Please.. That's a bit too much!",
        "That's a little bit higher than what I've heard it would cost..",
        "I guess you also need to make a living.. I understand.",
    };
    
    public static List<string> RandomGenericCriticalYesText = new List<string>()
    {
        "You've got yourself a deal!",
        "How generous of you!",
        "I won't forget this",
        "Thank you!",
        "You're a saviour",
        "Its great to see kindness in these lands again..",
    };
    
    public static List<string> RandomGenericYesText = new List<string>()
    {
        "Rid me of this curse, please!",
        "I have the money, shake hands?",
        "This seems reasonable",
        "Alright, can we start now?",
    };

    public static List<DiaInfo> MrCapitalTutorialInfo = new List<DiaInfo>()
    {
        new DiaInfo() { CanSkip = true, EventTOnTalk = UIManager.Instance.TutorialHideEverythingGameplayRelated, TextToDisplay = $"[center][b]Mr.Capital:[/b]\n Why hello! Its been a while, Mrs. {Game.Instance.PlayerNameFromPC} the witch." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n I see that you've done some renovation to my tower. Which I quite like!" },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n But I'm not here to have a chitchat about decoration, let us talk about what's important here." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n How to make money." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n So you can pay your rent to me and I leave you alone for another year to enjoy your always temporary place of residence.." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = UIManager.Instance.TutorialShowNextGameplayUIStuff, TextToDisplay = "[center][b]Mr.Capital[/b]\n Let us start with the basics. Every curse you'll lift has a material cost, Your job is to try and haggle for as much profits as possible so that you'd earn some money to pay your rent." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n Usually, Wizards and Witches charge a % that is universally known as 'market profit'." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n You'll need to haggle for that extra profit ontop of the material costs to make money." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n I don't really care if you can extort or trick these desperate villagers to make some profits;" },
        new DiaInfo() { CanSkip = true, EventTOnTalk = UIManager.Instance.TutorialShowNextGameplayUIStuff, TextToDisplay = "[center][b]Mr.Capital[/b]\n But be careful, charging above that % can make some customers angry." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n Building a bad reputation for your business and yourself is a double edged sword, so keep your Karma in check. Because I need that money..;" },
        new DiaInfo() { CanSkip = true, EventTOnTalk = Game.Instance.ShowCustomerMoodIndicator, TextToDisplay = "[center][b]Mr.Capital[/b]\n Pay attention to your customers' types and their moods, an angry customer is more likely to harm your reputation." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n Different customer types come with different interactions, for example: An impatient customer will get angry if you haggle too much with them." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n While a rich customer is willing to pay 50% more than known market profit % before they get angry." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n You'll learn all of them in time, via trial and error.." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n I'm sure 7 days is enough for you to figure out how things work." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = UIManager.Instance.TutorialShowNextGameplayUIStuff, TextToDisplay = "[center][b]Mr.Capital[/b]\n Anyways, Increase or decrease the asking price to your Heart's Content, don't worry about these fools not having enough money; they'll find a way." },
        new DiaInfo() { CanSkip = true, EventTOnTalk = UIManager.Instance.TutorialShowNextGameplayUIStuff, TextToDisplay = "[center][b]Mr.Capital[/b]\n But be mindful of everyone's times. Business is on a very tight time schedule so don't waste anyone's time!" },
        new DiaInfo() { CanSkip = true, EventTOnTalk = null, TextToDisplay = "[center][b]Mr.Capital[/b]\n And with that, I offer my best of wishes to you; Have fun making money.", EventTOnTalkEnd = Game.Instance.EndDay},
    };
}