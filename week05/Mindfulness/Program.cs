using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MindfulnessApp
{
    public static class Constants
    {
        public static readonly string[] SpinnerChars = { "|", "/", "-", "\\" };
        public const int PauseDuration = 3;
    }
    public abstract class Activity
    {
        private string _name;
        private string _description;
        protected int _duration;

        public Activity(string name, string description)
        {
            _name = name;
            _description = description;
            _duration = 0;
        }

        private void GetDuration()
        {
            while (true)
            {
                Console.Write($"How long, in seconds, would you like for your {_name} activity? ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int duration) && duration > 0)
                {
                    _duration = duration;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive number.");
                }
            }
        }

        protected void PauseWithSpinner(int seconds)
        {
            DateTime startTime = DateTime.Now;
            Console.Write("Preparing to continue... ");
            
            while ((DateTime.Now - startTime).TotalSeconds < seconds)
            {
                foreach (string character in Constants.SpinnerChars)
                {
                    Console.Write(character);
                    Thread.Sleep(100); 
                    Console.Write("\b"); 
                }
            }
            Console.WriteLine("Done.        ");
        }

        protected void PauseWithCountdown(int seconds, string message = "Pausing")
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write($"{message} for {i} seconds... \r");
                Thread.Sleep(1000);
            }
            Console.Write(new string(' ', 40) + "\r");
        }

        protected void ShowStartingMessage()
        {
            Console.WriteLine($"\n--- {_name} Activity ---");
            Console.WriteLine($"Description: {_description}");

            GetDuration();

            Console.WriteLine("\nGet ready to begin...");
            PauseWithCountdown(Constants.PauseDuration, "Starting in");
        }

        protected void ShowEndingMessage()
        {
            Console.WriteLine("\nGreat job! You did well.");
            PauseWithSpinner(Constants.PauseDuration);
            Console.WriteLine($"You completed the {_name} activity for {_duration} seconds.");
            PauseWithCountdown(Constants.PauseDuration, "Finishing in");
            Console.WriteLine("--------------------------------\n");
        }
        public void Run()
        {
            ShowStartingMessage();
            RunActivity();
            ShowEndingMessage();
        }
        protected abstract void RunActivity();
    }
    public class BreathingActivity : Activity
    {
        public BreathingActivity()
            : base("Breathing", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
        {
        }

        protected override void RunActivity()
        {
            DateTime startTime = DateTime.Now;
            int cycleTime = 4;
            int breathPause = cycleTime / 2;

            while ((DateTime.Now - startTime).TotalSeconds < _duration)
            {
                // Breathe In
                Console.WriteLine("\nBreathe in...");
                PauseWithCountdown(breathPause, "Hold");
                if ((DateTime.Now - startTime).TotalSeconds >= _duration)
                    break;
                Console.WriteLine("Breathe out...");
                PauseWithCountdown(breathPause, "Hold");
            }

            Console.WriteLine("\nBreathing session complete.");
        }
    }
    public class ReflectionActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless.",
            "Think of a time when you accomplished a major goal."
        };

        private List<string> _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        private Random _random = new Random();

        public ReflectionActivity()
            : base("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
        {
        }

        protected override void RunActivity()
        {
            DateTime startTime = DateTime.Now;
            string initialPrompt = _prompts[_random.Next(_prompts.Count)];
            Console.WriteLine($"\nPrompt: {initialPrompt}");
            PauseWithCountdown(Constants.PauseDuration, "Take a moment to think");
            while ((DateTime.Now - startTime).TotalSeconds < _duration)
            {
                string question = _questions[_random.Next(_questions.Count)];
                Console.WriteLine($"\n> {question}");
                PauseWithSpinner(Constants.PauseDuration);
            }

            Console.WriteLine("\nReflection session complete.");
        }
    }
    public class ListingActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "Who are some of your personal heroes?",
            "What things brought you joy today?"
        };

        private Random _random = new Random();

        public ListingActivity()
            : base("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
        {
        }

        protected override void RunActivity()
        {
            string initialPrompt = _prompts[_random.Next(_prompts.Count)];
            Console.WriteLine($"\nPrompt: {initialPrompt}");
            PauseWithCountdown(Constants.PauseDuration, "Think about this for");

            Console.WriteLine("Go! Start listing your items now. Type your item and press Enter.");
            Console.WriteLine($"You have {_duration} seconds.");

            DateTime startTime = DateTime.Now;
            int item_count = 0;
            while ((DateTime.Now - startTime).TotalSeconds < _duration)
            {
                double remainingTime = _duration - (DateTime.Now - startTime).TotalSeconds;
                if (remainingTime <= 0) break;
                int remainingSeconds = (int)Math.Floor(remainingTime);

                Console.Write($"[{remainingSeconds}s remaining] Item #{item_count + 1}: ");
                string item = Console.ReadLine(); 

                if (!string.IsNullOrWhiteSpace(item))
                {
                    item_count++;
                }
                if ((DateTime.Now - startTime).TotalSeconds >= _duration)
                    break;
            }

            Console.WriteLine($"\nTime's up! You listed {item_count} items.");
        }
    }

    class Program
    {
        static void DisplayMenu()
        {
            Console.WriteLine("Welcome to the Mindfulness App!");
            Console.WriteLine("Please choose an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
        }

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) => {
                Console.WriteLine("\nThank you for using the Mindfulness App. Goodbye! 👋");
                e.Cancel = false;
            };

            while (true)
            {
                DisplayMenu();
                Console.Write("Enter your choice (1-4): ");
                string choice = Console.ReadLine();

                Activity activity = null;

                switch (choice)
                {
                    case "1":
                        activity = new BreathingActivity();
                        break;
                    case "2":
                        activity = new ReflectionActivity();
                        break;
                    case "3":
                        activity = new ListingActivity();
                        break;
                    case "4":
                        Console.WriteLine("Thank you for using the Mindfulness App. Goodbye! 👋");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                        continue;
                }

                activity.Run();
            }
        }
    }
}