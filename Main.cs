using System;
using System.Diagnostics;
using System.IO;

using StatisticsNamespace;
using TestingNamespace;
using GameNamespace;

namespace MainNamespace
{
    class Program
    {
        // Establish program variables
        static string StatisticsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "StatisticsData.txt");
        static string opening = "Alexander Worthy 28605933 OOP Assessment 2\n\nPlease choose from the following:\n1. Sevens Out\n2. Three or More\n3. View Statistics\n4. Exit\nEnter Here:";
		

		
        public static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(opening);
                Testing.AlignCursorToLastLineWithMostRecentStringLength("Enter Here: ".Length);
                string choice = Console.ReadLine();
                if (choice == "4") // Exit
                {
                    break;
                }

                GameNamespace.SevensOutGame GameObject = null;
                if (choice == "1")
                {
                    // prompt player to choose opponent
                    GameObject = new GameNamespace.SevensOutGame();

                    GameNamespace.SevensOutGame sevensOutGame = GameObject as GameNamespace.SevensOutGame;
                    if (sevensOutGame != null)
                    {
                        sevensOutGame.AgainstComputer = GameNamespace.Game.ChooseOpponent();
                    }

                    GameObject = (GameNamespace.SevensOutGame)TestingNamespace.Testing.SevensOut(GameObject);
                    StatisticsNamespace.Statistics.WriteDataToFile(GameObject.Winner,GameObject.GameTime,true,GameObject.TimesDiceRolled);
                    
                }
                // three or more
                else if (choice == "2")
                {
                    // prompt player to choose opponent
                    GameNamespace.ThreeOrMoreGame ThreeOrMoreGameObject = new GameNamespace.ThreeOrMoreGame();
                    if (ThreeOrMoreGameObject != null)
                    {
                        ThreeOrMoreGameObject.AgainstComputer = GameNamespace.Game.ChooseOpponent();
                    }
                    ThreeOrMoreGameObject = (GameNamespace.ThreeOrMoreGame)TestingNamespace.Testing.ThreeOrMore(ThreeOrMoreGameObject);
                    StatisticsNamespace.Statistics.WriteDataToFile(ThreeOrMoreGameObject.Winner,ThreeOrMoreGameObject.GameTime,true,ThreeOrMoreGameObject.TimesDiceRolled);
                }
                // view stats
                else if (choice == "3")
                {
                    // display stats
                    
                    Statistics.GetGameStats();
                    Console.WriteLine("Press Enter to return to the main menu. ");
                    Console.ReadLine();
                }
                else
                {
                	Console.Clear();
                    Console.WriteLine("Invalid input! Press Enter to continue.");
                    Console.ReadLine();
                }
            }
        }
    }
}
