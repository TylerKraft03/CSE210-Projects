using System;
using System.Globalization;

public abstract class Activity
{
    private DateTime _date;
    private int _lengthInMinutes;

    public Activity(DateTime date, int lengthInMinutes)
    {
        _date = date;
        _lengthInMinutes = lengthInMinutes;
    }

    public abstract double GetDistance();

    public abstract double GetSpeed();

    public abstract double GetPace();

    public virtual string GetSummary()
    {
        string dateFormatted = _date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
        string activityType = GetType().Name;
        double distance = GetDistance();
        double speed = GetSpeed();
        double pace = GetPace();

        return $"{dateFormatted} {activityType} ({_lengthInMinutes} min): Distance {distance:F2} miles, Speed: {speed:F2} mph, Pace: {pace:F2} min per mile";
    }
}

public class Running : Activity
{
    private double _distanceMiles;

    public Running(DateTime date, int lengthInMinutes, double distanceMiles)
        : base(date, lengthInMinutes)
    {
        _distanceMiles = distanceMiles;
    }

    public override double GetDistance()
    {
        return _distanceMiles;
    }

    public override double GetSpeed()
    {
        double pace = GetPace();
        if (pace == 0) return 0;
        return 60 / pace;
    }

    public override double GetPace()
    {
        double distance = GetDistance();
        double minutes = 30.0; 
        if (distance == 0) return 0;
        return minutes / distance;
    }
}

public class Cycling : Activity
{
    private double _speedMph;

    public Cycling(DateTime date, int lengthInMinutes, double speedMph)
        : base(date, lengthInMinutes)
    {
        _speedMph = speedMph;
    }

    public override double GetDistance()
    {
        double minutes = 30.0;
        return (_speedMph / 60) * minutes;
    }

    public override double GetSpeed()
    {
        return _speedMph;
    }

    public override double GetPace()
    {
        if (_speedMph == 0) return 0;
        return 60 / _speedMph;
    }
}

public class Swimming : Activity
{
    private int _numberOfLaps;
    private const double LapLengthMeters = 50.0;
    private const double KmToMilesFactor = 0.621371;

    public Swimming(DateTime date, int lengthInMinutes, int numberOfLaps)
        : base(date, lengthInMinutes)
    {
        _numberOfLaps = numberOfLaps;
    }

    public override double GetDistance()
    {
        double distanceKm = (_numberOfLaps * LapLengthMeters) / 1000.0;
        return distanceKm * KmToMilesFactor;
    }

    public override double GetSpeed()
    {
        double pace = GetPace();
        if (pace == 0) return 0;
        return 60 / pace;
    }

    public override double GetPace()
    {
        double distance = GetDistance();
        if (distance == 0) return 0;
        double minutes = 30.0;
        return minutes / distance;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        List<Activity> activities = new List<Activity>();

        Running running = new Running(new DateTime(2025, 10, 16), 30, 3.0);
        activities.Add(running);

        Cycling cycling = new Cycling(new DateTime(2025, 10, 16), 30, 15.0);
        activities.Add(cycling);

        Swimming swimming = new Swimming(new DateTime(2025, 10, 16), 30, 50);
        activities.Add(swimming);

        Console.WriteLine("Fitness Activity Summary (Using Miles)");
        Console.WriteLine("Note: All activities are assumed to be 30 minutes for consistent speed/pace calculation.");
        Console.WriteLine("-----------------------------------------------------");

        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}