using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class CardInfo
{
    public string identifier;
    public string name;
    public bool isReady;
    public int unlockLevel;
    public List<string> tags;
    public string description;
    public List<string> action1;
    public List<string> action2;
    public List<string> action3;
    public List<string> action4;
    public List<string> action5;
    public List<string> action6;
    public List<string> action7;
    public List<string> action8;
    
    public List<List<string>> actions=>new List<List<string>>()
    {
        action1,action2,action3,action4,action5,action6,action7,action8
    };
    
    public int gold;
    public int cost;
    public int power;
    public int stay;
    public int medal;
    public bool isFemale;
}
public class TimerInfo
{
    public string identifier;
    public int time; //in seconds
    public List<int> values;
    public string type;
    public bool shouldSave;
    public bool shouldResetEveryday;
}

public  class DeckInfo
{
    public Dictionary<string,int> cards;
}

public class CSVLoader : Singleton<CSVLoader>
{
    public Dictionary<string, CardInfo> cardInfoDict = new Dictionary<string, CardInfo>();
    public List<DeckInfo> deckInfos = new List<DeckInfo>();
    public Dictionary<string, TimerInfo> timerDict = new Dictionary<string, TimerInfo>();

    public void Init()
    {
        // var cardInfos =
        //     CsvUtil.LoadObjects<CardInfo>("card");
        // foreach (var info in cardInfos)
        // {
        //     cardInfoDict[info.identifier] = info;
        // }
        // deckInfos = CsvUtil.LoadObjects<DeckInfo>("deck");
        var timerInfos = CsvUtil.LoadObjects<TimerInfo>("timer");
        foreach (var info in timerInfos)
        {
            timerDict[info.identifier] = info;
        }
    }
}