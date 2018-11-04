using HeartstoneDataCapture.Controllers;
using HeartstoneDataCapture.Providers;
using HeartstoneDataCapture.Models;
using System.Collections.Generic;
using System;

namespace HeartstoneDataCapture
{
    /// <summary>
    /// This program is responsible to create random decks for a particular class 
    /// </summary>
    /// <remarks>
    /// This program can generate n unique decks where n is the number of unique decks required.
    /// </remarks>
    class Program
    {

        static void Main(string[] args)
        {
            int genFolderCount = 10;
            int iterable = 100;

            opponentDeckController opController = new opponentDeckController();
            texttocsvController ttc = new texttocsvController();
            texttocsvCostController ttcc = new texttocsvCostController();
            oneHotController onc = new oneHotController();
            cardInfoProvider cPro = new cardInfoProvider();
            dataManipulatorController dataManipulator = new dataManipulatorController();

            /**Get the data of all the cards**/
            Dictionary<int, cardDefs> cards = cPro.getAllCardsWithId();

            /******************************This section is used to convert JSON to Txt*********************************/
            List<opponentModel> opModel = opController.convertJSONToDeck();
            opController.convertJSONToText(opModel);

            /******************************This section is used to convert Txt to Csv*********************************/
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("                          TXT to CSV  Controller                                   ");
            Console.WriteLine("***********************************************************************************\n");

            List<List<List<int>>> friendDeck =   ttc.scrapeFriendFromText(genFolderCount , iterable);
            List<List<int>> enemyDeck = ttc.scrapeEnemyFromText(genFolderCount);
            Dictionary<List<int>, List<int>> tuple = ttc.convertToTuple(friendDeck , enemyDeck , genFolderCount , iterable);
            List<string> playerStat = ttc.getPlayerResult(genFolderCount , iterable);

            // Convert the data into JSON Format    
            string finalJSON = ttc.convertToFinalStruct(tuple , playerStat , genFolderCount , iterable);

            // Convert the JSON string to CSV file
            ttc.convertToCsv(finalJSON);

            /******************************This section is used to convert Txt to Csv (with Cost)*********************************/
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("                          TEXT to CSV (with Cost)  Controller                      ");
            Console.WriteLine("***********************************************************************************\n");

            List<List<List<friendDeckModel>>> friendDeckCost =   ttcc.scrapeFriendFromText(genFolderCount , iterable);
            List<List<friendDeckModel>> enemyDeckCost = ttcc.scrapeEnemyFromText(genFolderCount,iterable);
            Dictionary<List<friendDeckModel>, List<friendDeckModel>> tupleCost = ttcc.convertToTuple(friendDeckCost , enemyDeckCost, genFolderCount , iterable);
            List<string> playerStatCost = ttcc.getPlayerResult(genFolderCount , iterable);

            // Convert the data into JSON Format    
            string finalJSONCost = ttcc.convertToFinalStruct(tupleCost , playerStatCost , genFolderCount , iterable);


            // Convert the JSON string to CSV file
            //ttcc.convertToCsv(finalJSONCost);

            /****************************This section will convert the data to OneHot Representation****************************/
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("                          One Hot Converter Controller                             ");
            Console.WriteLine("***********************************************************************************\n");

            Dictionary<string, string> CardPool = onc.GetCardPoolData();
            string oneHotStr = onc.convertJSONToOneHot1(finalJSON , CardPool);
            onc.convertToCsv(oneHotStr);
            //convertJSONToOneHot2(finalJSONCost);

            /****************************This section will add duplicate the data and store them in a text file ****************************/
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("                          Duplicate Data  Controller                               ");
            Console.WriteLine("***********************************************************************************\n");


            Dictionary<int, List<List<string>>> deckDataDict = dataManipulator.populateDeckData(genFolderCount , iterable);
            dataManipulator.storeDatatoTxt(deckDataDict, genFolderCount, iterable);
            dataManipulator.makeDuplicates(genFolderCount , 4);

        }
    }
}
