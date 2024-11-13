using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GridRow
{
    public List<int> selections; // 每行的选择状态

    public GridRow(int cols)
    {
        selections = new List<int>(new int[cols]); // 初始化为 0
    }
}
[System.Serializable]
public class Building : MonoBehaviour
{
    [HideInInspector]
    public string identifier;
    public BuildingInfo info=>CSVLoader.Instance.buildingInfoDict[identifier];
    public SpriteRenderer shape;
    //public SpriteRenderer shapeSupport;
   // public SpriteRenderer building;
    BuildingFunctionBase function;
    public List<Vector2Int> occupiedCells;

    public int rows = 3;

    public int cols = 3;
    public List<GridRow> selections;

    public void Trigger()
    {
        function?.Trigger();
    }
    
    private void Awake()
    {
        // 初始化选择状态
        //Init();
        if (function == null)
        {
            function = GetComponent<BuildingFunctionBase>();
        }
    }
    public void Init()
    {
        
        selections = new List<GridRow>(rows);
        for (int i = 0; i < rows; i++)
        {
            selections.Add(new GridRow(cols)); // 初始化每行
        }
    }

    public void SetRed()
    {
        SetColor(Color.red);
        
    }

    public void SetWhite()
    {
           SetColor(Color.white);
    }

    public void SetColor(Color color)
    {
        shape.color = color;
        //shapeSupport.color = color;
        //building.color = color;
        
    }
  
}
