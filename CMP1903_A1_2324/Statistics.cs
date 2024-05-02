using System;
using System.IO;
using System.Text.Json;


namespace CMP1903_A1_2324
{
    internal class Statistics
    {
        private const string StatisticsFilePath = "statistics.json";

        public void DisplayStatistics()
        {
            // Load the statistics from the JSON file
            string json = null;
            try
            {
                json = File.ReadAllText(StatisticsFilePath);
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText(StatisticsFilePath, "{}");
                json = "{}";
            }

            var statistics = JsonSerializer.Deserialize<StatisticsData>(json);
            

            Console.WriteLine("--------Three or More Statistics---------");
            Console.WriteLine($"Games Played: {statistics.ThreeOrMoreGamesPlayed}");
            Console.WriteLine($"Highscore: {statistics.ThreeOrMoreHighscore} by User {statistics.ThreeOrMoreWinner+1}");
            Console.WriteLine($"Rounds Played: {statistics.ThreeOrMoreRoundsPlayed}");
            Console.WriteLine("------------------------------------------");


            Console.WriteLine("----------Sevens Out Statistics----------");
            Console.WriteLine($"Games Played: {statistics.SevensOutGamesPlayed}");
            Console.WriteLine($"Highscore: {statistics.SevensOutHighscore} by User {statistics.SevensOutWinner+1}");
            Console.WriteLine($"Rounds Played: {statistics.SevensOutRoundsPlayed}");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Press any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();


        }

        public void StoreThreeOrMoreStatistics(int score, int roundsPlayed, int userID)
        {
            // Load the statistics from the JSON file
            string json = null;
            try
            {
                json = File.ReadAllText(StatisticsFilePath);
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText(StatisticsFilePath, "{}");
                json = "{}";
            }

            var statistics = JsonSerializer.Deserialize<StatisticsData>(json);

            // Update the statistics for Three or More
            statistics.ThreeOrMoreGamesPlayed++;
            if (score > statistics.ThreeOrMoreHighscore)
            {
                statistics.ThreeOrMoreHighscore = score;
                statistics.ThreeOrMoreWinner = userID;
            }

            statistics.ThreeOrMoreRoundsPlayed += roundsPlayed;
            // Save the updated statistics to the JSON file
            json = JsonSerializer.Serialize(statistics);
            File.WriteAllText(StatisticsFilePath, json);
        }

        public bool StoreSevensOutStatistics(int score, int roundsPlayed, int userID)
        {
            string json = null;
            bool highscore = false;
            // Load the statistics from the JSON file
            try
            {
                json = File.ReadAllText(StatisticsFilePath);
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText(StatisticsFilePath, "{}");
                json = "{}";
            }
            
            var statistics = JsonSerializer.Deserialize<StatisticsData>(json);

            // Update the statistics for Sevens Out
            statistics.SevensOutGamesPlayed++;
            if (score > statistics.SevensOutHighscore)
            {
                statistics.SevensOutHighscore = score;
                statistics.SevensOutWinner = userID;
                highscore = true;
            }

            //update the rounds played
            statistics.SevensOutRoundsPlayed += roundsPlayed;

            // Save the updated statistics to the JSON file
            json = JsonSerializer.Serialize(statistics);
            File.WriteAllText(StatisticsFilePath, json);

            return highscore;
        }
    }

    internal class StatisticsData
    {
        public int ThreeOrMoreGamesPlayed { get; set; }
        public int ThreeOrMoreHighscore { get; set; }
        public int ThreeOrMoreWinner { get; set; }
        public int ThreeOrMoreRoundsPlayed { get; set; }
        public int SevensOutGamesPlayed { get; set; }
        public int SevensOutHighscore { get; set; }
        public int SevensOutWinner { get; set; }
        public int SevensOutRoundsPlayed { get; set; }
    }
}
