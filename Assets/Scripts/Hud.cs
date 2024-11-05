using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public Transform BottomSelectionView;

    public GameObject buildingCellPrefab;
    // Start is called before the first frame update
    void Start()
    {

        List<string> shapeNames = new List<string>() { "tree1", "tree2", "sail1","sail2" };
        foreach (var name in shapeNames)
        {
            var buildingCell = Instantiate(buildingCellPrefab, BottomSelectionView).GetComponent <BuildingCell>();
            var building = Resources.Load < GameObject > ("BuildingShapes/"+name).GetComponent<Building>();
            buildingCell.Init(building);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
