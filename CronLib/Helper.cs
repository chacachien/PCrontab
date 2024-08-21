namespace CronLib;

public class Helper
{
    delegate Error ErrorChecker<T>(T value);
    public static DateTime IncreaseComponent(int position, int interval, DateTime nextTime)
    {
        return position switch
        {
            0 => nextTime.AddSeconds(interval),
            1 => nextTime.AddMinutes(interval),
            2 => nextTime.AddHours(interval),
            3 => nextTime.AddDays(interval),
            4 => nextTime.AddMonths(interval),
            // 5 => nextTime.AddDays(interval)
        };
    }
    public static int GetMaxRangeForPosition(DateTime current, int position)
    {
        // Returns the maximum range for each time unit (e.g., 59 for seconds, 59 for minutes, etc.)
        // Updated to consider the number of days in the specific month
        TimeDefine.Day.UpdateDayRange(current);

        return position switch
        {
            0 => TimeDefine.Second.Range[1],  // Seconds
            1 => TimeDefine.Minute.Range[1],  // Minutes
            2 => TimeDefine.Hour.Range[1],  // Hours
            3 => TimeDefine.Day.Range[1],   //DateTime.DaysInMonth(current.Year, current.Month),  // Days in current month
            4 => TimeDefine.Month.Range[1],  // Months
                       _ => 7,   // Day of Week (1-7)
        };
    }
    public static int GetComponent(DateTime current, int position)
    {
        // Extracts the corresponding component (e.g., seconds, minutes, etc.) from the DateTime
        return position switch
        {
            0 => current.Second,
            1 => current.Minute,
            2 => current.Hour,
            3 => current.Day,
            4 => current.Month,
            _ => (int)current.DayOfWeek,
        };
    }
}

public class Error
{
    
}

