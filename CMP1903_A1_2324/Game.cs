using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1903_A1_2324
{
    internal abstract class Game
    {
        static void Main(string[] args)
        {
            //Start the menu
            bool active = true;
            while (active)
            {


                Console.WriteLine("Welcome to the Game Menu");
                Console.WriteLine("1. Sevens Out");
                Console.WriteLine("2. Three or More");
                Console.WriteLine("3. View Statistics");
                Console.WriteLine("4. Testing");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Please select an option: ");
                string input = Console.ReadLine();
                //validate input
                if (int.TryParse(input, out int result))
                {
                    if (result > 0 && result < 6)
                    {
                        switch (input)
                        {
                            case "1":
                                Game sevensOut = new SevensOut();

                                sevensOut.PlayGame();
                                break;
                            case "2":
                                Game threeOrMore = new ThreeOrMore();
                                threeOrMore.PlayGame();
                                break;
                            case "3":
                                Statistics statistics = new Statistics();
                                statistics.DisplayStatistics();
                                 break;
                            case "4":
                                //Test testing = new Test();
                                //testing.PerformTests();
                                SevensOutTesting sevensOutTesting = new SevensOutTesting();
                                sevensOutTesting.TestSevensOut();
                                ThreeOrMoreTesting threeOrMoreTesting = new ThreeOrMoreTesting();
                                Console.ReadKey();
                                break;
                            case "5":
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please try again.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            Console.ReadKey();
        }

        public abstract void PlayGame();

        public int ValidateInput(string input, int min, int max)
        {
            int number;
            while (!int.TryParse(input, out number) || number < min || number > max)
            {
                Console.WriteLine($"Invalid input. Please enter a number between {min} and {max}:");
                input = Console.ReadLine();
            }
            return number;
        }
    }


}