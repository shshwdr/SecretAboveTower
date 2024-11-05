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
       var buildingCell = Instantiate(buildingCellPrefab, BottomSelectionView).GetComponent <BuildingCell>();
       var building = Resources.Load < GameObject > ("BuildingShapes/tree1").GetComponent<Building>();
       buildingCell.Init(building);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
