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
            Hud.Instance.AddBuilding(building,buildingCell.info);
            FindObjectOfType<SelectCardMenu>().Hide();
        });
    }

    public void UpdateCell(Building building,BuildingInfo info)
    {
        this.building = building;
        buildingCell.Init(building,info);
        descText.text = info.name + "\n" +info.synergyBK + "\n" +info.synergyElement + "\n" + info.description;
        
    }
}
