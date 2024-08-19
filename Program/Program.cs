namespace Program;

class Program
{
    public enum TypeField
    {
        Second = 0,
        Minute = 1,
        Hour = 2,
        Day = 3,
        Month = 4,
        DayOfWeek = 5
    }
    
    static void Main(string[] args)
    {
        TypeField a = TypeField.Month;


            Console.WriteLine($"{(int)a}");
        
    }
}