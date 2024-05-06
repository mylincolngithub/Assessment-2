using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using GameNamespace;

namespace TestingNamespace
{
    public class Testing
    {
        static Random RandObject = new Random();
        static string promptplayertorolldice = "Player {0}, press enter to roll the dice!";
        static string ComputerRollingPrompt = "Computer player is rolling the dice...";
        static string two_of_a_kind_message = "There were fewer than 3 of a kind. Reroll the remainders, reroll all dice, or not reroll?\n1. Reroll remainders\n2. Reroll all dice\n3. No reroll\nEnter Here: ";

        public static void AlignCursorToLastLineWithMostRecentStringLength(int MostRecentStringLength)
        {
            // Assuming it's called after Console.WriteLine()
            Console.SetCursorPosition(Console.CursorLeft + MostRecentStringLength, Console.CursorTop - 1);
        }

        public static Dictionary<int, int> GetFrequencyDictionaryFromList(List<int> DiceValues)
        {
        	Console.Clear();
        	Console.WriteLine("Player rolled...");
            foreach (int newvalue in DiceValues)
            {
                Console.WriteLine(newvalue + ",");
            }
            Console.ReadLine();
            var DiceDictionary = DiceValues.GroupBy(x => x).ToDictionary(Group => Group.Key, Group => Group.Count());
            return DiceDictionary;
        }

        public static int GetHighestFrequencyFromList(List<int> DiceValues)
        {
            Dictionary<int, int> DiceDictionary = GetFrequencyDictionaryFromList(DiceValues);
            var HighestFrequency = DiceDictionary.Max(Group => Group.Value);
            return HighestFrequency;
        }

        public static object CheckAgainstScores(int HighestFrequency, GameNamespace.ThreeOrMoreGame GameObject, bool Player)
        {
            int Increment = new int();
            if (HighestFrequency == 3)
            {
                Increment = 3;
            }
            else if (HighestFrequency == 4)
            {
                Increment = 6;
            }
            else if (HighestFrequency == 5)
            {
                Increment = 12;
            }

            if (Player == true)
            {
                GameObject.Player1Score += Increment;
            }
            else
            {
                GameObject.Player2Score += Increment;
            }
            return GameObject;
        }
        public static object HighestFrequencyToScore(List<int> DiceValues, Game GameObject, bool Player)
        {
            int HighestFrequency = GetHighestFrequencyFromList(DiceValues);
            if (HighestFrequency == 2)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(two_of_a_kind_message);
                   
                    string choice = Console.ReadLine();
                    Console.Clear();
                    if (choice == "1")
                    {
                        // Reroll remainders 
                        List<int> NewDiceValues = new List<int>();
        
                        Dictionary<int, int> DiceDictionary = GetFrequencyDictionaryFromList(DiceValues);
        
                        var DiceWithHighestFrequency = DiceDictionary.FirstOrDefault(x => x.Value == HighestFrequency).Key;
        
                        foreach (var key in DiceValues)
                        {
                            if (key == DiceWithHighestFrequency)
                            {
                                NewDiceValues.Add(key);
                            }
                        }
        
                        List<int> ThreeNewDiceValues = GameObject.RollDiceXTimes(3);
        
                        foreach (int OneOfThreeDiceValues in ThreeNewDiceValues)
                        {
                            NewDiceValues.Add(OneOfThreeDiceValues);
                        }
        
                        int HighestFrequencyOfNewDiceValues = GetHighestFrequencyFromList(NewDiceValues);
        
                        GameObject = (GameNamespace.ThreeOrMoreGame)CheckAgainstScores(HighestFrequencyOfNewDiceValues, (GameNamespace.ThreeOrMoreGame)GameObject, Player);
                        return GameObject;
                    }
                    else if (choice == "2")
                    {
                        // Reroll all - simply run the same code again
                        int NewHighestFrequency = GetHighestFrequencyFromList(DiceValues);
                        GameObject = (GameNamespace.ThreeOrMoreGame)CheckAgainstScores(NewHighestFrequency, (GameNamespace.ThreeOrMoreGame)GameObject, Player);
                        return GameObject;
                    }
                    else if (choice == "3")
                    {
                        // No reroll - move on
                        return GameObject;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Press Enter to continue");
                        Console.ReadLine();
                    }
                }
            }
            else if (HighestFrequency > 2)
            {
                GameObject = (GameNamespace.ThreeOrMoreGame)HighestFrequencyToScore(DiceValues, (GameNamespace.ThreeOrMoreGame)GameObject, true);
            }
            return GameObject;
        }


        public static object SevensOut(GameNamespace.SevensOutGame GameObject)
        {
            string playernum = "1";
            int RoundNumber = 1;
            int DiceRolled = 0;
            Stopwatch Timer = new Stopwatch();
            Timer.Start();

            while (true)
            {
                //player 1's turn
                Console.Clear();
                playernum = "1";
                Console.WriteLine("Round " + RoundNumber);
                string newpromptplayer = string.Format(promptplayertorolldice, playernum);
                Console.WriteLine(newpromptplayer);
                Console.ReadLine();
                List<int> DiceValues = GameObject.RollDiceXTimes(2);
                //get the total from the list of integers
                foreach (int newvalue in DiceValues)
                {
                    GameObject.Player1DiceTotal += newvalue; //add to the player's total
                }
                DiceRolled += 2;
                Debug.Assert(GameObject.Player1DiceTotal == 7, "Player 1's dice totalled " + GameObject.Player1DiceTotal);
                Console.Clear();
                Console.WriteLine("Player " + playernum + " scored a " + GameObject.Player1DiceTotal + "\nPress Enter to continue.");
                Console.ReadLine();
                
                if (GameObject.Player1DiceTotal == 7)
                {
                    Timer.Stop();
                    int TimeElapsed = (int)Math.Truncate(Timer.Elapsed.TotalSeconds);
                    GameObject.Winner = true;
                    GameObject.GameTime = TimeElapsed;
                    GameObject.TimesDiceRolled = DiceRolled;


                    return GameObject;
                }
                //if made it this far, player 1 didnt win. Now it's player 2's turn
                Console.Clear();
                playernum = "2";
                if (GameObject.AgainstComputer)
                {
                    Console.WriteLine(ComputerRollingPrompt);
                    Thread.Sleep(RandObject.Next(1, 3));
                }
                else
                {
                    newpromptplayer = string.Format(promptplayertorolldice, playernum);
                    Console.WriteLine(newpromptplayer);
                    Console.ReadLine();
                }

                //player 2 rolls dice
                DiceValues = GameObject.RollDiceXTimes(2);
                //get the total from the list of integers
                foreach (int newvalue in DiceValues)
                {
                    GameObject.Player2DiceTotal += newvalue; //add to the player's total
                }
                DiceRolled += 2;
                Debug.Assert(GameObject.Player2DiceTotal == 7, "Player 2's dice totalled " + GameObject.Player2DiceTotal);
                Console.WriteLine("Player " + playernum + " scored a " + GameObject.Player2DiceTotal);
                if (GameObject.Player2DiceTotal == 7)
                {
                    Timer.Stop();
                    int TimeElapsed = (int)Math.Truncate(Timer.Elapsed.TotalSeconds);
                    GameObject.GameTime = TimeElapsed;
                    return GameObject;
                }

                DiceRolled += 2;
                //round ended
                RoundNumber += 1;
                GameObject.Player1DiceTotal = 0;
                GameObject.Player2DiceTotal = 0;
                Console.Clear();
                Console.WriteLine("No-one scored a 7. Press Enter to start the next round.");
                Console.ReadLine();
            }




        }
        public static GameNamespace.ThreeOrMoreGame ThreeOrMore(GameNamespace.ThreeOrMoreGame GameObject)
        {
            string playernum = "1";
            int RoundNumber = 1;
            int DiceRolled = 0;
            Stopwatch Timer = new Stopwatch();
            Timer.Start();

            while (true)
            {
                //player 1's turn
                Console.Clear();
                playernum = "1";
                Console.WriteLine("Round " + RoundNumber);
                string newpromptplayer = string.Format(promptplayertorolldice, playernum);
                Console.WriteLine(newpromptplayer);
                Console.ReadLine();
                List<int> DiceValues = GameObject.RollDiceXTimes(5);
                //get the total from the list of integers
                

                DiceRolled += 5;

                GameObject = (GameNamespace.ThreeOrMoreGame)Testing.HighestFrequencyToScore(DiceValues, GameObject, true);

                Debug.Assert(GameObject.Player1Score > 19, "Player 1's score has reached " + GameObject.Player1Score);
                Console.Clear();

                if (GameObject.Player1Score > 19)
                {
                    Timer.Stop();
                    int TimeElapsed = (int)Math.Truncate(Timer.Elapsed.TotalSeconds);
                    GameObject.Winner = true;
                    GameObject.GameTime = TimeElapsed;
                    GameObject.TimesDiceRolled = DiceRolled;


                    return GameObject;
                }
                //if made it this far, player 1 didnt win. Now it's player 2's turn
                Console.Clear();
                playernum = "2";
                if (GameObject.AgainstComputer)
                {
                    Console.WriteLine(ComputerRollingPrompt);
                    Thread.Sleep(RandObject.Next(1, 3));
                }
                else
                {
                    newpromptplayer = string.Format(promptplayertorolldice, playernum);
                    Console.WriteLine(newpromptplayer);
                    Console.ReadLine();
                }

                //player 2 rolls dice
                DiceValues = GameObject.RollDiceXTimes(5);
                Console.WriteLine("Player " + playernum + " rolled...");
                foreach (int newvalue in DiceValues)
                {
                    Console.WriteLine(newvalue + ",");
                }



                GameObject = (GameNamespace.ThreeOrMoreGame)Testing.HighestFrequencyToScore(DiceValues, GameObject, false);


                DiceRolled += 5;
                Debug.Assert(GameObject.Player2Score > 19, "Player 2's score has reached " + GameObject.Player2Score);
                Console.Clear();
                Console.WriteLine("Player " + playernum + " scored a " + GameObject.Player2Score);
                if (GameObject.Player2Score > 19)
                {
                    Timer.Stop();
                    int TimeElapsed = (int)Math.Truncate(Timer.Elapsed.TotalSeconds);
                    GameObject.Winner = false;
                    GameObject.GameTime = TimeElapsed;
                    GameObject.TimesDiceRolled = DiceRolled;


                    return GameObject;
                }

                DiceRolled += 2;
                //round ended
                RoundNumber += 1;
                Console.Clear();
            }
        }
    }
}
