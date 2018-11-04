using System;
using System.Collections.Generic;
using System.Text;

namespace HeartstoneDataCapture.Models
{
    class csvStructure
    {
        public List<int> FriendDeck { get; set; }
        public List<int> EnemyDeck { get; set; }
        public string WinnerDeck { get; set; }
    }
}
