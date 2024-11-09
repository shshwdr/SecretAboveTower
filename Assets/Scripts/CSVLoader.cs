using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class BuildingInfo
{
    public string identifier;
    public string name;
    public bool isReady;
    public int rarity;
    public string synergyBK;
    public string synergyElement;
    public string prefab;
    public string description;
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
    public Dictionary<string, BuildingInfo> buildingInfoDict = new Dictionary<string, BuildingInfo>();
    public List<DeckInfo> deckInfos = new List<DeckInfo>();
    public Dictionary<string, TimerInfo> timerDict = new Dictionary<string, TimerInfo>();

    public void Init()
    {
        var buildingInfos =
            CsvUtil.LoadObjects<BuildingInfo>("building");
        foreach (var info in buildingInfos)
        {
            buildingInfoDict[info.identifier] = info;
        }
        // deckInfos = CsvUtil.LoadObjects<DeckInfo>("deck");
        var timerInfos = CsvUtil.LoadObjects<TimerInfo>("timer");
        foreach (var info in timerInfos)
        {
            timerDict[info.identifier] = info;
        }
    }
}