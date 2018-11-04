using System;
using System.Collections.Generic;
using System.Text;
using HeartstoneDataCapture.Models;

namespace HeartstoneDataCapture.Models
{
    class csvStructureCost
    {
        public List<friendDeckModel> FriendDeck { get; set; }
        public List<friendDeckModel> EnemyDeck { get; set; }
        public string WinnerDeck { get; set; }
    }
}
