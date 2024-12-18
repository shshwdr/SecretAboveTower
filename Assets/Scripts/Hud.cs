using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : Singleton<Hud>
{
    public Transform BottomSelectionView;

    public GameObject buildingCellPrefab;

    public TMP_Text gold;

    public TMP_Text time;

    public ProgressBar nextBuildingProgress;

    public ProgressBar nextPayProgress;
    public ProgressBar nextHarvestProgress;

    private int nextPayValue =0;

    private int nextHarvestValue = 0;

    public void UpdateNextHarvestValue()
    {
        
    }
    //public Transform synergiesParent;
    // Start is called before the first frame update
    void Start()
    {

        List<string> shapeNames = new List<string>() { "foodTree", "hovercraft", "lightTree" };
        foreach (var name in shapeNames)
        {
            var buildingCell = Instantiate(buildingCellPrefab, BottomSelectionView).GetComponent <BuildingCell>();
            var building = Resources.Load < GameObject > ("BuildingShapes/"+name).GetComponent<Building>();
            buildingCell.Init(building, CSVLoader.Instance.buildingInfoDict[name]);
        }
        EventPool.OptIn("DrawCardTimerTick",updateResources);
        nextBuildingProgress.SetMaxValue((int)TimerManager.Instance.timers[TimerType.DrawCard].Duration);

        nextHarvestProgress.SetMaxValue((int)TimerManager.Instance.timers[TimerType.TriggerBuilding].Duration);
        nextPayProgress.SetMaxValue((int)TimerManager.Instance.timers[TimerType.ConsumeResource].Duration);
    }

    public BuildingCell AddBuilding(Building building,BuildingInfo info)
    {
        var buildingCell = Instantiate(buildingCellPrefab, BottomSelectionView).GetComponent <BuildingCell>();
        buildingCell.Init(building,info);
        return buildingCell;
    }

    private void updateResources()
    {
        gold.text = "Gold: "+ResourceManager.Instance.GetGold().ToString();
        time.text = "Time: "+TimerManager.Instance.PlayedTime.ToString();
        nextBuildingProgress.SetProgress(TimerManager.Instance.timers[TimerType.DrawCard].TimeRemaining);
        
        nextHarvestProgress.SetProgress(TimerManager.Instance.timers[TimerType.TriggerBuilding].TimeRemaining);
        nextPayProgress.SetProgress(TimerManager.Instance.timers[TimerType.ConsumeResource].TimeRemaining);
    }
    
}
