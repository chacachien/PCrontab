using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CronLib
{
/*    public class SecondExpressionHandler : IExpressionHandler
    {


        /// <summary>
        /// Expression: ["*"], [",", "1", "10"]
        /// </summary>
        /// <param name="expression"></param>
        public void Handle(List<string> expression)
        {
            TypeSchedule? ts = new();
            //every
            if (expression[0] == "/") {

                ts.NameTypeSchedule = ETypeSchedule.Every;
                ts.Timer = new List<int>();
            }
            //at
            else if (expression[0] == ","){
                ts.NameTypeSchedule = ETypeSchedule.At;
                ts.Timer = new List<int>();
            }
            else if (expression[0] == "*")
            {
                ts.NameTypeSchedule = ETypeSchedule.Every;
                
            }

            for(int i = 1; i < expression.Count; i++)
            {
                int value = int.Parse(expression[i]);
                CheckRange(kind, value);
                ts.Timer.Add(int.Parse(expression[i]));
            }
        }

        private void CheckRange(TypeField kind, int value)
        {
            if (ExpressionHandleFactory.Ranges.TryGetValue(kind, out var range))
            {

            }
        }
    }
    public class MinuteExpressionHandler : IExpressionHandler
    {
        public void Handle(List<string> expression)
        {
            throw new NotImplementedException();
        }
    }
    public class HourExpressionHandler : IExpressionHandler
    {
        public void Handle(List<string> expression)
        {
            throw new NotImplementedException();
        }
    }
    public class DayExpressionHandler : IExpressionHandler
    {
        public void Handle(List<string> expression)
        {
            throw new NotImplementedException();
        }
    }
    public class MonthExpressionHandler : IExpressionHandler
    {
        public void Handle(List<string> expression)
        {
            throw new NotImplementedException();
        }
    }
    public class DOWExpressionHandler : IExpressionHandler
    {
        public void Handle(List<string> expression)
        {
            throw new NotImplementedException();
        }
    }*/


    public class ExpressionHandle
    {
        public TypeField typeField { get; set; }

        private static readonly Dictionary<TypeField, (int Min, int Max)> Ranges = new()
        {
            { TypeField.Second, (TimeDefine.Second.Range[0], TimeDefine.Second.Range[1]) },
            { TypeField.Minute, (TimeDefine.Minute.Range[0], TimeDefine.Minute.Range[1]) },
            { TypeField.Hour, (TimeDefine.Hour.Range[0], TimeDefine.Hour.Range[1]) },
            { TypeField.Day, (TimeDefine.Day.Range[0], TimeDefine.Day.Range[1]) },
            { TypeField.Month, (TimeDefine.Month.Range[0], TimeDefine.Month.Range[1]) },
            { TypeField.DayOfWeek, (TimeDefine.DayOfWeek.Range[0], TimeDefine.DayOfWeek.Range[1]) }
        };


        public  List<string> Preprocessing(string expression)
        {

                // handle expression to array
                // loop in array and decide what kind of schedule

                List<string> elements = new();
                char? separator = null;
                foreach (var c in expression)
                {
                    if ( c == '/' || c == ',')
                    {
                        if (separator != null && separator != c)
                        {
                            throw new InvalidOperationException("More than one separator found in the expression.");
                        }
                        separator = c; // Assign the found separator
                    }
                }
                if (separator != null)
                {
                    elements.Add(separator.ToString());
                    elements.AddRange(expression.Split(separator.Value).Where(e => !string.IsNullOrEmpty(e)));
                }
                else
                {
                    elements.Add(expression);
                }
            return elements;
        }
        public TypeSchedule Handle(List<string> expression)
        {
            TypeSchedule? ts = new();
            //every
            if (expression[0] == "/")
            {
                ts.NameTypeSchedule = ETypeSchedule.Every;
                ts.Timer = new List<int>();
            }

            //at
            else if (expression[0] == ",")
            {
                ts.NameTypeSchedule = ETypeSchedule.At;
                ts.Timer = new List<int>();
            }
            else if (expression[0] == "*")
            {
                ts.NameTypeSchedule = ETypeSchedule.Every;
                ts.Timer = new List<int>() { 1 };
            }
            else
            {
                ts.NameTypeSchedule = ETypeSchedule.At;
                ts.Timer = new List<int>();
            }

            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i] == "*" || expression[i] == "/" || expression[i] == ",")
                {
                    continue;
                }
                int value = int.Parse(expression[i]);
                CheckRange(typeField, value);

                ts.Timer.Add(value);
            }
            return ts;
        }

        private void CheckRange(TypeField kind, int value)
        {
            if (Ranges.TryGetValue(kind, out var range))
            {
                if(value <range.Min || value > range.Max)
                {
                    throw new Exception("Out of range");
                }
            }
            else
            {
                throw new Exception("Unsupported TypeField");
            }
        }
    }
}
