using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager> // Start is called before the first frame update
{
    List< Building> buildings = new List<Building>();

    public void AddBuilding(Building building)
    {
        
        TimerManager.Instance.CheckAllTimerShouldStart();
        buildings.Add(building);
    }

    public void TriggerAllBuildings()
    {
        foreach (var building in buildings)
        {
            building.Trigger();
        }
    }
}
