using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingCell : MonoBehaviour, IPointerDownHandler
{
    public Image shape;
    public Image shapeSupport;
    public Image building;

    GameObject buildingPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Building building)
    {
        shape.sprite = building.shape.sprite;
        buildingPrefab = building.gameObject;
        //shapeSupport.sprite = building.shapeSupport.sprite;
        //building.building.sprite = building.building.sprite;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        
        var building = Instantiate(buildingPrefab);
        PlayerControllerManager.Instance.StartDragging(building);
    }
}
