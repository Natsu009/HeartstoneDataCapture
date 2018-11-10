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
    class winRateController
    {
        public cardInfoProvider allCards = new cardInfoProvider();

        /// <summary>
        /// This method will scrape the value from text files
        /// The values of Friendly Decks will be stored
        /// </summary>
        public List<List<List<int>>> scrapeFriendFromText(int genFolderCount , int iterable)
        {
            List<List<List<int>>> friendDecksFinal = new List<List<List<int>>>();
            Dictionary<string, cardDefs> cards = allCards.getAllCards();

            Console.WriteLine("*************** Get the friend decks from Gen Folders ******************\n\n");
            for (int i = 0; i < genFolderCount; i++)
            {
                Console.WriteLine("Scraping friend decks info for gen Folder no: " + i);
                List<List<int>> friendDecks = new List<List<int>>();
                for (int j = 0; j < iterable; j++)
                {
                    List<int> friendDeckTemp = new List<int>();
                    string fileName = "";

                    if (i == 0)
                    {
                        fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDeckDuplicate150/gen-" + i + "/Overall/Deck" + j + "/" + 0 + "-" + j + ".txt";
                    }
                    else
                    {
                        fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDeckDuplicate150/gen" + (i + 1) + "-0/Overall/Deck" + j + "/" + 0 + "-" + j + ".txt";
                    }

                    string lines = File.ReadAllText(fileName);
                    string[] items = lines.Split('*');

                    int count = 0;
                    List<friendDeckModel> fModelWhole = new List<friendDeckModel>();

                    foreach (string item in items)
                    {

                        friendDeckModel fModel = new friendDeckModel();
                        if (count < 30)
                        {
                            fModel.cardName = cards.GetValueOrDefault(item).dbfId;
                            fModel.cost = cards.GetValueOrDefault(item).cost;
                            fModelWhole.Add(fModel);
                        }
                        else
                        {
                            break;
                        }
                        count++;
                    }

                    fModelWhole = fModelWhole.OrderBy(x => x.cost).ToList();

                    foreach (friendDeckModel fMod in fModelWhole)
                    {
                        friendDeckTemp.Add(fMod.cardName);
                    }
                    friendDecks.Add(friendDeckTemp);
                }
                friendDecksFinal.Add(friendDecks);

            }
            return friendDecksFinal;
        }


        public List<List<int>> scrapeEnemyFromText(int genFolderCount , int iterable)
        {
            Console.WriteLine("*************** Get the enemy decks from gen folders ******************\n\n");
            List<List<int>> enemyDecks = new List<List<int>>();
            Dictionary<string, cardDefs> cardsList = allCards.getAllCards();

            for (int i = 0; i < genFolderCount; i++)
            {
                Console.WriteLine("Scraping enemy decks info for gen Folder no: " + i);
                List<int> enemyDeckTemp = new List<int>();
                string fileName = "";

                if (i == 0)
                {
                    fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDeckDuplicate150/gen-" + i + "/EnemyDeck.txt";
                }
                else
                {
                    fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDeckDuplicate150/gen" + (i + 1) + "-0/EnemyDeck.txt";
                }

                string lines = File.ReadAllText(fileName);

                string[] content = lines.Split('\n');

                string[] cards = content[1].Split('*');

                List<friendDeckModel> fModelWhole = new List<friendDeckModel>();
                int count = 0;
                foreach (string card in cards)
                {
                    friendDeckModel fModel = new friendDeckModel();
                    if (count < 30)
                    {

                        fModel.cardName = cardsList.GetValueOrDefault(card).dbfId;
                        fModel.cost = cardsList.GetValueOrDefault(card).cost;
                        fModelWhole.Add(fModel);
                    }
                    else
                    {
                        break;
                    }
                    count++;
                }

                fModelWhole = fModelWhole.OrderBy(x => x.cost).ToList();

                foreach (friendDeckModel fMod in fModelWhole)
                {
                    enemyDeckTemp.Add(fMod.cardName);
                }

                enemyDecks.Add(enemyDeckTemp);


            }

            return enemyDecks;

        }

        /// <summary>
        /// This method will calculate the WinRate of each and every decks on the basis of the total GamePlayed
        /// </summary>
        /// <param name="deckPerGen"></param>
        /// <param name="genFolderDups"></param>
        /// <param name="genFolderCount"></param>
        /// <param name="gamePerDeck"></param>
        /// <returns></returns>
        public List<double> getWinRate(int deckPerGen, int genFolderDups, int genFolderCount , int gamePerDeck) {

            Console.WriteLine("*************** Calculate the WinRate of the Folders ******************\n\n");

            List<double> finalWinRate = new List<double>();
            for(int i = 0; i < genFolderCount; i++)
            {
                Console.WriteLine("For gen Folder: " + i);
                List<List<int>> dupsWinRate = new List<List<int>>();
                for (int j = 0; j < genFolderDups; j++)
                {
                    List<int> tempdupsWinRate = new List<int>();
                    int totalWinRate = 0; 
                    for (int k = 0; k < deckPerGen; k++)
                    {
                        

                        string fileName = "";

                        if (i == 0)
                        {
                            fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDeckDuplicate150/gen-" + j + "/Overall/Deck" + k + "/" + 0 + "-" + k + ".txt";
                        }
                        else
                        {
                            fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDeckDuplicate150/gen" + (i + 1) + "-"+j+"/Overall/Deck" + k + "/" + 0 + "-" + k + ".txt";
                        }

                        

                        string lines = File.ReadAllText(fileName);
                        string[] itemList = lines.Split("/n");

                        int count = 0;
                        foreach (string items in itemList) {
                            // Get the result
                            string [] item = items.Split("/");


                            if (item[0].Contains("Player1: WON"))
                            {
                                totalWinRate++;
                                // deckIncrementCount++;
                                count++;
                            }
                            else
                            {
                                count++;
                                continue;
                            }
                        }
                        Console.Write("Count is: " + count + "\n");
                        tempdupsWinRate.Add(totalWinRate);
                        totalWinRate = 0;

                       

                    }

                    dupsWinRate.Add(tempdupsWinRate);
                }

                // Calculate the percentage
                for(int mainStruc = 0; mainStruc < 100 ; mainStruc++)
                {
                    int winRate = 0;
                    for(int subStruc = 0; subStruc < genFolderDups; subStruc++)
                    {
                        winRate += dupsWinRate[subStruc][mainStruc];
                    }
                    double result =  ((double)winRate/ (double)gamePerDeck)*100;
                    finalWinRate.Add(result );
                }
            }
            return finalWinRate;
        }

        /// <summary>
        /// This method will convert the Friendly Decks and their corresponding enemydeck into tuple
        /// </summary>
        /// <param name="friendDecksFinal"></param>
        /// <param name="enemyDecks"></param>
        /// <returns></returns>
        public Dictionary<List<int>, List<int>> convertToTuple(List<List<List<int>>> friendDecksFinal, List<List<int>> enemyDecks, int genFolderCount, int iterable)
        {
            Console.WriteLine("*************** Combine the friend deck with their respective enmy deck ******************");
            Dictionary<List<int>, List<int>> tuple = new Dictionary<List<int>, List<int>>();
            int flag = 0; int count = 0;

            foreach (List<List<int>> friendDeck in friendDecksFinal)
            {
                foreach (List<int> friendDeckTemp in friendDeck)
                {
                    if (count != 0 && count % iterable == 0 && count < (genFolderCount * iterable))
                    {
                        flag++;
                    }

                    if (flag == genFolderCount)
                    {
                        return tuple;
                    }
                    else
                    {
                        tuple.Add(friendDeckTemp, enemyDecks[flag]);
                    }

                    count++;
                }

            }

            return tuple;
        }


        public string convertToFinalStruct(Dictionary<List<int>, List<int>> tuple, List<double> playerStat, int genFolderCount, int iterable)
        {
            Console.WriteLine("*************** Combine the friend deck with their respective enemy deck and win and Lose stat ******************\n\n");
            string jsonStruct = "";

            List<dupStructure> cStruct = new List<dupStructure>();
            double[] pStat = playerStat.ToArray();

            int count = 0;
            foreach (var tunpleKey in tuple.Keys)
            {
                dupStructure cStructTemp = new dupStructure();

                if (count < (genFolderCount * iterable))
                {
                    cStructTemp.FriendDeck = tunpleKey;
                    cStructTemp.EnemyDeck = tuple.GetValueOrDefault(tunpleKey);
                    cStructTemp.WinRate = pStat[count];
                }

                cStruct.Add(cStructTemp);
                count++;
            }

            jsonStruct = JsonConvert.SerializeObject(cStruct);

            /* Save the JSON string in a FinalDataJSON.json */
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FinalData/FinalDataWinRateJSON.json";
            System.IO.File.WriteAllText(fileName, jsonStruct);

            return jsonStruct;
        }


        public void convertToCsv(string jsonContent)
        {
            Console.WriteLine("*************** Conver the JSON to CSV ******************\n\n");
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
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }
                }
            }

            /* Save the JSON string in a FinalDataJSON.json */
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FinalData/FinalDataWinRate.csv";
            System.IO.File.WriteAllText(fileName, csvString.ToString());

        }
    }
}
