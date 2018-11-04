using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeartstoneDataCapture.Controllers
{
    class dataManipulatorController
    {
        /// <summary>
        /// This method will convert the get the list of decks for each gen folder
        /// </summary>
        /// <param name="genFolderCount"></param>
        /// <param name="iterable"></param>
        /// <returns></returns>
        public Dictionary<int , List<List<string>>>  populateDeckData(int genFolderCount ,  int iterable ) {

            Dictionary<int, List<List<string>>> deckDataDict = new Dictionary<int, List<List<string>>>();
            List<List<string>> deckData = null;

      
            Console.WriteLine("\n\n********************** Get the decks from text files ************************\n\n");
            for (int i = 0; i < genFolderCount; i++)
            {
                deckData = new List<List<string>>();
                Console.WriteLine("This is for Folder No: " + (i+1));
                for (int j = 0; j < iterable; j++)
                {
                    List<string> deckDataTemp = new List<string>();
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
                    foreach(string item in items)
                    {
                        if(count < 30)
                        {
                            deckDataTemp.Add(item);
                        }
                        count++;
                    }

                    deckData.Add(deckDataTemp);
                }

                Console.WriteLine("Total No of files scraped for Folder No: "+ (i+1) + " is: " + iterable + "\n");
                deckDataDict.Add(i, deckData);
            }

            return deckDataDict;
        }

        /// <summary>
        /// This method will duplicate the data and store them in a text file. It will be duplicated by the value iterable
        /// </summary>
        /// <param name="deckDataDict"></param>
        /// <param name="genFolderCount"></param>
        /// <param name="iterable"></param>
        public void storeDatatoTxt(Dictionary<int ,List<List<string>>> deckDataDict, int genFolderCount , int iterable) 
        {

            Console.WriteLine("********************** Store the data in to text file ************************\n\n");

            string fileName = "";
            for (int i = 0; i < genFolderCount; i++)
            {

                Console.WriteLine("This is for Folder No: " + (i + 1));

                List<List<string>> deckData = new List<List<string>>();
                deckData = deckDataDict.GetValueOrDefault(i);
                int count = 0;

                if (i == 0)
                {
                    fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen-" + i + "/decksNew.txt";
                }
                else
                {
                    fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen" + (i + 1) + "-0/decksNew.txt";
                }

                File.WriteAllText(fileName, String.Empty);

                for (int j = 0; j < iterable; j++)
                {

                    List<string> deckDataTemp = deckData[j];
                    string deckFormat = "";
                    foreach (string card in deckDataTemp)
                    {
                        deckFormat += card + "*";
                    }

                    
                    for (int k = 0; k < iterable; k++)
                    {
                        File.AppendAllText(fileName , count + "*" + deckFormat + "\n");
                        count++;
                    }
                    
                }
            }
        }

        /// <summary>
        /// This will create duplicate copies which will depend on value duplicates and store them duplicate folder
        /// </summary>
        /// <param name="genFolderCount"></param>
        /// <param name="duplicates"></param>
        public void makeDuplicates(int genFolderCount , int duplicates)
        {
            Console.WriteLine("********************** Duplicate the file in to Duplicates folder ************************\n\n");
            string fileName = "";
            string writeFileName = "";
            for(int i = 0; i < genFolderCount; i++)
            {
                if (i == 0) fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen-" + i + "/decksNew.txt";
                else fileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/FriendDecksData/gen" + (i + 1) + "-0/decksNew.txt";
                

                string text = System.IO.File.ReadAllText(fileName);

                for(int j = 0; j < duplicates; j++)
                {
                    if (i == 0)
                    {
                        writeFileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/Duplicates/Decks-gen-" + i + "-" + j + ".txt";
                    }
                    else
                    {
                        writeFileName = "C:/Development/HeartstoneDataCapture/HeartstoneDataCapture/Data/Duplicates/Decks-gen-" + (i+1) + "-" + j + ".txt";

                    }

                    File.WriteAllText(writeFileName, text);

                }
            }
            

        }
    }
}
