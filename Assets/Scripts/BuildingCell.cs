using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCell : MonoBehaviour
{
    public Image shape;
    public Image shapeSupport;
    public Image building;
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
        shapeSupport.sprite = building.shapeSupport.sprite;
        //building.building.sprite = building.building.sprite;
    }
}
