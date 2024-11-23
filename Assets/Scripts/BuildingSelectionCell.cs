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
    private Building buildingPrefab;

    private void Start()
    {
        //Hud.Instance.AddBuilding(building,buildingCell.info);
        button.onClick.AddListener(() =>
        {
            var cell = Hud.Instance.AddBuilding(buildingPrefab,buildingCell.info);
            cell.OnPointerDown();
            // var building = Instantiate<GameObject>(buildingPrefab.gameObject);
            // var info = buildingCell.info;
            // building.GetComponent<Building>().identifier = info.identifier;
            // if (info.image != null && info.image != "")
            // {
            //     building.GetComponent<Building>().shape.sprite = buildingPrefab.shape.sprite;
            // }
            // PlayerControllerManager.Instance.StartDragging(building,gameObject);
            
            FindObjectOfType<SelectCardMenu>().Hide();
        });
    }

    public void UpdateCell(Building building,BuildingInfo info)
    {
        this.buildingPrefab = building;
        buildingCell.Init(building,info);
        descText.text = info.name + "\n" +info.synergyBK + "\n" +info.synergyElement + "\n" + info.description;
        
    }
}
