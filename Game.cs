using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace GameNamespace
{
    public abstract class Game
    {
        public bool AgainstComputer { get; set; }
        public int Player1DiceTotal { get; set; }
        public int Player2DiceTotal { get; set; }
        public int GameTime { get; set; }
        public int TimesDiceRolled { get; set; }
        public bool Winner { get; set; }



        public virtual List<int> RollDiceXTimes(int x)
        {
            List<int> DiceValues = new List<int>(); 
            Random RandObject = new Random();
            for (int i = 0; i < x; i++) 
            {
                DiceValues.Add(RandObject.Next(1, 7)); 
            }
            return DiceValues;
        }
        public static bool ChooseOpponent()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please choose your opponent mode:\n1. 2 Player\n2. Computer\n3. Back\nEnter Here:");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    return false;
                }
                else if (choice == "2")
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid input! Press enter to continue");
                    Console.ReadLine();
                }
            }
        }
    }

    public class SevensOutGame : Game
    {
    
    }

    public class ThreeOrMoreGame : Game
    {
        public int Player1Score;
        public int Player2Score;

    }
}
