namespace CronLib;

public class TimeUnit
{
    public string Name { get; set; }
    public int[] Range { get; set; }
    public string[] Element { get; set; }
    public DateTime Current;


    internal void UpdateDayRange(DateTime current)
    {
        Range = new[] { 1, DateTime.DaysInMonth(current.Year, current.Month) };
    }
}

public class TimeDefine
{
    public static TimeUnit Second = new()
    {
        Name = "Second",
        Range = [0, 59]
    };

    public static TimeUnit Minute = new()
    {
        Name = "Minute",
        Range = [0, 59]
    };

    public static TimeUnit Hour = new()
    {
        Name = "Hour",
        Range = [0, 23]
    };

    public static TimeUnit Day = new()
    {
        Name = "Day",
        Range = [1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)]
    };

    public static TimeUnit Month = new()
    {
        Name = "Month",
        Range = [1, 12],
        Element = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    };

    public static TimeUnit DayOfWeek = new()
    {
        Name = "Dow",
        Range = [0, 6],
        Element = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
    };

}