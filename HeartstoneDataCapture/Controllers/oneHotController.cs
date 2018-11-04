using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using CsvHelper;
using HeartstoneDataCapture.Models;
using HeartstoneDataCapture.Providers;
using Newtonsoft.Json;

namespace HeartstoneDataCapture.Controllers
{
    class oneHotController
    {

        cardInfoProvider cPro = new cardInfoProvider();


        /// <summary>
        /// Populates the CardPool dict with data
        /// </summary>
        public Dictionary<string,string> GetCardPoolData()
        {
            Dictionary<string, string> CardPool = new Dictionary<string, string>();
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/CardDefs/CardList53.txt";
            string lines = File.ReadAllText(fileName);
            string[] items = lines.Split('\n');

            int count = 1;
            foreach(string card in items)
            {
                CardPool.Add(card.Split('\r')[0], card.Split('\r')[0]);
                count++;
            }

            return CardPool;

        }


        /// <summary>
        /// This method will conver the JSON data  to one hot representation
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string convertJSONToOneHot1(String json, Dictionary<string, string> CardPool)
        {
            Dictionary<int , cardDefs> cardsInfo =  cPro.getAllCardsWithId();
            Dictionary<string, cardDefs> cardsInfoStr = cPro.getAllCards();
            List<csvStructure> data = JsonConvert.DeserializeObject<List<csvStructure>>(json);

            /***/
            List<int> CardPoolIdMain = new List<int>();
            foreach (string keys in CardPool.Keys)
            {
                CardPoolIdMain.Add(cardsInfoStr.GetValueOrDefault(keys.Split('\r')[0]).dbfId);
            }
            /***/

            List<csvStructure> finalOneHotStr = new List<csvStructure>();
            
            foreach (csvStructure struc in data)
            {
                List<int> FriendDeck = new List<int>();
                List<int> EnemyDeck = new List<int>();
       
                Dictionary<int, int> cardCountFriend = new Dictionary<int, int>();
                Dictionary<int, int> cardCountEnemy = new Dictionary<int, int>();
                List<int> CardPoolId = new List<int>();

                csvStructure cStructTemp = new csvStructure();

                foreach (string keys in CardPool.Keys)
                {
                    CardPoolId.Add(cardsInfoStr.GetValueOrDefault(keys.Split('\r')[0]).dbfId);
                }

                foreach( int id in struc.FriendDeck)
                {
                    string cardName = cardsInfo.GetValueOrDefault(id).name;
                    if(CardPool.ContainsKey(cardName))
                    {
                        if(cardCountFriend.ContainsKey(id))
                        {
                            cardCountFriend[id] = cardCountFriend[id] + 1;
                        }
                        else
                        {
                            cardCountFriend.Add(id, 1);
                        }
                        
                    }
                }

                foreach (int id in struc.EnemyDeck)
                {
                    string cardName = cardsInfo.GetValueOrDefault(id).name;
                    if (CardPool.ContainsKey(cardName))
                    {
                        if (cardCountEnemy.ContainsKey(id))
                        {
                            cardCountEnemy[id] = cardCountEnemy[id] + 1;
                        }
                        else
                        {
                            cardCountEnemy.Add(id, 1);
                        }

                    }
                }

                foreach(int id in CardPoolId)
                {
                    if(cardCountFriend.ContainsKey(id))
                    {
                        FriendDeck.Add(cardCountFriend[id]);
                    }
                    else
                    {
                        FriendDeck.Add(0);
                    }


                    if (cardCountEnemy.ContainsKey(id))
                    {
                        EnemyDeck.Add(cardCountEnemy[id]);
                    }
                    else
                    {
                        EnemyDeck.Add(0);
                    }

                }

                cStructTemp.FriendDeck = FriendDeck;
                cStructTemp.EnemyDeck = EnemyDeck;
                cStructTemp.WinnerDeck = struc.WinnerDeck;

                finalOneHotStr.Add(cStructTemp);
            }


            string result = JsonConvert.SerializeObject(finalOneHotStr);


            return result;    
        }


        public void convertToCsv(string jsonContent)
        {

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

            /* Save the JSON string in a FinalDataJSONOneHot.json */
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FinalData/FinalDaOneHot.csv";
            System.IO.File.WriteAllText(fileName, csvString.ToString());

        }

    }
}
