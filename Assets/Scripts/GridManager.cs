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

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        // 将网格坐标转换为世界坐标
        float x = gridPosition.x * cellSize + cellSize / 2; // 单元中心
        float y = gridPosition.y * cellSize + cellSize / 2; // 单元中心
        return new Vector3(x, y, 0);
    }
    public bool CanPlace(Building building)
    {
        return true;
    }
}
