using System;
using System.Collections.Generic;
using System.Text;

namespace HeartstoneDataCapture.Models
{
    class cardDefs
    {
        public string id { get; set; }
        public int dbfId { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string flavor { get; set; }
        public string artist { get; set; }
        public int attack { get; set; }
        public string cardClass { get; set; }
        public bool collectible { get; set; }
        public int cost { get; set; }
        public bool elite { get; set; }
        public string faction { get; set; }
        public int health { get; set; }
        public List<string> mechanics { get; set; }
        public string rarity { get; set; }
        public string set { get; set; }
        public string type { get; set; }


    }
}
