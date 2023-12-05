namespace MIJGWJ54.Scripts.Customer;

public class Customer
{
    public Curse Curse { get; set; }
    public CustomerTypes CustomerType { get; set; }
    public readonly CustomerManager.HappyStatus StartingMood;
    public readonly float CustomerPatienceTime;

    public Customer(Curse curse, CustomerTypes type, CustomerManager.HappyStatus startingMood, float patienceTime = 16f)
    {
        Curse = curse;
        CustomerType = type;
        StartingMood = startingMood;
        CustomerPatienceTime = type == CustomerTypes.Impatient ? 6f : patienceTime;
    }
}