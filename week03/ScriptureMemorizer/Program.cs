using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptureMemorizationApp
{
    public class Word
    {
        private string _text;
        private bool _isHidden;

        public Word(string text)
        {
            _text = text;
            _isHidden = false;
        }

        public void Hide()
        {
            _isHidden = true;
        }

        public bool IsHidden()
        {
            return _isHidden;
        }
        public string GetDisplayText()
        {
            if (_isHidden)
            {
                return new string('_', _text.Length);
            }
            else
            {
                return _text;
            }
        }
    }
    public class ScriptureReference
    {
        private string _book;
        private int _chapter;
        private int _startVerse;
        private int? _endVerse;
        public ScriptureReference(string book, int chapter, int verse)
        {
            _book = book;
            _chapter = chapter;
            _startVerse = verse;
            _endVerse = null;
        }
        public ScriptureReference(string book, int chapter, int startVerse, int endVerse)
        {
            _book = book;
            _chapter = chapter;
            _startVerse = startVerse;
            _endVerse = endVerse;
        }

        public override string ToString()
        {
            if (_endVerse.HasValue)
            {
                return $"{_book} {_chapter}:{_startVerse}-{_endVerse.Value}";
            }
            else
            {
                return $"{_book} {_chapter}:{_startVerse}";
            }
        }
    }
    public class Scripture
    {
        private ScriptureReference _reference;
        private List<Word> _words;
        private static readonly Random _random = new Random();

        public Scripture(ScriptureReference reference, string text)
        {
            _reference = reference;
            string[] rawWords = text.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            _words = new List<Word>();
            foreach (string rawWord in rawWords)
            {
                char lastChar = rawWord.LastOrDefault();
                if (!char.IsLetterOrDigit(lastChar) && rawWord.Length > 1)
                {
                    _words.Add(new Word(rawWord.Substring(0, rawWord.Length - 1)));
                    _words.Add(new Word(lastChar.ToString()));
                }
                else
                {
                    _words.Add(new Word(rawWord));
                }
            }
            _words = _words.Where(w => !string.IsNullOrWhiteSpace(w.GetDisplayText().Replace("_", ""))).ToList();
        }
        public void HideRandomWords(int count = 3)
        {
            List<Word> availableWords = _words.Where(w => !w.IsHidden()).ToList();

            int wordsToHide = Math.Min(count, availableWords.Count);

            for (int i = 0; i < wordsToHide; i++)
            {
                if (availableWords.Count == 0)
                    break;
                int indexToHide = _random.Next(availableWords.Count);
                
                availableWords[indexToHide].Hide();

                availableWords.RemoveAt(indexToHide);
            }
        }

        public string GetDisplayText()
        {
            return $"{_reference}\n\n" + 
                   string.Join(" ", _words.Select(w => w.GetDisplayText()));
        }
        public bool IsCompletelyHidden()
        {
            return _words.All(w => w.IsHidden());
        }
    }
    public class Program
    {

        private static List<Scripture> _scriptureLibrary = new List<Scripture>
        {
            new Scripture(
                new ScriptureReference("John", 3, 16),
                "For God so loved the world, that he gave his only begotten Son, that whosoever believeth in him should not perish, but have everlasting life."
            ),
            new Scripture(
                new ScriptureReference("Proverbs", 3, 5, 6),
                "Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths."
            ),
            new Scripture(
                new ScriptureReference("D&C", 8, 2),
                "Yea, thus saith the still small voice, which whispereth through and pierceth all things and often doth the Lord correct his people."
            )
        };

        private static Scripture GetRandomScripture()
        {
            Random rand = new Random();
            int index = rand.Next(_scriptureLibrary.Count);
            return _scriptureLibrary[index];
        }

        static void Main(string[] args)
        {
            Scripture currentScripture = GetRandomScripture();

            string userInput = "";

            while (userInput.ToLower() != "quit" && !currentScripture.IsCompletelyHidden())
            {
                Console.Clear();
                Console.WriteLine(currentScripture.GetDisplayText());
                Console.WriteLine("\n\nPress ENTER to hide more words or type 'quit' to end.");

                userInput = Console.ReadLine();

                if (userInput.ToLower() == "quit")
                {
                    break;
                }

                if (userInput == "")
                {
                    currentScripture.HideRandomWords(3);
                }
            }
            
            Console.Clear();
            Console.WriteLine(currentScripture.GetDisplayText());
            Console.WriteLine("\n\n-- All words hidden. Practice complete! --");
        }
    }
}