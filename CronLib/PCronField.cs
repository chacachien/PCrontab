namespace CronLib;

public class PCronField : IPCronField
{
    public TypeField Kind { get; set; }
    public int Value { get; set; }
    public TypeSchedule Scheduler { get; set; }

    //readonly int[] Range = new int[2];

    static public T TryParse<T>(TypeField kind, string expression, Func<PCronField, T> valueSelector)
    {
        return valueSelector(new PCronField(kind, expression));
    }
    

    public int GetNext()
    {
        return 0;
    }


    /// <summary>
    /// Expression: "*" => ["*"}, "/3" => ["/", "3"], "1,2,33" => [",", "1","2","33"]
    /// Process the single expression (every or at) 
    /// </summary>
    private void HandleExpression(TypeField kind, string expression)
    {
        // handle expression to array
        // loop in array and decide what kind of schedule
        List<string> elements = new();
        ExpressionHandle expressionHandle = new()
        {
            typeField = kind
        };
        elements = expressionHandle.Preprocessing(expression);
        Scheduler = expressionHandle.Handle(elements);
    }


     public PCronField(TypeField kind, string expression )
    {
        Kind = kind;
        HandleExpression(kind, expression);

    }
}

