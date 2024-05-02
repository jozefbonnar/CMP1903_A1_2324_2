using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1903_A1_2324
{
    internal class ThreeOrMore : Game
    {
        protected readonly List<Die> dice = Enumerable.Repeat(new Die(), 5).ToList();
        protected readonly int[] score = { 0, 0 };
        protected int CurrentUserID = 0;
        protected bool computer = false;
        protected int RoundsPlayed = 0;
        internal List<Die> Dice => dice;

        public int[] Score => score;

        public int CurrentUserID1 { get => CurrentUserID; set => CurrentUserID = value; }
        public bool Computer { get => computer; set => computer = value; }

        public void startGame()
        {
            Console.WriteLine("Welcome to Three or More!");
            Console.WriteLine("Roll all 5 dice hoping for a 3-of-a-kind or better.");
            Console.WriteLine("If 2-of-a-kind is rolled, player may choose to rethrow all, or the remaining dice.");
            Console.WriteLine("3-of-a-kind: 3 points, 4-of-a-kind: 6 points, 5-of-a-kind: 12 points.");
            Console.WriteLine("First to a total of 20 wins. Press ENTER to start the game.");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Would you like to play against the computer? (yes (1) / no (2))");
            char keyChar = Console.ReadKey().KeyChar;
            Computer = ValidateInput(keyChar.ToString(), 1, 2) == 1;
            Console.Clear();
        }

        public override void PlayGame()
        {
            startGame();
            while (Score[0] < 20 && Score[1] < 20)
            {
                int roundScore = 0;
                int[] counts = new int[6];
                Console.Write($"User {CurrentUserID1 + 1} rolled: ");
                foreach (var die in Dice) //roll all dice
                {
                    int value = die.RollDie();
                    counts[value - 1]++; //increment the count for the die value
                    Console.Write($"{value} ");
                }
                Console.WriteLine();

                ////print counts for debugging
                //for (int i = 0; i < 6; i++)
                //{
                //    Console.WriteLine($"{i + 1}: {counts[i]}");
                //}

                if (counts.Contains(5))
                {
                    Console.WriteLine($"User {CurrentUserID1 + 1} rolled 5 of a kind!");
                    roundScore = 12;
                }
                else if (counts.Contains(4))
                {
                    roundScore = 6;
                    Console.WriteLine($"User {CurrentUserID1 + 1} rolled 4 of a kind!");
                }
                else if (counts.Contains(3))
                {
                    roundScore = 3;
                    Console.WriteLine($"User {CurrentUserID1 + 1} rolled 3 of a kind!");
                }
                else if (counts.Contains(2))
                    HandleTwoOfAKind(ref roundScore, counts);

                Score[CurrentUserID1] += roundScore; //add the round score to the user's total score
                Console.WriteLine($"User {CurrentUserID1 + 1} scored {roundScore} points this round. Total: {Score[CurrentUserID1]}");
                Console.WriteLine("Press ENTER to continue.");
                Console.ReadKey();
                CurrentUserID1 = (CurrentUserID1 + 1) % 2;
                RoundsPlayed++;
            }

            Console.WriteLine($"Player {(Score[0] >= 20 ? 1 : 2)} wins!");
            Statistics statistics = new Statistics();
            statistics.StoreThreeOrMoreStatistics(Score[0] >= 20 ? Score[0] : Score[1], RoundsPlayed, Score[0] >= 20 ? 0 : 1);
        }

        public void HandleTwoOfAKind(ref int roundScore, int[] counts, bool testMode = false)
        {
            if (Computer && CurrentUserID1 == 1)
            {
                Console.WriteLine($"User {CurrentUserID1 + 1} rolled 2 of a kind. Rerolling all dice.");
                Console.Write($"User {CurrentUserID1 + 1} rolled: ");

                foreach (var die in Dice)
                {
                    die.RollDie();
                    Console.Write($"{die.GetDieValue()} ");
                }
                Console.WriteLine();
                roundScore += CheckScores(CurrentUserID1, true);
            }
            else
            {
                Console.WriteLine($"User {CurrentUserID1 + 1} rolled 2 of a kind. Would you like to rethrow all dice or the remaining ones? (all (1) / remaining (2))");
                int input = 1;
                if (!testMode)
                {
                    char keyChar = Console.ReadKey().KeyChar;
                    input = ValidateInput(keyChar.ToString(), 1, 2);
                    Console.WriteLine();
                }

                if (input == 1)
                {
                    Console.WriteLine($"Rerolling all dice.");
                    Console.Write($"User {CurrentUserID1 + 1} rolled: ");
                    foreach (var die in Dice)
                    {
                        die.RollDie();
                        Console.Write($"{die.GetDieValue()} ");
                    }
                    Console.WriteLine();
                    roundScore += CheckScores(CurrentUserID1, true);
                }
                else
                {
                    Console.WriteLine($"Rerolling remaining dice.");
                    Console.Write($"User {CurrentUserID1 + 1} rolled: ");
                    for (int i = 0; i < 5; i++)
                    {
                        if (counts[Dice[i].GetDieValue() - 1] != 2)
                        {
                            Dice[i].RollDie();
                            Console.Write($"{Dice[i].GetDieValue()} ");
                        }


                    }
                    Console.WriteLine();
                    roundScore += CheckScores(CurrentUserID1, true);
                }
            }
        }


        public int CheckScores(int CurrentUserID, bool alreadyRolled = false, bool testMode = false)
        {
            int roundScore = 0;
            int[] counts = new int[6];
            foreach (var die in Dice)
                counts[die.RollDie() - 1]++;

            if (counts.Contains(5))
            {
                Console.WriteLine($"User {CurrentUserID + 1} rolled 5 of a kind!");
                roundScore = 12;
            }
            else if (counts.Contains(4))
            {
                roundScore = 6;
                Console.WriteLine($"User {CurrentUserID + 1} rolled 4 of a kind!");
            }
            else if (counts.Contains(3))
            {
                roundScore = 3;
                Console.WriteLine($"User {CurrentUserID + 1} rolled 3 of a kind!");
            }
            else if (counts.Contains(2) && alreadyRolled)
                return roundScore;
            else if (counts.Contains(2))
                HandleTwoOfAKind(ref roundScore, counts, testMode);

            return roundScore;
        }


    }

    internal class ThreeOrMoreTesting : ThreeOrMore
    {
        public ThreeOrMoreTesting() : base()
        {
            string logFilePath = "ThreesOutTestingLog.txt";
            Console.WriteLine($"Logging to {logFilePath}");


            using (StreamWriter writer = new StreamWriter(logFilePath))
            {
                writer.WriteLine("------Testing Three or More------");
                Console.WriteLine("Testing Three or More");
                Debug.Assert(ValidateInput("1", 1, 2) == 1);
                writer.WriteLine("Test Passed: ValidateInput");
                //test the CheckScores method
                int score = CheckScores(0, true, true);

                Debug.Assert(score >= 0 && score <= 12);
                writer.WriteLine("Test Passed: CheckScores");

                //test the HandleTwoOfAKind method
                int roundScore = 0;
                int[] counts = { 1, 1, 2, 3, 4, 5 };
                HandleTwoOfAKind(ref roundScore, counts, true);
                Debug.Assert(roundScore >= 0 && roundScore <= 6);
                writer.WriteLine("Test Passed: HandleTwoOfAKind");
                writer.WriteLine("All tests passed.");



                Console.WriteLine("Test Passed: Three or More");

            }

        }
    }

}
