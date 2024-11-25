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
public class Building : MonoBehaviour,IHoverable
{
    private BoxCollider2D collider;
    public List<string> effects = new List < string > ();
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

    public void AddEffect(string effect)
    {
        
        effects.Add(effect);
    }

    public int HarvestValue()
    {
        //function?.Trigger();
        var happiness = info.baseHappy;

        int value = 0;
        if (info.actions.Count > 1)
        {

            int.TryParse(info.actions[1], out value);
        }

        switch (info.actions[0])
        {
            case "nextToAny":
                var adjacentBuildings = GridManager.Instance.AdjacentBuildings(this);
                happiness += value * adjacentBuildings.Count;
                break;
            
            case "finishedMilestone":
                var finishedMilestone = MilestoneManager.Instance.FinishedMilestone;
                happiness += value * finishedMilestone;
                break;
            case "randomNeighbourHarvest":
                adjacentBuildings = GridManager.Instance.AdjacentBuildings(this);
                List<Building> candidates = new List<Building>();
                foreach (var building in adjacentBuildings)
                {
                    if (building.info.identifier != info.identifier)
                    {
                        candidates.Add(building);
                    }
                }

                if (candidates.Count > 0)
                {
                    happiness += candidates.RandomItem().HarvestValue();
                }
                break;
            case "constructionCount":
                happiness += BuildingManager.Instance.FindAllBuildingsWithSynergy("construction").Count * value;
                break;
        }

        return happiness;
    }
    
    public void Harvest()
    {
        var happiness = HarvestValue();
        
        ResourceManager.Instance.AddGold(transform,happiness);
        ScoreRecordManager.Instance.AddScore(this, happiness);
    }
    
    private void Awake()
    {
        // 初始化选择状态
        //Init();
        if (function == null)
        {
            function = GetComponent<BuildingFunctionBase>();
        }
        ResetCollider();
    }
    
    void ResetCollider()
    {
        // 获取 SpriteRenderer 和 BoxCollider2D 组件
        BoxCollider2D boxCollider = GetComponentInChildren<BoxCollider2D>();
        SpriteRenderer spriteRenderer = boxCollider.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && boxCollider != null)
        {
            // 获取 Sprite 的边界大小
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

            // 设置 BoxCollider2D 的尺寸，使其匹配 Sprite 的大小
            boxCollider.size = spriteSize;
            boxCollider.offset = spriteRenderer.sprite.bounds.center - transform.position; // 调整偏移，使其正确匹配
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

    public void Hover()
    {
        HoverOverMenu.FindFirstInstance<HoverOverMenu>().Show(info.name,info.description);
    }
}
