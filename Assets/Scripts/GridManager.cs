using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    private Dictionary<Vector2Int, bool> grid; // 用于存储网格占用状态
    public float cellSize = 1f; // 每个网格单元的大小
    public GameObject prefab; // 用于获取 prefab 的大小

    private void Start()
    {
        grid = new Dictionary<Vector2Int, bool>();
        
        // 根据 prefab 的大小设置 cellSize
        UpdateCellSize();
    }

    // 更新网格单元的大小，基于 prefab
    private void UpdateCellSize()
    {
        if (prefab != null)
        {
            // 获取 prefab 的大小
            SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                // 计算 cellSize，假设使用 sprite 的边界大小
                cellSize = renderer.bounds.size.x/3; // 这里选择 x 轴的大小，您可以根据需要调整
            }
        }
    }

    // 将特定位置标记为占用
    public void OccupyCell(Vector2 position)
    {
        Vector2Int cellIndex = WorldToGridPosition(position);
        if (!grid.ContainsKey(cellIndex))
        {
            grid[cellIndex] = true; // 标记为占用
        }
    }

    // 将特定位置标记为未占用
    public void FreeCell(Vector2 position)
    {
        Vector2Int cellIndex = WorldToGridPosition(position);
        if (grid.ContainsKey(cellIndex))
        {
            grid[cellIndex] = false; // 标记为未占用
        }
    }

    // 检查特定位置是否被占用
    public bool IsCellOccupied(Vector2 position)
    {
        Vector2Int cellIndex = WorldToGridPosition(position);
        return grid.ContainsKey(cellIndex) && grid[cellIndex];
    }

    // 将世界坐标转换为网格坐标
    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        // 将世界位置转换为网格坐标，向下取整
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition,GameObject buildingPrefab)
    {
        
        // 将网格坐标转换为世界坐标
        float x = gridPosition.x * cellSize + cellSize / 2; // 单元中心
        float y = gridPosition.y * cellSize + cellSize / 2; // 单元中心
        
        
        // 获取建筑的尺寸
        SpriteRenderer renderer = buildingPrefab.GetComponentInChildren<SpriteRenderer>();
        float width = renderer.bounds.size.x; // 建筑宽度
        float height = renderer.bounds.size.y; // 建筑高度
        x+= width/2;
        y+= height/2;
        
        return new Vector3(x, y, 0);
    }

    public Vector2Int GetBuildingOffset(GameObject buildingPrefab)
    {
        SpriteRenderer renderer = buildingPrefab.GetComponentInChildren<SpriteRenderer>();
        float width = renderer.bounds.size.x; // 建筑宽度
        float height = renderer.bounds.size.y; // 建筑高度
        
        Vector2Int gridPosition = WorldToGridPosition(new Vector2(width/2, height/2));
        return gridPosition;
    }

    public Vector2Int GetBuildingGridPosition(Vector3 mousePosition, GameObject buildingPrefab)
    {
        // 获取建筑的尺寸
        SpriteRenderer renderer = buildingPrefab.GetComponentInChildren<SpriteRenderer>();
        float width = renderer.bounds.size.x; // 建筑宽度
        float height = renderer.bounds.size.y; // 建筑高度
mousePosition.x-= width/2;
mousePosition.y-= height/2;
        // 计算网格坐标
        Vector2Int gridPosition = WorldToGridPosition(mousePosition);
    
        // // 计算偏移量：由于 pivot 在中心，需要减去一半的宽度和高度
        // int xOffset = Mathf.FloorToInt(width / (cellSize * 2)); // 水平偏移
        // int yOffset = Mathf.FloorToInt(height / (cellSize * 2)); // 垂直偏移
        //
        // // 确保 gridPosition 在合理范围内
        // int gridX = gridPosition.x - xOffset;
        // int gridY = gridPosition.y - yOffset;

        return new Vector2Int(gridPosition.x, gridPosition.y);
    } 

    bool CanPlace(Vector2Int checkPos)
    {
        if (checkPos.y < -3 || checkPos.x > 7 || checkPos.x < -9)
        {
            return false;
        }
        // 创建复选框
        if( grid.ContainsKey(checkPos))
        {
            return false;
        }

        return true;
    }
    public bool CanPlace(Building building,Vector2Int pos)
    {
        bool hasSupport = false;
        for (int j = 0; j < building.cols; j++) //每一个竖溜
        {
            bool checkedLowest = false;
        for (int i = 0; i < building.rows; i++)
        {
                if (building.selections[i].selections[j] == 1)
                {
                    if (!checkedLowest)
                    {
                        checkedLowest = true;
                        if (!CanPlace(new Vector2Int(j + pos.x, i + pos.y-1)))
                        {
                            hasSupport = true;
                        }
                    }
                    var checkPos= new Vector2Int(j + pos.x, i + pos.y);

                    if (!CanPlace(checkPos))
                    {
                        return false;
                    }
                }
            }
        }
        
        return hasSupport;
    }

    public void PlaceBuilding(Building building, Vector2Int gridPosition)
    {
        List<Vector2Int> occupiedCells = new List<Vector2Int>();
        //building.transform.position = GridToWorldPosition(gridPosition);
        for (int i = 0; i < building.rows; i++)
        {
            for (int j = 0; j < building.cols; j++)
            {
                // 创建复选框
                bool isSelected = building.selections[i].selections[j] == 1;
                if (isSelected)
                {
                    var pos = new Vector2Int(j + gridPosition.x, i + gridPosition.y);
                    occupiedCells.Add(pos);
                    OccupyCell((pos));
                }
            }
        }

        building.occupiedCells = occupiedCells;
        BuildingManager.Instance.AddBuilding(building);

        CloudManagerNew.Instance.RemoveCloud(building.occupiedCells);
    }
}
