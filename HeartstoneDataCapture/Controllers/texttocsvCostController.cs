using CsvHelper;
using HeartstoneDataCapture.Models;
using HeartstoneDataCapture.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace HeartstoneDataCapture.Controllers
{
    class texttocsvCostController
    {
        public cardInfoProvider allCards = new cardInfoProvider();

        /// <summary>
        /// This method will scrape the value from text files
        /// The values of Friendly Decks and their cost will be stored 
        /// </summary>
        public List<List<List<friendDeckModel>>> scrapeFriendFromText(int genFolderCount , int iterable)
        {
            List<List<List<friendDeckModel>>> friendDecksFinal = new List<List<List<friendDeckModel>>>();
            Dictionary<string, cardDefs> cards = allCards.getAllCards();

            for (int i = 0; i < genFolderCount; i++)
            {
                Console.WriteLine("\n*********************** Scrape the friend deck from text **************************\n");
                
                List<List<friendDeckModel>> friendDecks = new List<List<friendDeckModel>>();
                
                for (int j = 0; j < iterable; j++)
                {
                    Console.WriteLine("Scraping the friend decks for gen folder no: " + i + "\n");
                    List<friendDeckModel> friendDeckTemp = new List<friendDeckModel>();
                    string fileName = "";

                    if (i == 0)
                    {
                        fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen-" + i + "/Overall/Deck" + j + "/" + 0 + "-" + j + ".txt";
                    }
                    else
                    {
                        fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen" + (i + 1) + "-0/Overall/Deck" + j + "/" + 0 + "-" + j + ".txt";
                    }

                    string lines = File.ReadAllText(fileName);
                    string[] items = lines.Split('*');

                    int count = 0;

                    foreach (string item in items)
                    {
                        friendDeckModel fModel = new friendDeckModel();
                        if (count < 30)
                        {
                            fModel.cardName = cards.GetValueOrDefault(item).dbfId;
                            fModel.cost = cards.GetValueOrDefault(item).cost;
                            friendDeckTemp.Add(fModel);
                        }
                        else
                        {
                            break;
                        }
                        count++;
                    }

                    friendDeckTemp = friendDeckTemp.OrderBy(x => x.cost).ToList();

                    friendDecks.Add(friendDeckTemp);
                }
                friendDecksFinal.Add(friendDecks);

            }
            return friendDecksFinal;
        }

        /// <summary>
        /// This method will scrape the value from text files
        /// The values of Enemy decks will be stored
        /// </summary>
        public List<List<friendDeckModel>> scrapeEnemyFromText(int genFolderCount , int iterable)
        {
            Console.WriteLine("\n*********************** Scrape the enemy deck from text **************************\n");
            
            List<List<friendDeckModel>> enemyDecks = new List<List<friendDeckModel>>();
            Dictionary<string, cardDefs> cards = allCards.getAllCards();

            for (int i = 0; i < genFolderCount; i++)
            {
                Console.WriteLine("Scraping the enemy decks for gen folder no: " + i + "\n");
                List<friendDeckModel> enemyDeckTemp = new List<friendDeckModel>();
                string fileName = "";

                if (i == 0)
                {
                    fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen-" + i + "/EnemyDeck.txt";
                }
                else
                {
                    fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen" + (i + 1) + "-0/EnemyDeck.txt";
                }



                string lines = File.ReadAllText(fileName);

                string[] content = lines.Split('\n');

                string[] cardsList = content[1].Split('*');

                int count = 0;
                foreach (string card in cardsList)
                {
                    friendDeckModel fModel = new friendDeckModel();
                    if (count < 30)
                    {
                        fModel.cardName = cards.GetValueOrDefault(card).dbfId;
                        fModel.cost = cards.GetValueOrDefault(card).cost;
                        enemyDeckTemp.Add(fModel);
                    }
                    else
                    {
                        break;
                    }
                    count++;
                }
                enemyDeckTemp = enemyDeckTemp.OrderBy(x => x.cost).ToList();

                enemyDecks.Add(enemyDeckTemp);
            }

            return enemyDecks;

        }

        /// <summary>
        /// This method will convert the Friendly Decks and their corresponding enemydeck into tuple
        /// </summary>
        /// <param name="friendDecksFinal"></param>
        /// <param name="enemyDecks"></param>
        /// <returns></returns>
        public Dictionary<List<friendDeckModel>, List<friendDeckModel>> convertToTuple(List<List<List<friendDeckModel>>> friendDecksFinal, List<List<friendDeckModel>> enemyDecks , int genFolderCount , int iterable)
        {
            Console.WriteLine("\n*********************** Join the friend deck with their enemy deck **************************\n");
            Dictionary<List<friendDeckModel>, List<friendDeckModel>> tuple = new Dictionary<List<friendDeckModel>, List<friendDeckModel>>();
            int flag = 0; int count = 0;

            foreach (List<List<friendDeckModel>> friendDeck in friendDecksFinal)
            {
                foreach (List<friendDeckModel> friendDeckTemp in friendDeck)
                {
                    if (count != 0 && count %iterable == 0 && count < (genFolderCount*iterable))
                    {
                        flag++;
                    }


                    if (flag == 10)
                    {
                        return tuple;
                    }
                    else
                    {
                        tuple.Add(friendDeckTemp, enemyDecks[flag]);
                    };
                    count++;
                }

            }

            return tuple;
        }

        public string convertToFinalStruct(Dictionary<List<friendDeckModel>, List<friendDeckModel>> tuple, List<string> playerStat , int genFolderCount , int iterable)
        {
            Console.WriteLine("\n*********************** Join the enemy and friend deck with their win **************************\n");

            string jsonStruct = "";

            List<csvStructureCost> cStruct = new List<csvStructureCost>();
            
            string[] pStat = playerStat.ToArray();

            int count = 0;
            foreach (var tunpleKey in tuple.Keys)
            {
                csvStructureCost cStructTemp = new csvStructureCost();

                if (count < (genFolderCount*iterable))
                {
                    cStructTemp.FriendDeck = tunpleKey;
                    cStructTemp.EnemyDeck = tuple.GetValueOrDefault(tunpleKey);
                    cStructTemp.WinnerDeck = pStat[count];
                }

                cStruct.Add(cStructTemp);
                count++;
            }



            jsonStruct = JsonConvert.SerializeObject(cStruct);

            /* Save the JSON string in a FinalDataJSON.json */
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FinalData/FinalDataCostJSON.json";
            System.IO.File.WriteAllText(fileName, jsonStruct);


            return jsonStruct;
        }

        public void convertToCsv(string jsonContent)
        {
            Console.WriteLine("\n*********************** Convert the JSON to CSV **************************\n");
            DataTable dtOne = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            string delimiter = ",";

            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString))
            {
                csv.Configuration.Delimiter = delimiter;

                using (var dt = dtOne)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            if (i == 0 || i == 1)
                            {
                                csv.WriteField(row[i].ToString());
                            }
                            else
                            {
                                csv.WriteField(row[i]);
                            }
                        }
                        csv.NextRecord();
                    }
                }
            }

            /* Save the JSON string in a FinalDataJSON.json */
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FinalData/FinalDataCostJSON.csv";
            System.IO.File.WriteAllText(fileName, csvString.ToString());

        }

        /// <summary>
        /// This method will get the result of the match E.g. Player 1 or Player 2
        /// </summary>
        /// <returns></returns>
        public List<string> getPlayerResult(int genFolderCount , int iterable)
        {
            Console.WriteLine("\n*********************** Get the result of match won **************************\n");

            List<string> playerStat = new List<string>();

            for (int i = 0; i < genFolderCount; i++)
            {
                Console.WriteLine("Scraping the game result for  gen folder no : " +i+"\n");
                for (int j = 0; j < iterable; j++)
                {
                    string fileName = "";

                    if (i == 0)
                    {
                        fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen-" + i + "/Overall/Deck" + j + "/" + 0 + "-" + j + ".txt";
                    }
                    else
                    {
                        fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen" + (i + 1) + "-0/Overall/Deck" + j + "/" + 0 + "-" + j + ".txt";
                    }

                    string lines = File.ReadAllText(fileName);
                    string[] items = lines.Split('/');

                    if (items[1].Contains("WON"))
                    {
                        playerStat.Add("Enemy");
                    }
                    else
                    {
                        playerStat.Add("Friend");
                    }
                }
            }

            return playerStat;

        }



    }
}
