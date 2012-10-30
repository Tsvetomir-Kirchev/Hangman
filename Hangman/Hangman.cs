using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Hangman
{
    class Hangman
    {
        static string[] words = 
        {
            "computer", "programmmer", "software",
            "developer", "debugger", "compiler",
            "algorithm", "array", "method", "variable"
        };

        private static string word;
        private static int mistakes = 0;
        private static StringBuilder secretWord = new StringBuilder();
        private static bool hasCheated = false;
        private static Dictionary<string, int> scoreBoard = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            string line = "";
            StartGame();

            while(true)
            {
                line = Console.ReadLine();
                if (line.Length == 1)
                {
                    CheckForExistsLetter(Convert.ToChar(line));
                }
                else if (line == "exit")
                {
                    Console.WriteLine("Good bye!");
                    return;
                }
                else
                {
                    switch (line)
                    {
                        case "help":
                            Help();
                            break;
                        case "restart":
                            Restart();
                            break;
                        case "top":
                            PrintScoreBoard(true);
                            break;
                        default:
                            Console.WriteLine("Incorrect guess or command!");
                            PrintSecretWord();
                            break;
                    }
                }
            }
        }

        static void StartGame()
        {
            Console.WriteLine("Welcome to \"Hangman\" game. Please try to guess my secret word.");
            Console.WriteLine("Use \'top\' to view the top scoreboard, \'restart\' to start new game,");
            Console.WriteLine("\'help\' to cheat and \'exit\' to quit the game.");

            hasCheated = false;
            mistakes = 0;
            Random rand = new Random();
            word = words[rand.Next(0, words.Length)];
            for (int i = 0; i < word.Length; i++)
            {
                secretWord.Append('_');
            }
            Console.Write("The secret word is: ");
            PrintSecretWord();
        }

        static void PrintSecretWord()
        {
            for (int i = 0; i < secretWord.Length; i++)
            {
                Console.Write(secretWord[i] + " ");
            }
            Console.WriteLine();
            Console.Write("Enter your guess: ");
        }

        static void Restart()
        {
            secretWord.Clear();
            Console.WriteLine();
            StartGame();
        }

        private static void CheckForExistsLetter(char letter)
        {
            bool hasLetter = false;
            int existsLetterCounter = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (letter == word[i])
                {
                    secretWord[i] = letter;
                    existsLetterCounter++;
                    hasLetter = true;
                    if (secretWord.ToString().Equals(word))
                    {
                        PrintWinMessage();
                        return;
                    }
                }
            }
            if (hasLetter)
            {
                Console.WriteLine("Good job! You revealed {0} letters.", existsLetterCounter);
            }
            else
            {
                Console.WriteLine("Sorry! There are no unrevealed letters \"{0}\".", letter);
                mistakes++;
            }
            PrintSecretWord();
        }

        static void Help()
        {
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (secretWord[i] == '_')
                {
                    secretWord[i] = word[i];
                    PrintSecretWord();
                    hasCheated = true;
                    break;
                }
            }
        }

        static void PrintWinMessage()
        {
            if (hasCheated)
            {
                Console.WriteLine("You won with {0} mistakes but you have cheated. " +
                    "You are not allowed to enter into the scoreboard.", mistakes);
                Console.WriteLine("The secret word is: {0}", secretWord);
                Restart();
            }
            else
            {
                Console.WriteLine("You won with {0} mistakes", mistakes);
                Console.WriteLine("The secret word is: {0}", secretWord);
                Top();
                Restart();
            }
        }

        static void Top()
        {
            Console.Write("Please enter your name fot the top scoreboard: ");
            string name = Console.ReadLine();
            scoreBoard.Add(name, mistakes);

            PrintScoreBoard(false);
        }

        static void PrintScoreBoard(bool topCommand)
        {
            Console.WriteLine("Scoreboard:");

            var sortedDictionary = (from dic in scoreBoard orderby dic.Value ascending select dic);

            int counter = 0;
            foreach (var item in sortedDictionary)
            {
                counter++;
                Console.WriteLine(counter + ". " + item.Key +
                    " --> " + item.Value + " mistakes");
                if (counter == 5)
                {
                    break;
                }
            }

            if (topCommand)
            {
                Console.Write("Enter your guess or command: ");
            }
        }
    }
}
