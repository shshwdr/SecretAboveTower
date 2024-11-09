using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectionCell : MonoBehaviour
{
    public BuildingCell buildingCell;
    public TMP_Text descText;
    public Button button;
    private Building building;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            Hud.Instance.AddBuilding(building);
            FindObjectOfType<SelectCardMenu>().Hide();
        });
    }

    public void UpdateCell(Building building)
    {
        this.building = building;
        buildingCell.Init((building));
        descText.text = building.info.name + "\n" +building.info.synergyBK + "\n" +building.info.synergyElement + "\n" + building.info.description;
        
    }
}
