using System;
using System.Collections.Generic;
using System.Text;

namespace HeartstoneDataCapture.Models
{
    class opponentModel
    {

        /// <summary>
        /// This variable will take the Name of deck
        /// </summary>
        public string deckName { get; set; }

        /// <summary>
        /// This variable will take the class the deck it belongs to
        /// </summary>
        public string className { get; set; }

        /// <summary>
        /// This variable will take the Info of the card and the count of that particular card in the model
        /// </summary>
        //public Dictionary<string, List<opponentCardModel>> cards { get; set; }
        public List<opponentCardModel> cards { get; set; }


    }
}
