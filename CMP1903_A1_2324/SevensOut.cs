using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1903_A1_2324
{
    internal class SevensOut : Game
    {
        private readonly Die[] dice = { new Die(), new Die() };
        private int[] total = { 0, 0 };
        private int[] score = { 0, 0 };
        private int currentUserID = 0;
        private bool computer = false;
        private int roundsPlayed = 0;
        private bool testingMode = false;

        internal Die[] Dice => Dice1;

        internal Die[] Dice1 => dice;

        public int[] Total { get => total; set => total = value; }
        public int[] Score { get => score; set => score = value; }
        public int CurrentUserID { get => currentUserID; set => currentUserID = value; }
        public bool Computer { get => computer; set => computer = value; }
        public int RoundsPlayed { get => roundsPlayed; set => roundsPlayed = value; }
        public bool TestingMode { get => testingMode; set => testingMode = value; }

        public SevensOut(bool Testing = false)
        {
            Console.Clear();
            TestingMode = Testing;
            Console.WriteLine("Welcome to Sevens Out");
            Console.WriteLine("Would you like to play against the computer? (Yes (1)/ No (2)");
            if (!TestingMode)
            {
                //use validate input to check the user input
                char keyChar = Console.ReadKey().KeyChar;
                Computer = ValidateInput(keyChar.ToString(), 1, 2) == 1;
                Console.Clear();

            }

        }

        public override void PlayGame()
        {

            bool playAgain = true;
            while (playAgain)
            {
                while (Total[CurrentUserID] != 7)
                {
                    RoundsPlayed++;
                    Total[CurrentUserID] = 0;
                    if (!Computer || CurrentUserID == 0)
                    {
                        Console.Write($"User {CurrentUserID + 1} Press ENTER to roll the dice");
                        if (!TestingMode)
                        {
                            Console.ReadKey();
                            Console.WriteLine();
                        }
                    }

                    RollDice();

                    if (Total[CurrentUserID] == 7)
                    {
                        Console.WriteLine($"User {CurrentUserID + 1} rolled {Dice[0].GetDieValue()} and {Dice[1].GetDieValue()}, which totals 7. Game Over. {(CurrentUserID == 0 ? "Your" : "Computer's")} scored: {Score[CurrentUserID]}");
                        break; // Exit the loop if the total is 7
                    }
                    else if (Dice[0].GetDieValue() == Dice[1].GetDieValue())
                    {
                        Score[CurrentUserID] = Score[CurrentUserID] + (Total[CurrentUserID] * 2);
                        Console.WriteLine($"User {CurrentUserID + 1} rolled a double {Dice[0].GetDieValue()}. {(CurrentUserID == 0 ? "Your" : "Computer's")} score is: " + Score[CurrentUserID]);
                    }
                    else
                    {
                        Score[CurrentUserID] += Total[CurrentUserID];
                        Console.WriteLine($"User {CurrentUserID + 1} rolled {Dice[0].GetDieValue()} and {Dice[1].GetDieValue()}, which totals {Total[CurrentUserID]}. {(CurrentUserID == 0 ? "Your" : "Computer's")} score is: " + Score[CurrentUserID]);
                    }

                    CurrentUserID = (CurrentUserID + 1) % 2;
                }

                int winner = Score[0] > Score[1] ? 0 : 1;
                int otherPlayer = winner == 0 ? 1 : 0;

                Console.WriteLine($"Player 1 score: {Score[0]}");
                Console.WriteLine($"Player 2 score: {Score[1]}");
                Console.WriteLine($"Player {winner + 1} wins with a score of {Score[winner]}!");
                Console.WriteLine($"Player {otherPlayer + 1} score: {Score[otherPlayer]}");
                Statistics statistics = new Statistics();

                bool newHighscore = statistics.StoreSevensOutStatistics(Score[winner], RoundsPlayed, winner);
                if (newHighscore)
                {
                    Console.WriteLine("Congratulations! You have set a new high score!");
                }
                Console.WriteLine();

                Console.WriteLine("Would you like to play again? (Yes (1)/No (2)");
                if (!TestingMode)
                {
                    char keyChar = Console.ReadKey().KeyChar;
                    playAgain = ValidateInput(keyChar.ToString(), 1, 2) == 1;
                    if (playAgain)
                    {
                        ResetGameState();
                    }
                    else
                    {
                        playAgain = false;
                    }
                }
                else
                {
                    playAgain = false;
                }


            }
        }

        public void RollDice()
        {
            for (int i = 0; i < 2; i++)
            {
                Total[CurrentUserID] += Dice[i].RollDie();
            }
        }

        public void ResetGameState()
        {
            Total = new int[] { 0, 0 };
            Score = new int[] { 0, 0 };
            CurrentUserID = 0;
            RoundsPlayed = 0;
        }

    }

    internal class SevensOutTesting
    {
        public void TestSevensOut()
        {
            Console.WriteLine("------Testing Sevens Out------");

            string logFilePath = "SevensOutTestLog.txt";
            using (StreamWriter writer = new StreamWriter(logFilePath))
            {
                WriteLog("Log file for Sevens Out testing", writer);
                WriteLog("Game instance details initially:", writer);
                WriteGameInstanceDetails(new SevensOut(true), writer);
                TestGameInstance(new SevensOut(true), writer);
                writer.WriteLine("All tests passed");
            }

            Console.WriteLine("Log file created: SevensOutTestLog.txt");
        }

        private void WriteLog(string message, StreamWriter writer)
        {
            writer.WriteLine(message);
            writer.WriteLine("-----------------------------");
            writer.WriteLine();
        }

        private void WriteGameInstanceDetails(SevensOut sevensOut, StreamWriter writer)
        {
            writer.WriteLine($"Total[0]: {sevensOut.Total[0]}");
            writer.WriteLine($"Total[1]: {sevensOut.Total[1]}");
            writer.WriteLine($"Score[0]: {sevensOut.Score[0]}");
            writer.WriteLine($"Score[1]: {sevensOut.Score[1]}");
            writer.WriteLine($"CurrentUserID: {sevensOut.CurrentUserID}");
            writer.WriteLine($"RoundsPlayed: {sevensOut.RoundsPlayed}");
            writer.WriteLine($"Computer: {sevensOut.Computer}");
            writer.WriteLine($"TestingMode: {sevensOut.TestingMode}");
            writer.WriteLine();
        }

        private void TestGameInstance(SevensOut sevensOut, StreamWriter writer)
        {
            
            Debug.Assert(sevensOut != null, "Failed to create a game instance");
            Debug.Assert(sevensOut.Total[0] == 0 && sevensOut.Total[1] == 0 && sevensOut.Score[0] == 0 && sevensOut.Score[1] == 0 && sevensOut.CurrentUserID == 0 && sevensOut.RoundsPlayed == 0 && !sevensOut.Computer && sevensOut.TestingMode, "Game instance not initialized correctly");
            sevensOut.RollDice();
            Debug.Assert(sevensOut.Total[0] >= 2 && sevensOut.Total[0] <= 12);
            sevensOut.PlayGame();
            Debug.Assert(sevensOut.Total[0] == 7 || sevensOut.Total[1] == 7);
            Debug.Assert(sevensOut.RoundsPlayed >= 0);
            WriteLog("Game instance details after playing:", writer);
            WriteGameInstanceDetails(sevensOut, writer);
            WriteLog("Game instance details after reset:", writer);
            sevensOut.ResetGameState();
            Debug.Assert(sevensOut.Total[0] == 0 && sevensOut.Total[1] == 0 && sevensOut.Score[0] == 0 && sevensOut.Score[1] == 0 && sevensOut.CurrentUserID == 0 && sevensOut.RoundsPlayed == 0 && !sevensOut.Computer && sevensOut.TestingMode, "Game instance not reset correctly");
            WriteGameInstanceDetails(sevensOut, writer);
            Die die = new Die();
            Debug.Assert(die.RollDie() >= 1 && die.RollDie() <= 6);
            WriteLog("Die class tests passed", writer);

        }
    }

}
