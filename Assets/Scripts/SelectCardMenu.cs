using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectCardMenu : MenuBase
{
    public List<BuildingSelectionCell> buildingCells = new List<BuildingSelectionCell>();
    public Button refreshButton;
    public Button skipButton;

    public TMP_Text title;
    public TMP_Text desc;
    

    protected override void Start()
    {
        base.Start();
        //Show();
        // refreshButton.onClick.AddListener(() =>
        // {
        //     Refresh();
        // });
        // skipButton.onClick.AddListener(() =>
        // {
        //     Hide();
        // });
    }
    public  void Show(string tile , bool immediate = false)
    {
        this.title.text = tile;
        base.Show(immediate);
        Refresh();
        SFXManager.Instance.PlayCardSelection();
        
    }

    void Refresh()
    {
        var allCandidates = BuildingManager.Instance.GetAllDrawableBuildings();
        for (int i = 0; i < buildingCells.Count; i++)
        {
            var info = allCandidates.PickItem();
            var buildingCell = buildingCells[i];
            var building = Resources.Load < GameObject > ("BuildingShapes/"+info.prefab).GetComponent<Building>();
            buildingCell.UpdateCell(building,info);
        }
    }

}