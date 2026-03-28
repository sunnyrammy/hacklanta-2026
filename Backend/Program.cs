using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LieDetectorGame
{
    class Program
    {
        static async Task Main()
        {
            // --- Setup ---
            string userLie = "I was at home all night"; // example lie for testing
            int suspicion = 0;
            int questionNumber = 1;

            List<(string role, string content)> history = new List<(string, string)>();

            Console.WriteLine("Starting AI lie detector test...\n");

            // --- Main Game Loop ---
            while (suspicion < 100)
            {
                var question = await AI.AskMultipleChoice(history, userLie, suspicion, questionNumber);

                // Display question
                Console.WriteLine($"\nAI Question {questionNumber}: {question.Question}");
                for (int i = 0; i < question.Options.Length; i++)
                    Console.WriteLine($"{i + 1}. {question.Options[i]}");

                // Temporary: pick a random option for testing
                Random rnd = new Random();
                int selectionIndex = rnd.Next(question.Options.Length);
                string selected = question.Options[selectionIndex];

                Console.WriteLine($"Player selects: {selected}");

                // Add to history
                history.Add(("assistant", question.Question));
                history.Add(("user", selected));

                // Update suspicion
                suspicion += question.SuspicionDelta;
                Console.WriteLine($"Suspicion now: {suspicion}/100");

                questionNumber++;
            }

            Console.WriteLine("\nYou got caught!");
        }
    }
}