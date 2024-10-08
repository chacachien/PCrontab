﻿using CronLib;

namespace Program;

class Program
{
    /*    public enum TypeField
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
            string input = "0/2 * * * * *";
            string[] listExpression = input.Split(" ");
            if (listExpression.Length != 6)
                throw new Exception("Invalid expression!");
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine(listExpression[i]);
            }
            Console.WriteLine(listExpression.Length);
        }*/
    static void Main(string[] args)
    {
        var current = DateTime.Now;
/*        Console.WriteLine($"current: {current}");
        PCron cron =  PCron.Parse("* * * * * *");
        var nextOccurrence = cron.GetNextOccurence(current);
        Console.WriteLine($"Next occurrence is at: {nextOccurrence}");*/
        for(int i =0; i< 10; i++)
        {
            Console.WriteLine($"current: {current}");
            PCron cron = PCron.Parse("* 1,4 6 22 * *");
            current = cron.GetNextOccurence(current);
            Console.WriteLine($"Next occurrence is at: {current}");
        }
    }
    //static void Main(string[] args)
    //{
    //    //cancelParallel();
    //    //testTask();
    //    /*ProcessDataAsync();
    //    Console.WriteLine("Press any key to exit...");
    //    Console.ReadKey();*/
    //    /*            string expression = "1,2,3 ";
    //                var l = Preprocessing(expression);
    //                var t = Handle(l);
    //                Console.WriteLine(t);
    //                Console.ReadLine();*/
    //    var secondParam = new ScheduleParameter { Type = "at", Values = new List<int> { 30 } };
    //    var minuteParam = new ScheduleParameter { Type = "every", Values = new List<int> { 1 } };
    //    var hourParam = new ScheduleParameter { Type = "every", Values = new List<int> { 1 } };
    //    var dayParam = new ScheduleParameter { Type = "every", Values = new List<int> { 1 } };
    //    var monthParam = new ScheduleParameter { Type = "every", Values = new List<int> { 1 } };
    //    var dayOfWeekParam = new ScheduleParameter { Type = "every", Values = new List<int> { 1 } };

    //    DateTime now = DateTime.Now;
    //    List<ScheduleParameter> listScheduleParameter = new List<ScheduleParameter>
    //        {
    //            secondParam, minuteParam, hourParam, dayParam, monthParam, dayOfWeekParam
    //        };
    //    DateTime nextOccurrence = GetNextOccurrence(now, listScheduleParameter);

    //    Console.WriteLine($"Next occurrence is at: {nextOccurrence}");
    //    Console.ReadLine();

    //}

    static public Dictionary<int, bool> atValues = new Dictionary<int, bool>();
    static public Dictionary<int, Stack<int>> everyValues = new Dictionary<int, Stack<int>>();
    static public DateTime GetNextOccurrence(DateTime startTime, List<ScheduleParameter> listScheduleParameter)
    {
        DateTime nextTime = startTime;

        //nextTime = AdjustTimeComponent(nextTime, listScheduleParameter, 0, t => t.Second, (t, val) => t.AddSeconds(val));
        //nextTime = AdjustTimeComponent(nextTime, listScheduleParameter, 1, t => t.Minute, (t, val) => t.AddMinutes(val));
        //nextTime = AdjustTimeComponent(nextTime, listScheduleParameter, 2, t => t.Hour, (t, val) => t.AddHours(val));
        //nextTime = AdjustTimeComponent(nextTime, listScheduleParameter, 3, t => t.Day, (t, val) => t.AddDays(val));
        //nextTime = AdjustTimeComponent(nextTime, listScheduleParameter, 4, t => t.Month, (t, val) => t.AddMonths(val));
        //nextTime = AdjustDayOfWeek(nextTime, listScheduleParameter);
        for(int i =0; i< listScheduleParameter.Count; i++)
        {
            if (listScheduleParameter[i].Type == "every")
            {
                SetupArray(nextTime, i, listScheduleParameter[i]);
            }
        }
        foreach (var e in everyValues)
        {
            Console.WriteLine($"ID: {e.Key}");
            foreach (var v in e.Value)
            {
                Console.Write($"{v},");
            }
        }
        return nextTime;
    }

    private static void SetupArray(DateTime current, int i, ScheduleParameter scheduleParameter)
    {
        Stack<int> s = new Stack<int>();
        Stack<int> minStack = new Stack<int>();
        int interval = scheduleParameter.Values.First();
        int maxRange = GetRangeForPosition(current, i);

        int currentValue = GetComponent(current, i);

        for(int j = 0; j< maxRange; j++)
        {
            s.Push(j);
        }
        while (s.Count > 0)
        {
            int o;
            s.TryPop(out o);
            minStack.Push(o);
        }
        everyValues[i] = minStack;
    }

    private static int GetRangeForPosition(DateTime current, object position)
    {
        return position switch
        {
            0 => 59,
            1 => 59,
            2 => 59,
            3 => DateTime.DaysInMonth(current.Year, current.Month),
            4 => 12,
            5 => 7
        };
    }

    private static int GetComponent(DateTime current, object postion)
    {
        return postion switch
        {
            0 => current.Second,
            1 => current.Minute,
            2 => current.Hour,
            3 => current.Day,
            4 => current.Month,
            5 => (int)current.DayOfWeek
        };
    }

    static private DateTime AdjustTimeComponent(DateTime current, List<ScheduleParameter> listScheduleParameter, int position, Func<DateTime, int> getComponent, Func<DateTime, int, DateTime> addFunction)
    {
        int currentValue = getComponent(current);
        int firstElement = listScheduleParameter[position].Values.First(); // assuming single value for "every"
        var listOfValueCount = listScheduleParameter[position].Values.Count;
        var listOfValue = listScheduleParameter[position].Values;
        if (listScheduleParameter[position].Type == "every")
        {
            // call when all at is adapted
            if (checkAt())
            {
                return addFunction(current, firstElement);
            }
        }
        else
        {
            var result = firstElement;
            atValues.Add(position, false);

            for (int i = 0; i < listOfValueCount; i++)
            {
                {
                    if (listOfValue[i] >= currentValue)
                    {
                        result = listOfValue[i];
                        break;
                    }
                }
            }

            // wait to next value and set before (type not = "every") to 0
            if (result < currentValue)
            {
                for (int i = 0; i < position; i++)
                {
                    bool o;
                    if (atValues.TryGetValue(i, out o))
                    {
                        if (o)
                        {
                            if (i < position)
                            {
                                // set this current time of field to start value
                            }
                        }
                    }
                }
            }
            else if (result == currentValue)
            {
                atValues.Add(position, true);
            }
            else
            {
                // plus the value datetime after to 1, example if position is minute, set hour to next hour
            }


        }

        return current;
    }

    private static bool checkAt()
    {
        if (atValues.Count == 0) return false;
        foreach (var v in atValues.Values)
        {
            if (!v)
            {
                return false;
            }
        }
        return true;
    }

    static private DateTime AdjustDayOfWeek(DateTime current, ScheduleParameter param)
    {
        // Similar to other adjustments, but specific to the day of the week
        if (param.Type == "every")
        {
            int daysToAdd = param.Values.First() - (int)current.DayOfWeek;
            if (daysToAdd <= 0) daysToAdd += 7;
            return current.AddDays(daysToAdd);
        }

        return current;
    }
}
public enum ETypeSchedule
{
    Every = 0,
    At = 1
}
public class TypeSchedule
{
    public ETypeSchedule NameTypeSchedule { get; set; }
    public List<int> Timer { get; set; }
}


public class ScheduleParameter
{
    public string Type { get; set; } // "every" or "at"
    public List<int> Values { get; set; } // list of values
}

