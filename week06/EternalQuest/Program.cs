using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SimpleGoal), nameof(SimpleGoal))]
[JsonDerivedType(typeof(EternalGoal), nameof(EternalGoal))]
[JsonDerivedType(typeof(ChecklistGoal), nameof(ChecklistGoal))]
public abstract class Goal
{
    private string _name;
    private string _description;
    private int _basePoints;
    
    protected Goal() { }

    public Goal(string name, string description, int basePoints)
    {
        _name = name;
        _description = description;
        _basePoints = basePoints;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public int BasePoints
    {
        get => _basePoints;
        set => _basePoints = value;
    }

    public abstract int RecordEvent();

    public virtual string GetStatusDisplay()
    {
        return $"[?] {Name} ({Description})";
    }

    public virtual bool IsComplete()
    {
        return false;
    }
}

public class SimpleGoal : Goal
{
    private bool _isComplete = false;

    protected SimpleGoal() { }

    public SimpleGoal(string name, string description, int basePoints)
        : base(name, description, basePoints) { }

    public bool IsCompleted
    {
        get => _isComplete;
        set => _isComplete = value;
    }

    public override int RecordEvent()
    {
        if (_isComplete)
        {
            Console.WriteLine($"Goal '{Name}' is already complete.");
            return 0;
        }

        _isComplete = true;
        Console.WriteLine($"Goal '{Name}' completed! You earned {BasePoints} points.");
        return BasePoints;
    }

    public override string GetStatusDisplay()
    {
        string status = _isComplete ? "[X]" : "[ ]";
        return $"{status} {Name} (Simple)";
    }

    public override bool IsComplete()
    {
        return _isComplete;
    }
}

public class EternalGoal : Goal
{
    private int _timesRecorded = 0;

    protected EternalGoal() { }

    public EternalGoal(string name, string description, int basePoints)
        : base(name, description, basePoints) { }

    public int TimesRecorded
    {
        get => _timesRecorded;
        set => _timesRecorded = value;
    }

    public override int RecordEvent()
    {
        _timesRecorded++;
        Console.WriteLine($"Goal '{Name}' recorded. You earned {BasePoints} points.");
        return BasePoints;
    }

    public override string GetStatusDisplay()
    {
        return $"[ ] {Name} (Eternal Goal - Recorded: {_timesRecorded} times)";
    }
}

// ---

public class ChecklistGoal : Goal
{
    private int _requiredCount;
    private int _bonusPoints;
    private int _currentCount = 0;

    protected ChecklistGoal() { }

    public ChecklistGoal(string name, string description, int basePoints, int requiredCount, int bonusPoints)
        : base(name, description, basePoints)
    {
        _requiredCount = requiredCount;
        _bonusPoints = bonusPoints;
    }

    public int RequiredCount
    {
        get => _requiredCount;
        set => _requiredCount = value;
    }
    
    public int BonusPoints
    {
        get => _bonusPoints;
        set => _bonusPoints = value;
    }
    
    public int CurrentCount
    {
        get => _currentCount;
        set => _currentCount = value;
    }

    public override int RecordEvent()
    {
        if (IsComplete())
        {
            Console.WriteLine($"Goal '{Name}' is already complete. No more points awarded.");
            return 0;
        }

        _currentCount++;
        int points = BasePoints;

        if (IsComplete())
        {
            points += _bonusPoints;
            Console.WriteLine($"Goal '{Name}' completed! You earned the bonus of {_bonusPoints} points!");
        }

        Console.WriteLine($"Goal '{Name}' recorded. You earned {points} points.");
        return points;
    }

    public override string GetStatusDisplay()
    {
        string status = IsComplete() ? "[X]" : "[ ]";
        return $"{status} {Name} (Completed {_currentCount}/{_requiredCount} times)";
    }

    public override bool IsComplete()
    {
        return _currentCount >= _requiredCount;
    }
}


public class GoalManager
{
    private List<Goal> _goals = new List<Goal>();
    private int _score = 0;
    private string _filePath = "eternalquest_goals.json";

    public int Score => _score;
    
    public void Start()
    {
        LoadGoals();
        MainMenu();
    }


    public void MainMenu()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nEternal Quest");
            Console.WriteLine($"Your Current Score: {_score}");
            Console.WriteLine("---------------------");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Record Event");
            Console.WriteLine("4. Save Goals");
            Console.WriteLine("5. Load Goals");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    ListGoals();
                    break;
                case "3":
                    RecordEvent();
                    break;
                case "4":
                    SaveGoals();
                    break;
                case "5":
                    LoadGoals();
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public void CreateGoal()
    {
        Console.WriteLine("Select Goal Type:");
        Console.WriteLine("1. Simple Goal (e.g., Run a Marathon)");
        Console.WriteLine("2. Eternal Goal (e.g., Read Scriptures Daily)");
        Console.WriteLine("3. Checklist Goal (e.g., Attend the Temple 10 Times)");
        Console.Write("Enter your choice (1-3): ");
        string typeChoice = Console.ReadLine();

        Console.Write("Goal Name: ");
        string name = Console.ReadLine();
        Console.Write("Goal Description: ");
        string description = Console.ReadLine();
        Console.Write("Base Points Awarded: ");
        int basePoints = int.Parse(Console.ReadLine());

        switch (typeChoice)
        {
            case "1":
                _goals.Add(new SimpleGoal(name, description, basePoints));
                break;
            case "2":
                _goals.Add(new EternalGoal(name, description, basePoints));
                break;
            case "3":
                Console.Write("Required Count: ");
                int requiredCount = int.Parse(Console.ReadLine());
                Console.Write("Bonus Points: ");
                int bonusPoints = int.Parse(Console.ReadLine());
                _goals.Add(new ChecklistGoal(name, description, basePoints, requiredCount, bonusPoints));
                break;
            default:
                Console.WriteLine("Invalid goal type selected. Goal creation failed.");
                return;
        }
        Console.WriteLine($"\nNew Goal '{name}' created!");
    }

    public void ListGoals()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals have been created yet.");
            return;
        }

        Console.WriteLine("\nYour Goals");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetStatusDisplay()}");
        }
    }

    public void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available to record an event.");
            return;
        }

        ListGoals();
        Console.Write("\nEnter the number of the goal you accomplished: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= _goals.Count)
        {
            Goal goal = _goals[index - 1];
            int pointsEarned = goal.RecordEvent();
            _score += pointsEarned;
            Console.WriteLine($"\nYou have earned {pointsEarned} points.");
            Console.WriteLine($"Your new score is: {_score}");
        }
        else
        {
            Console.WriteLine("Invalid goal number.");
        }
    }


    public void SaveGoals()
    {
        try
        {
            var data = new
            {
                Score = _score,
                Goals = _goals
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            
            // Serialize the Goal list (polymorphic serialization handles derived types)
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, jsonString);
            Console.WriteLine($"\nGoal progress and score successfully saved to {_filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving: {ex.Message}");
        }
    }

    public void LoadGoals()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine("\nNo saved file found. Starting with a new score and no goals.");
            _goals = new List<Goal>();
            _score = 0;
            return;
        }

        try
        {
            string jsonString = File.ReadAllText(_filePath);
            
            var data = JsonSerializer.Deserialize<JsonElement>(jsonString);
            
            _score = data.GetProperty("Score").GetInt32();
            
            var goalsJson = data.GetProperty("Goals").GetRawText();
            _goals = JsonSerializer.Deserialize<List<Goal>>(goalsJson);

            Console.WriteLine($"\nGoal progress and score successfully loaded from {_filePath}");
            Console.WriteLine($"Loaded Score: {_score}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nAn error occurred while loading: {ex.Message}");
            Console.WriteLine("Starting with a new score and no goals to prevent data corruption.");
            _goals = new List<Goal>();
            _score = 0;
        }
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        GoalManager manager = new GoalManager();
        manager.Start();
    }
}