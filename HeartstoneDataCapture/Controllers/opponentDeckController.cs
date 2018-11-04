using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HeartstoneDataCapture.Models;
using Newtonsoft.Json;

namespace HeartstoneDataCapture.Controllers
{
    class opponentDeckController
    {
        public static Dictionary<string,List<opponentModel>> opModel = new Dictionary<string, List<opponentModel>>();


        /// <summary>
        /// This method will convert deckInfo into the text file according to the format required
        /// E.g. Deck will represented like => NumDeck*Card*Card.....*Card
        /// </summary>
        /// <param name="deckInfo"></param>
        public void convertJSONToText(List<opponentModel> deckInfo)
        {
            Dictionary<string,string> deckText = new Dictionary<string, string>();
            int count = 0;

            foreach (opponentModel deck in deckInfo)
            {
                string text = "";
                count = count + 1;
                text += count + "*";


                foreach (opponentCardModel card in deck.cards)
                {
                    for (int i = 0; i < card.count; i++)
                    {
                        text += card.name + "*";
                    }
                }

                text = text.Substring(0, text.Length - 1);
                Console.WriteLine("The list of cards in deck : " + text);
                deckText.Add(deck.deckName, text);
            }

            /*********************** This section will save the content into TextFile ***********************/

            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FinalData/decks.txt";

            System.IO.File.WriteAllText(fileName, string.Empty);

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(fileName, true))
            {
                foreach (string deckVal in deckText.Values)
                {
                    file.WriteLine(deckVal);
                }
            }
        }


        /// <summary>
        /// This method will convert the JSON file into the desired deck Mode
        /// </summary>
        /// <returns>List of decks and the data associated with it</returns>
        public List<opponentModel> convertJSONToDeck()
        {
            string fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/CardDefs/decks.json";

            opModel = JsonConvert.DeserializeObject<Dictionary<string, List<opponentModel>>>(File.ReadAllText(fileName));

            Console.WriteLine("Hey the JSON file is converted into the opponentDeck model");

            List<opponentModel> opModelVal = opModel.GetValueOrDefault("opponentDecks");

            return opModelVal;
        }
    }
}
