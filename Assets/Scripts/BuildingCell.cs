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
    public bool isDraggable = true;
    public BuildingInfo info;
    GameObject buildingPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Building building,BuildingInfo info)
    {
        shape.sprite = building.shape.sprite;
        this.info = info;
        buildingPrefab = building.gameObject;
        if (info.image != null && info.image != "")
        {
            shape.sprite = Resources.Load<Sprite>("BuildingsSprite/"+info.image);
        }
        //shapeSupport.sprite = building.shapeSupport.sprite;
        //building.building.sprite = building.building.sprite;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        
        var building = Instantiate(buildingPrefab);
        building.GetComponent<Building>().identifier = info.identifier;
        if (info.image != null && info.image != "")
        {
            building.GetComponent<Building>().shape.sprite = shape.sprite;
        }
        PlayerControllerManager.Instance.StartDragging(building,gameObject);
    }

    public void OnPointerDown()
    {
        
        
        var building = Instantiate(buildingPrefab);
        building.GetComponent<Building>().identifier = info.identifier;
        if (info.image != null && info.image != "")
        {
            building.GetComponent<Building>().shape.sprite = shape.sprite;
        }
        PlayerControllerManager.Instance.StartDragging(building,gameObject);
    }
}
