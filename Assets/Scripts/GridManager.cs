using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : Singleton<GridManager>
{
    private Dictionary<Vector2Int, bool> grid; // 用于存储网格占用状态
    private Dictionary<Vector2Int, SkyObject> gridToGo; // 用于存储网格占用状态
    public float cellSize = 1f; // 每个网格单元的大小
    public GameObject prefab; // 用于获取 prefab 的大小
    public GameObject destroyPrefab;
    public void Init()
    {
        grid = new Dictionary<Vector2Int, bool>();
        gridToGo = new Dictionary<Vector2Int, SkyObject>();

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
                cellSize = renderer.bounds.size.x / 3; // 这里选择 x 轴的大小，您可以根据需要调整
            }
        }
    }

    // 将特定位置标记为占用
    // ReSharper disable Unity.PerformanceAnalysis
    // public void OccupyCell(Vector2 position)
    // {
    //     Vector2Int cellIndex = WorldToGridPosition(position);
    //     if (!grid.ContainsKey(cellIndex))
    //     {
    //         grid[cellIndex] = true; // 标记为占用
    //         gridToGo[cellIndex] = prefab;
    //     }
    // }

    public void MarkCell(Vector2Int cellIndex, SkyObject go)
    {
        if (!grid.ContainsKey(cellIndex))
        {
            //grid[cellIndex] = true; // 标记为占用
            gridToGo[cellIndex] = go;
        }
    }

    public void OccupyCell(Vector2Int cellIndex)
    {
        if (!grid.ContainsKey(cellIndex))
        {
            grid[cellIndex] = true; // 标记为占用
            //gridToGo[cellIndex] = go;
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


    public Vector3 GridToWorldPositionWithHalf(Vector2Int gridPosition)
    {
        // 将网格坐标转换为世界坐标
        float x = gridPosition.x * cellSize + cellSize; // 单元中心
        float y = gridPosition.y * cellSize + cellSize; // 单元中心

        return new Vector3(x, y, 0);
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition, GameObject buildingPrefab)
    {
        // 将网格坐标转换为世界坐标
        float x = gridPosition.x * cellSize + cellSize / 2; // 单元中心
        float y = gridPosition.y * cellSize + cellSize / 2; // 单元中心


        // 获取建筑的尺寸
        SpriteRenderer renderer = buildingPrefab.GetComponentInChildren<SpriteRenderer>();
        float width = renderer.bounds.size.x; // 建筑宽度
        float height = renderer.bounds.size.y; // 建筑高度
        x += width / 2;
        y += height / 2;

        return new Vector3(x, y, 0);
    }

    public Vector2Int GetBuildingOffset(GameObject buildingPrefab)
    {
        SpriteRenderer renderer = buildingPrefab.GetComponentInChildren<SpriteRenderer>();
        float width = renderer.bounds.size.x; // 建筑宽度
        float height = renderer.bounds.size.y; // 建筑高度

        Vector2Int gridPosition = WorldToGridPosition(new Vector2(width / 2, height / 2));
        return gridPosition;
    }

    public Vector2Int GetBuildingGridPosition(Vector3 mousePosition, GameObject buildingPrefab)
    {
        // 获取建筑的尺寸
        SpriteRenderer renderer = buildingPrefab.GetComponentInChildren<SpriteRenderer>();
        float width = renderer.bounds.size.x; // 建筑宽度
        float height = renderer.bounds.size.y; // 建筑高度
        mousePosition.x -= width / 2;
        mousePosition.y -= height / 2;
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

    public bool CanPlace(Vector2Int checkPos)
    {
        if (checkPos.y < -3 || checkPos.x > 7 || checkPos.x < -9)
        {
            return false;
        }

        // 创建复选框
        if (grid.ContainsKey(checkPos))
        {
            return false;
        }

        return true;
    }
    
    

    public bool CanPlace(Building building, Vector2Int pos)
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
                        if (!CanPlace(new Vector2Int(j + pos.x, i + pos.y - 1)))
                        {
                            hasSupport = true;
                        }
                    }

                    var checkPos = new Vector2Int(j + pos.x, i + pos.y);

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
        bool willDestroy = false;
        List<GameObject> destroyGOs = new List<GameObject>();
        for (int i = 0; i < building.rows; i++)
        {
            for (int j = 0; j < building.cols; j++)
            {
                // 创建复选框
                bool isSelected = building.selections[i].selections[j] == 1;
                if (isSelected)
                {
                    var pos = new Vector2Int(j + gridPosition.x, i + gridPosition.y);
                    //如果已经被mark了

                    if (gridToGo.ContainsKey(pos))
                    {
                        switch (gridToGo[pos].type)
                        {
                            case SkyObjectType.castle:
                            case SkyObjectType.goodCastle:
                                FindObjectOfType<PopupMenu>().Show("Crash","During construction, people accidentally collided with the Sky Castle, leading to the destruction of both the building and the castle.");
                                destroyGOs.Add(gridToGo[pos].gameObject);
                                
                                gridToGo.Remove(pos);
                                willDestroy = true;
                                break;
                            case SkyObjectType.destroy:
                                FindObjectOfType<PopupMenu>().Show("BlackHole","A mysterious black hole appeared in the sky. Any building that came into contact with it vanished without a trace.");
                                willDestroy = true;
                                break;
                            case SkyObjectType.debuff:
                                building.AddEffect("debuff");
                                FindObjectOfType<PopupMenu>().Show("It's a grim..","Unidentified flying objects hovered around a building, permanently lowering its happiness score.");
                                //destroyGOs.Add(gridToGo[pos].gameObject);
                                gridToGo.Remove(pos);
                                break;
                            case SkyObjectType.rainbow:
                                building.AddEffect("rainbow");
                                FindObjectOfType<PopupMenu>().Show("Look! A rainbow!","Everyone who sees the rainbow feels happiness.This building's happiness score was permanently increased.");
                                //destroyGOs.Add(gridToGo[pos].gameObject);
                                
                                gridToGo.Remove(pos);
                                break;
                        }
                    }

                    // occupiedCells.Add(pos);
                    // OccupyCell(pos, building.gameObject);
                }
            }
        }

        foreach (var go in destroyGOs)
        {
            DestoryTile(go.transform.position);
            Destroy((go.gameObject));
        }


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
                }
            }
        }
        if (willDestroy)
        {
            DestroyBuilding(occupiedCells);
            Destroy((building.gameObject));
            return;
        }

        foreach (var pos in occupiedCells)
        {
            
            OccupyCell(pos);
        }
        

        building.occupiedCells = occupiedCells;
        BuildingManager.Instance.AddBuilding(building);

        CloudManagerNew.Instance.RemoveCloud(AdjacentTiles(building));
        
        //检查是否和castle相连

        foreach (var tile in AdjacentTiles(building))
        {
            if (gridToGo.ContainsKey(tile) && gridToGo[tile] .used == false)
            {
                if (gridToGo[tile].type == SkyObjectType.castle)
                {
                    gridToGo[tile].used = true;
                    FindObjectOfType<SelectBuffMenu>().Show("People encountered the legendary Sky Castle and received its blessings.");
                    OccupyCell(tile);
                    
                    var go = Instantiate(Resources.Load<GameObject>("ObjectInSky/goodCastle"));
                    go.transform.position = gridToGo[tile].transform.position;
                }

                // if (gridToGo[tile].type == SkyObjectType.goodCastle)
                // {
                //     gridToGo[tile].used = true;
                //     FindObjectOfType<SelectBuffMenu>().Show();
                //     OccupyCell(tile);
                // }
            }
        }
    }

    public void DestoryTile(Vector3 position)
    {
        var go =  Instantiate(destroyPrefab);
        go.transform.position = position;
        
        Destroy(go,0.33f);
    }

    public void DestroyBuilding(List<Vector2Int> building)
    {
        foreach (var cell in building)
        {
            DestoryTile(GridToWorldPosition(cell));
        }
    }
    

    public static List<Vector2Int> AdjacentTiles(Vector2Int pos, int radius =1)
    {
        
        List<Vector2Int> tiles = new List<Vector2Int>();

        // 遍历所有可能的格子
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                // 计算当前格子的相对位置
                Vector2Int newPos = new Vector2Int(pos.x + x, pos.y + y);

                // 使用曼哈顿距离判断是否在 radius 范围内
                if (Mathf.Abs(x) + Mathf.Abs(y) <= radius)
                {
                    tiles.Add(newPos);
                }
            }
        }

        return tiles;
    }
    public List<Vector2Int> AdjacentTiles(Building building,int distance = 1)
    {
        var res = new List<Vector2Int>();
        foreach (var pos in building.occupiedCells)
        {
            res.AddRange((AdjacentTiles(pos,distance)));
        }
        //remove duplicate in res
        res =  res.Distinct().ToList();

        return res;
    }
}