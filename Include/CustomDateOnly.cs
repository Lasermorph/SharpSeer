namespace SharpSeer;

public class CustomDateOnly
{
    public int Year { get; set; } = 0;
    public int Month { get; set; } = 0;
    public int Day { get; set; } = 0;

    public CustomDateOnly()
    {
        DateTime dateTime =  DateTime.Now;
        Year = dateTime.Year;
        Month = dateTime.Month;
        Day = dateTime.Day;
    }

    public CustomDateOnly(CustomDateOnly customDateOnly)
    {
        Year = customDateOnly.Year;
        Month = customDateOnly.Month;
        Day = customDateOnly.Day;
    }

    public CustomDateOnly(DateOnly dateOnly)
    {
        Year = dateOnly.Year;
        Month = dateOnly.Month;
        Day = dateOnly.Day;
    }

    public CustomDateOnly(DateTime dateTime)
    {
        Year = dateTime.Year;
        Month = dateTime.Month;
        Day = dateTime.Day;
    }

    // If given object is bigger return 1, equal 0, or smaller -1 that this CustomDateOnly
    public int Comparer(in CustomDateOnly customDateOnly)
    {
        if (customDateOnly.Year == Year && customDateOnly.Month == Month && customDateOnly.Day == Day)
        {
            return 0;
        }
        if (customDateOnly.Year > Year && customDateOnly.Month > Month && customDateOnly.Day > Day)
        {
            return 1;
        }
        return -1;
    }

    // If given object is bigger return 1, equal 0, or smaller -1 that this CustomDateOnly
    public int Comparer(in DateTime dateTime)
    {
        if (dateTime.Year == Year && dateTime.Month == Month && dateTime.Day == Day)
        {
            return 0;
        }
        if (dateTime.Year > Year)
        {
            return 1;
        }
        else if (dateTime.Year == Year && dateTime.Month > Month)
        {
            return 1;
        }
        else if (dateTime.Year == Year && dateTime.Month == Month && dateTime.Day > Day)
        {
            return 1;
        }
        return -1;
    }

    public int Comparer(in DateOnly dateOnly)
    {
        if (dateOnly.Year == Year && dateOnly.Month == Month && dateOnly.Day == Day)
        {
            return 0;
        }
        if (dateOnly.Year > Year && dateOnly.Month > Month && dateOnly.Day > Day)
        {
            return 1;
        }
        return -1;
    }

}
