using System.Runtime.InteropServices;

namespace CronLib;

public class PCron
{
    
    
    public DateTime GetNext(DateTime t)
    {
        
        return DateTime.Now;
    }
    /// <summary>
    /// Create 6 part of the PCron schedule: date of week, month, day, hour, minute, second
    /// </summary>
    public static PCron Parse<T>(string input)
    {
        
        return TryParse(input, (PCron p) => p);
    }

    private static T TryParse<T>(string input, Func<PCron, T> valueSelector)
    {
        // handle input
        List<PCronField> listPCronField = new List<PCronField>();
        string[] listExpression = input.Split(" ");
        if (listExpression.Length != 6)
            throw new Exception("Invalid expression!");
        for (int i = 0; i < 6; i++)
        {
            var kind = (TypeField)i;
            PCronField field = PCronField.TryParse<T>(kind, listExpression[i], (PCronField p) => p);
        }
        return valueSelector(new PCron(listPCronField));
    }
    
    // public static PCron Parse(string input)
    // {
    //     // handle input    
    //     return new PCron();
    // }
    
    PCron(List<PCronField> listPCronField)
    {
        
    }
}