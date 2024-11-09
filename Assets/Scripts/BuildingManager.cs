using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager> // Start is called before the first frame update
{
    List< Building> buildings = new List<Building>();

    public void AddBuilding(Building building)
    {
        
        TimerManager.Instance.StartTimers();
        buildings.Add(building);
    }

    public void TriggerAllBuildings()
    {
        foreach (var building in buildings)
        {
            building.Trigger();
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
