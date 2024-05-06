using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StatisticsNamespace
{
    public class Statistics
    {
    	
    	private static Dictionary<string, object> GetDictionaryFromJSON(string JSONString)
    	{
    		return JsonConvert.DeserializeObject<Dictionary<string, object>>(JSONString);
    	}
    	
    	private static string GetJSONFromDictionary(Dictionary<string, object> DictionaryToConvert)
    	{
    		return JsonConvert.SerializeObject(DictionaryToConvert, Formatting.Indented);
    	}
    	
    	private static string ReadJSONFromFile()
    	{
    		string filePath = Path.Combine(Directory.GetCurrentDirectory(), "StatisticsData.txt");
    		return File.ReadAllText(filePath);
    	}
    	
       	private static List<int> ReadDataFromFile(bool Gametype)
	    {
		   	string ReadInJSON = ReadJSONFromFile();
	       	var ReadInData = GetDictionaryFromJSON(ReadInJSON);
	       	var GametypeSpecificList = Gametype ? ((JArray)ReadInData["SevensOut"]).ToObject<List<int>>() : ((JArray)ReadInData["ThreeOrMore"]).ToObject<List<int>>();
	       	return GametypeSpecificList;
        }



        private static List<int> EditData(List<int> ReadInData, int Gametime, bool winner)
        {
            ReadInData[0] += 1; // Increase num of games by 1

            if (Gametime < ReadInData[5])
            {
                ReadInData[5] = Gametime;
            }

            if (winner) // Player 1 won
            {
                ReadInData[3] += 1;
                if (ReadInData[1] < ReadInData[3])
                {
                    ReadInData[1] = ReadInData[3];
                }
            }
            else
            {
                ReadInData[4] += 1;
                if (ReadInData[2] < ReadInData[4])
                {
                    ReadInData[2] = ReadInData[4];
                }
            }
            return ReadInData;
        }

       	private static void CreateDataTemplate()
        {
       		Dictionary<string,object> DictionaryTemplate = new Dictionary<string,object>
       		{
       			{"SevensOut",new List<int> {0,0,0,0,0}},
       			{"ThreeOrMore",new List<int> {0,0,0,0,0}},
       			{"LifeTimeDiceRolls",0},
       		};

            string TemplateAsJSON = GetJSONFromDictionary(DictionaryTemplate);

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "StatisticsData.txt");
            File.WriteAllText(filePath, TemplateAsJSON);
        }

        public static void WriteDataToFile(bool winner, int Gametime, bool Gametype, int TotalDiceRolls)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "StatisticsData.txt");

            if (File.Exists(filePath))
            {
                string ReadInData = ReadJSONFromFile();
                var ConvertedJSON = GetDictionaryFromJSON(ReadInData);
				
                string GameTypeName = Gametype ? "SevensOut" : "ThreeOrMore";
                
                List<int> DataToEdit = (List<int>)ConvertedJSON[GameTypeName];
                
                DataToEdit = EditData(DataToEdit, Gametime, winner);
				
                ConvertedJSON[GameTypeName] = DataToEdit;
                
                string BackToJSON = GetJSONFromDictionary(ConvertedJSON);

                File.WriteAllText(filePath, BackToJSON);
            }
            else
            {
                // File doesn't exist - create new template
                CreateDataTemplate();
            }
        }
		public static void GetGameStats()
		{
		    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "StatisticsData.txt");
		
		    if (File.Exists(filePath))
		    {
		        string FileAsJSON = ReadJSONFromFile();
		        var FileData = GetDictionaryFromJSON(FileAsJSON);
		
		        var SevensOutData = ((List<int>)FileData["SevensOut"]);
		        var ThreeOrMoreData = ((List<int>)FileData["ThreeOrMore"]);
		        var LifeTimeDiceRolls = (int)FileData["LifeTimeDiceRolls"];
		
		        List<string> DataTypes = new List<string> { "Number of Games played: ", "Longest Player 1 winstreak: ", "Longest Player 2 winstreak: ", "Current winstreak of Player 1: ", "Current winstreak of Player 2: ", "Fastest Game time: " };
		        Console.WriteLine("Sevens Out");
		
		        for (int i = 0; i < 5; i++)
		        {
		            Console.WriteLine(DataTypes[i] + SevensOutData[i]);
		        }
		        Console.WriteLine("Three Or More");
		        for (int i = 0; i < 5; i++)
		        {
		            Console.WriteLine(DataTypes[i] + ThreeOrMoreData[i]);
		        }
		        Console.WriteLine("Lifetime Dice Rolls: " + LifeTimeDiceRolls);
		    }
		    else
		    {
		        Console.WriteLine("No game data exists. Play some games to create some data!");
		    }
		}

    }
}
