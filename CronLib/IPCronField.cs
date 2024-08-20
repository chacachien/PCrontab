namespace CronLib;

public interface IPCronField
{
    public TypeField Kind { get; set; }
    public int Value { get; set; }
    public TypeSchedule Scheduler { get; set; } 
    //public T TryParse<T>(TypeField kind, string expression, Func<PCronField, T> valueSelector);
    public int GetNext();


}


