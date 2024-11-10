using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public enum TileType {
    Middle,
    EdgeTop,
    EdgeBottom,
    EdgeLeft,
    EdgeRight,
    OuterCornerTopLeft,
    OuterCornerTopRight,
    OuterCornerBottomLeft,
    OuterCornerBottomRight,
    InnerCornerTopLeft,
    InnerCornerTopRight,
    InnerCornerBottomLeft,
    InnerCornerBottomRight
}

[System.Serializable]
public class TileSprites {
    public TileType type;
    public Sprite[] sprites; // 每种类型有多个随机 sprite
}
public class CloudManager : MonoBehaviour
{
    
   public TileSprites[] tileSprites;
    public GameObject tilePrefab; // 用于实例化 tile 的预制体
    public int width, height;
    public int[,] cloudTiles;

    private Dictionary<TileType, Sprite[]> tileDictionary;

    void Start()
    {
        InitializeTileDictionary();
        GenerateCloudTiles();
        RenderCloudTiles();
    }

    void InitializeTileDictionary()
    {
        tileDictionary = new Dictionary<TileType, Sprite[]>();
        foreach (var tile in tileSprites)
        {
            tileDictionary[tile.type] = tile.sprites;
        }
    }

    void GenerateCloudTiles()
    {
        cloudTiles = new int[width, height];
        // 随机生成云朵（可以根据需要修改）
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cloudTiles[x, y] = Random.Range(0, 2); // 0 或 1
            }
        }
    }

    void RenderCloudTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (cloudTiles[x, y] == 1)
                {
                    TileType type = DetermineTileType(x, y);
                    Sprite sprite = GetRandomSprite(type);
                    CreateTile(x, y, sprite);
                }
            }
        }
    }

    TileType DetermineTileType(int x, int y)
    {
        // 检查周围 8 个方向（超出边界视为无云）
        bool top = (y + 1 < height) && cloudTiles[x, y + 1] == 1;
        bool bottom = (y - 1 >= 0) && cloudTiles[x, y - 1] == 1;
        bool left = (x - 1 >= 0) && cloudTiles[x - 1, y] == 1;
        bool right = (x + 1 < width) && cloudTiles[x + 1, y] == 1;

        bool topLeft = (x - 1 >= 0 && y + 1 < height) && cloudTiles[x - 1, y + 1] == 1;
        bool topRight = (x + 1 < width && y + 1 < height) && cloudTiles[x + 1, y + 1] == 1;
        bool bottomLeft = (x - 1 >= 0 && y - 1 >= 0) && cloudTiles[x - 1, y - 1] == 1;
        bool bottomRight = (x + 1 < width && y - 1 >= 0) && cloudTiles[x + 1, y - 1] == 1;

// 优先检测 Inner Corners
        if (top && left && !topLeft) return TileType.InnerCornerTopLeft;
        if (top && right && !topRight) return TileType.InnerCornerTopRight;
        if (bottom && left && !bottomLeft) return TileType.InnerCornerBottomLeft;
        if (bottom && right && !bottomRight) return TileType.InnerCornerBottomRight;

// 检测 Outer Corners
        if (!top && !left && bottom && right) return TileType.OuterCornerTopLeft;
        if (!top && !right && bottom && left) return TileType.OuterCornerTopRight;
        if (!bottom && !left && top && right) return TileType.OuterCornerBottomLeft;
        if (!bottom && !right && top && left) return TileType.OuterCornerBottomRight;

// 检测边缘 tiles
        if (!top && bottom && left && right) return TileType.EdgeTop;
        if (top && !bottom && left && right) return TileType.EdgeBottom;
        if (top && bottom && !left && right) return TileType.EdgeLeft;
        if (top && bottom && left && !right) return TileType.EdgeRight;

// 检测中间
        if (top && bottom && left && right) return TileType.Middle;

        return TileType.Middle; // 默认情况


    }

    Sprite GetRandomSprite(TileType type)
    {
        Sprite[] sprites = tileDictionary[type];
        return sprites[Random.Range(0, sprites.Length)];
    }

    void CreateTile(int x, int y, Sprite sprite)
    {
        GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}