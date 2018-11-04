using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HeartstoneDataCapture.Models;
using Newtonsoft.Json;  

namespace HeartstoneDataCapture.Providers
{
    class cardInfoProvider
    {

        public Dictionary<string , cardDefs > getAllCards()
        {
            Dictionary<string, cardDefs> cards = new Dictionary<string, cardDefs>();
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/CardDefs/CardDefs.json";
            string lines = File.ReadAllText(fileName);

            List<cardDefs> cardsInfo = JsonConvert.DeserializeObject<List<cardDefs>>(lines);
            int count = 1;

            foreach (cardDefs cardInfo in cardsInfo)
            {
                if(cards.GetValueOrDefault(cardInfo.name) == null && cardInfo.id.Equals("PlaceholderCard") == false)
                {
                    cards.Add(cardInfo.name, cardInfo);
                    count++;
                }
                else
                {
                    continue;
                }
            }

            return cards;
        }


        public Dictionary<int, cardDefs> getAllCardsWithId()
        {
            Dictionary<int, cardDefs> cards = new Dictionary<int, cardDefs>();
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/CardDefs/CardDefs.json";
            string lines = File.ReadAllText(fileName);

            List<cardDefs> cardsInfo = JsonConvert.DeserializeObject<List<cardDefs>>(lines);
            int count = 1;

            foreach (cardDefs cardInfo in cardsInfo)
            {
                if (cards.GetValueOrDefault(cardInfo.dbfId) == null && cardInfo.id.Equals("PlaceholderCard") == false)
                {
                    cards.Add(cardInfo.dbfId, cardInfo);
                    count++;
                }
                else
                {
                    continue;
                }
            }

            return cards;
        }

    }
}
