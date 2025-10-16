using System;
using System.Collections.Generic;

public class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthSeconds { get; set; }

    private List<Comment> _comments;

    public Video(string title, string author, int lengthSeconds)
    {
        Title = title;
        Author = author;
        LengthSeconds = lengthSeconds;
        _comments = new List<Comment>(); // Initialize the list when the video is created
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    public List<Comment> GetComments()
    {
        return _comments;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- YouTube Video and Comment Tracker ---");
        Console.WriteLine();

        Video video1 = new Video("C# Basics: Understanding Classes", "CodingPr0", 980);
        Video video2 = new Video("Advanced AI: Neural Networks Explained", "AIT3acher", 1520);
        Video video3 = new Video("Gardening Hacks for Small Spaces", "DonutLord", 550);

        video1.AddComment(new Comment("CSharpCoder1000", "Great explanation of constructors!"));
        video1.AddComment(new Comment("KevinTheDev", "I was confused about properties, this helped a lot."));
        video1.AddComment(new Comment("Loremwashere", "Can you do a video on interfaces next?"));

        video2.AddComment(new Comment("TomHanks2001", "The visualization of backpropagation was excellent."));
        video2.AddComment(new Comment("CollegeDropOut8", "Solid theoretical foundation, thanks for sharing!"));
        video2.AddComment(new Comment("User54321", "This video is too long. Can you summarize?"));
        video2.AddComment(new Comment("ResearcherX", "Very well structured. Saving this for reference."));

        video3.AddComment(new Comment("Bob_Iger1", "The vertical planting idea is genius for my balcony!"));
        video3.AddComment(new Comment("Plants4life", "What kind of soil do you recommend for herbs?"));
        video3.AddComment(new Comment("DirtMan", "Short, sweet, and highly practical. Love it."));

        List<Video> videos = new List<Video> { video1, video2, video3 };

        foreach (Video video in videos)
        {
            Console.WriteLine("========================================");
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthSeconds} seconds");

            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()} 💬");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine("Comments:");
            if (video.GetNumberOfComments() == 0)
            {
                Console.WriteLine("    (No comments yet)");
            }
            else
            {
                foreach (Comment comment in video.GetComments())
                {
                    Console.WriteLine($"    - {comment.CommenterName}: {comment.Text}");
                }
            }
            Console.WriteLine();
        }
    }
}