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
    public string image;
    public int baseHappy;
    public List<string> actions;

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

public class BuffInfo
{
    public string type;
    public string subType;
    public string identifier => type + subType;
    public string desc;
    public bool canDraw;
    public string icon;
    public List<int> values;
}

public class MilestoneInfo
{
    public int distance;
}

public class SkyObjectInfo
{
    public string identifier;
    public string desc;
}
public  class DeckInfo
{
    public Dictionary<string,int> cards;
}

public class SynergyInfo
{
public  string identifier;
public string name;
public string description;
public List<int> values;
public string buff;
}

public class CSVLoader : Singleton<CSVLoader>
{
    public Dictionary<string, BuildingInfo> buildingInfoDict = new Dictionary<string, BuildingInfo>();
    public List<DeckInfo> deckInfos = new List<DeckInfo>();
    public Dictionary<string, TimerInfo> timerDict = new Dictionary<string, TimerInfo>();
    public Dictionary<string, BuffInfo> buffInfoDict = new Dictionary<string, BuffInfo>();
    public Dictionary<string, SkyObjectInfo> skyObjectInfoDict = new Dictionary<string, SkyObjectInfo>();
    public Dictionary<string, SynergyInfo> synergyInfoDict = new Dictionary<string, SynergyInfo>();
public List<MilestoneInfo> milestones;
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
        milestones = CsvUtil.LoadObjects<MilestoneInfo>("milestone");
        var buffInfos = CsvUtil.LoadObjects<BuffInfo>("buff");
        foreach (var info in buffInfos)
        {
            buffInfoDict[info.identifier] = info;
        }
        var skyObjectInfos = CsvUtil.LoadObjects<SkyObjectInfo>("skyObject");
        foreach (var info in skyObjectInfos)
        {
            skyObjectInfoDict[info.identifier] = info;
        }
        var synergyInfos = CsvUtil.LoadObjects<SynergyInfo>("synergy");
        foreach (var info in synergyInfos)
        {
            synergyInfoDict[info.identifier] = info;
        }
    }
}