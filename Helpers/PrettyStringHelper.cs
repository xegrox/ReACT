namespace ReACT.Helpers;

public static class PrettyString
{
    public static string OrdinalString(this int num)
    {
        if( num <= 0 ) return num.ToString();
        
        switch(num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        return (num % 10) switch
        {
            1 => num + "st",
            2 => num + "nd",
            3 => num + "rd",
            _ => num + "th"
        };
    }

    public static string RelativeString(this DateTime d)
    {
        var span = DateTime.Now.Subtract(d);
        var dayDiff = (int)span.TotalDays;
        var secDiff = (int)span.TotalSeconds;

        string Plural(string s, int i) => i > 1 ? s + "s" : s; 
        switch (dayDiff)
        {
            case 0:
                switch (secDiff)
                {
                    case < 60:
                        return "just now";
                    case < 3600:
                        var minuteDiff = secDiff / 60;
                        return $"{minuteDiff} {Plural("minute", minuteDiff)} ago";
                    default:
                        var hourDiff = secDiff / 3600;
                        return $"{hourDiff} {Plural("hour", hourDiff)} ago";
                }
            case 1:
                return "yesterday";
            case < 7:
                return $"{dayDiff} days ago";
            case < 31:
                var weekDiff = dayDiff / 7; 
                return $"{weekDiff} {Plural("week", weekDiff)} ago";
            case < 365:
                var monthDiff = dayDiff / 31;
                return $"{monthDiff} {Plural("month", monthDiff)} ago";
            default:
                var yearDiff = dayDiff / 365;
                return $"{yearDiff} {Plural("year", yearDiff)} ago";
        }
    }
}