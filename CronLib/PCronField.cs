namespace CronLib;

public class PCronField:IPCronField
{
    public TypeField Kind { get; set; }
    public int Value { get; set; }
    readonly int[] Range = new int[2];

    static public void TryParse<T>(TypeField kind, string expression, Func<PCronField, T> valueSelector)
    {
        valueSelector(new PCronField());
    }
    

    public int GetNext()
    {
        
    }

    PCronField(TypeField kind, string expression )
    {
        if (kind == TypeField.Second)
        {
            
        }
    }
}

