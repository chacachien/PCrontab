namespace CronLib;

public interface IPCronField
{
    public TypeField Kind { get; set; }
    public int Value { get; set; }

    public void TryParse();
    public int GetNext();

}


