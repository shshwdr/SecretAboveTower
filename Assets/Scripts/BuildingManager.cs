using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager> // Start is called before the first frame update
{
    List< Building> buildings = new List<Building>();
    public Dictionary<string, List<Building>> synergyToBuildings = new Dictionary<string, List<Building>>();

    public List<Building> FindAllBuildingsWithSynergy(string synergy)
    {
        if (!synergyToBuildings.ContainsKey(synergy))
        {
            return new List<Building>();
        }
        return synergyToBuildings[synergy];
    }
    public void AddBuilding(Building building)
    {
        
        SFXManager.Instance.PlayBuilding();
        TimerManager.Instance.StartTimers();
        buildings.Add(building);

        var synergies = new List<string>() { building.info.synergyBK, building.info.synergyElement };
        foreach (var synergy in synergies)
        {
            
            if (synergyToBuildings.ContainsKey(synergy))
            {
                synergyToBuildings[synergy].Add(building);
            }
            else
            {
                synergyToBuildings[synergy] = new List<Building>();
                synergyToBuildings[synergy].Add(building);
            }
        }
        
        SynergyView.Instance.UpdateView();
        MilestoneManager.Instance.CheckMilestone(building.occupiedCells);

        if (building.info.synergyBK == "tree" &&  BuffManager.Instance.canGetAnotherBuilding())
        {
            PopupMessageManager.Instance.AddMessage(new PopupMessageData(){messageType= PopupMessageType.SelectBuilding,title = "Tree Synergy Works! You Get A New Building!"});
        }

        if (building.info.actions.Contains("payTimeIncrease"))
        {
            TimerManager.Instance.timers[TimerType.ConsumeResource].Duration += 1;
        }
    }

    public void TriggerAllBuildings()
    {
        ScoreRecordManager.Instance.ClearScore();
        foreach (var building in buildings)
        {
            building.Harvest();
        }
    }

    public List<BuildingInfo> GetAllDrawableBuildings()
    {
        List<BuildingInfo> res = new List<BuildingInfo>();
        foreach (var b in CSVLoader.Instance.buildingInfoDict.Values)
        {
            if (b.isReady)
            {
                res.Add(b);
            }
        }

        return res;
    }
}
