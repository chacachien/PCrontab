using System.Runtime.InteropServices;

namespace CronLib;

public class PCron
{

    public List<PCronField> listScheduleParameter = new();
    
    /// <summary>
    /// Create 6 part of the PCron schedule: date of week, month, day, hour, minute, second
    /// </summary>
    public static PCron Parse(string input)
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
            PCronField field = PCronField.TryParse(kind, listExpression[i], p => p);
            listPCronField.Add(field); 
        }
        return valueSelector(new PCron(listPCronField));
    }
    

    PCron(List<PCronField> listPCronField)
    {
        foreach (var l in listPCronField)
            listScheduleParameter.Add(l);
    }
     private Dictionary<int, Stack<int>> everyValues = new Dictionary<int, Stack<int>>();
     public bool firstEvery = true;
     public int[] finalResult = new int[6];
    public bool isSignal = false;
    private void SetUpArray(DateTime current, int position, int interval)
    {
            Stack<int> s = new Stack<int>();
            Stack<int> minStack = new Stack<int>();
            int maxRange = Helper.GetMaxRangeForPosition(current, position);

            int currentValue = Helper.GetComponent(current, position);
        int startValue = currentValue;
        if (firstEvery)
        {
            startValue = currentValue + interval;
            firstEvery = false;
        }
        else
        {

            startValue = currentValue;
        }
        for (int j = startValue; j < maxRange + 1; j += interval)
            {
                s.Push(j);
            }
            while (s.Count > 0)
            {
                int o;
                s.TryPop(out o);
                minStack.Push(o);
            }
            everyValues[position] = minStack;
/*            foreach(var v in everyValues)
        {
            Console.WriteLine($"{v.Key}:");
            
            foreach(var o in v.Value)
            {
                Console.WriteLine($"{o},");
            }
            Console.WriteLine("");
        }*/
        }
    
    private void SignalNextValue(int i, DateTime current)
    {
        int j = i;

        while (!isSignal && j < 6)
        {
            if (listScheduleParameter[i].Scheduler.NameTypeSchedule == ETypeSchedule.At)
            {
                var v = Helper.GetComponent(current, i);
                if (v < finalResult[i])
                {
                    return;
                }
            }
            if (listScheduleParameter[i].Scheduler.NameTypeSchedule == ETypeSchedule.Every)
            {
                int o;
                everyValues[i].TryPop(out o);
                isSignal = true;
                return;
            } 
            j++;
        }
    }
    public DateTime GetNextOccurence(DateTime current)
    {
        DateTime nextTime = current;
        int l = listScheduleParameter.Count;

        // Set up arrays for each time component based on their "every" intervals
        
        for (int i = 0; i < l; i++)
        {
            if (listScheduleParameter[i].Scheduler.NameTypeSchedule == ETypeSchedule.Every)
            {
                SetUpArray(nextTime, i, listScheduleParameter[i].Scheduler.Timer.First());
            }
        }

        // Further logic to determine the next occurrence based on these arrays would go here


        // get the first match value in every and at array

        for (int i = 0; i < l; i++)
        {
            // finding the smallest value greater or equal than current value 
            var currentValue = Helper.GetComponent(nextTime, i);
            if (listScheduleParameter[i].Scheduler.NameTypeSchedule == ETypeSchedule.Every)
            {
                // handle the stack 
                int o;
                if (everyValues[i].TryPop(out o))
                {
                    finalResult[i] = o;
                }
                else
                {
                    var interval = listScheduleParameter[i].Scheduler.Timer.First();
                    nextTime = Helper.IncreaseComponent(i, interval, nextTime);
                    SetUpArray(nextTime, i, listScheduleParameter[i].Scheduler.Timer.First());
                    if (everyValues[i].TryPop(out o))
                    {
                        finalResult[i] = o;
                    }
                    SignalNextValue(i + 1, current);
                }
            }
            else
            {
                // handler the list
                var result = listScheduleParameter[i].Scheduler.Timer.First();

                for (int j = 0; j < listScheduleParameter[i].Scheduler.Timer.Count; j++)
                {

                    if (listScheduleParameter[i].Scheduler.Timer[j] >= currentValue)
                    {
                        result = listScheduleParameter[i].Scheduler.Timer[j];
                        break;
                    }
                }
                finalResult[i] = result;
                if(currentValue > result )
                {
                    
                    if(Helper.GetComponent(current, i+1) >= finalResult[i+1])
                        SignalNextValue(i + 1, current);
                }
            }
        }
        return new DateTime(current.Year, finalResult[4], finalResult[3], finalResult[2], finalResult[1], finalResult[0]);
    }
}