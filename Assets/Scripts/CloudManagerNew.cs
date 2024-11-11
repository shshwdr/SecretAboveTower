using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudManagerNew : Singleton<CloudManagerNew>
{
    public Sprite[] cloudSprites; // 将你的所有 sprites 加载到此数组中
    public GameObject tilePrefab;
    public Dictionary<Vector2Int, GameObject> tileMap = new Dictionary<Vector2Int, GameObject>();
    public int width, height;
    public int[,] cloudTiles;

    public void RemoveCloud(Vector2Int gridPosition)
    {
        if (tileMap.ContainsKey(gridPosition))
        {
            Destroy(tileMap[gridPosition].gameObject);
            tileMap.Remove((gridPosition));
        }
    }

    // 定义每种类型的数量
    private int[] typeCounts = {
        16, // Center
        8,  // Top Edge
        8,  // Left Edge
        8,  // Right Edge
        8,  // Bottom Edge
        8,  // Left and Right Edge
        8,  // Top and Bottom Edge
        8,  // All Edges
        8,  // Three Edges, Pointed Up
        8,  // Three Edges, Pointed Left
        8,  // Three Edges, Pointed Right
        8,  // Three Edges, Pointed Down
        4,  // Top Left Outer Corner
        4,  // Top Right Outer Corner
        4,  // Bottom Right Outer Corner
        4,  // Bottom Left Outer Corner
        4,  // Top Left Inner Corner
        4,  // Top Right Inner Corner
        4,  // Bottom Right Inner Corner
        4,  // Bottom Left Inner Corner
        8,  // Top Edge, Bottom Left Inner Corner
        8,  // Top Edge, Bottom Right Inner Corner
        8,  // Top Edge, Both Bottom Inner Corners
        8,  // Left Edge, Top Right Inner Corner
        8,  // Left Edge, Bottom Right Inner Corner
        8,  // Left Edge, Both Right Inner Corners
        8,  // Right Edge, Top Left Inner Corner
        8,  // Right Edge, Bottom Left Inner Corner
        8,  // Right Edge, Both Left Inner Corners
        8,  // Bottom Edge, Top Right Inner Corner
        8,  // Bottom Edge, Top Left Inner Corner
        8,  // Bottom Edge, Both Top Corners
        4,  // Top Left Outer Corner, Bottom Right Inner Corner
        4,  // Top Right Outer Corner, Bottom Left Inner Corner
        4,  // Bottom Right Outer Corner, Top Left Inner Corner
        4   // Bottom Left Outer Corner, Top Right Inner Corner
    };

    void Start()
    {
        GenerateCloudTiles();
        RenderCloudTiles();
    }

    void GenerateCloudTiles()
    {
        cloudTiles = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cloudTiles[x, y] = 0;
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                
                cloudTiles[x, y] = 1;
            }
        }
        // // 随机生成云朵
        for (int x = 0; x < width; x++)
        {
            for (int y = 2; y < height; y++)
            {
                cloudTiles[x, y] = Random.Range(0, 3)>0?1:0;
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
                    // int bitmask = GetBitmask(x, y);
                    // Sprite sprite = cloudSprites[GetSpriteIndex(bitmask)];
                    Sprite sprite = cloudSprites[DetermineSprite(x, y)];
                    CreateTile(x, y, sprite);
                }
            }
        }
    }

    int GetBitmask(int x, int y)
    {
        int bitmask = 0;
        if (IsCloud(x, y + 1)) bitmask |= 1;         // top
        if (IsCloud(x + 1, y + 1)) bitmask |= 2;     // topRight
        if (IsCloud(x + 1, y)) bitmask |= 4;         // right
        if (IsCloud(x + 1, y - 1)) bitmask |= 8;     // bottomRight
        if (IsCloud(x, y - 1)) bitmask |= 16;        // bottom
        if (IsCloud(x - 1, y - 1)) bitmask |= 32;    // bottomLeft
        if (IsCloud(x - 1, y)) bitmask |= 64;        // left
        if (IsCloud(x - 1, y + 1)) bitmask |= 128;   // topLeft
        return bitmask;
    }

    bool IsCloud(int x, int y)
    {
        // 边界视为无云
        if (x < 0 || x >= width || y < 0 || y >= height)
            return false;
        return cloudTiles[x, y] == 0;
    }

    int DetermineType(int x, int y)
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

        int typeIndex = 0;
        
        if (!top && bottom && !bottomRight && !bottomLeft && left && right) return 22;//Top Edge, Both Bottom Inner Corners
        
        if (!top && left && !bottomLeft && bottom && right) return 20; //Top Edge, Bottom Left Inner Corner
        if (!top && right && !topRight && bottom && left) return 21;//Top Edge, Bottom Right Inner Corner
        
        
        if (top && left && !topLeft && bottom && right) return 16; //Top Left Inner Corner
        if (top && right && !topRight && bottom && left) return 17;
        if (bottom && right && !bottomRight && top && left) return 18;//bottom right
        if (bottom && left && !bottomLeft && top && right) return 19;//bottom left
        
        
        if (!top && !left && bottom && !right) return 8; //Three Edges
        if (!top && right && !bottom && !left) return 9;
        if (!bottom && !right && !top && left) return 10;
        if (!bottom && !left && top && !right) return 11;
        
        
        if (bottom && !right && top && !left) return 5;
        if (!bottom && left && !top && right) return 6;
        
        
        if (!top && !left && bottom && right) return 12; //Top Left Outer
        if (!top && !right && bottom && left) return 13; //Top Right Outer
        if (!bottom && !right && top && left) return 14;
        if (!bottom && !left && top && right) return 15; 
        
        if (!top && !bottom && !left && !right) return 7; //All Edges 
        
        if (!top && bottom && left && right) return 1;//Top Edge
        if (top && bottom && !left && right) return 2;
        if (top && bottom && left && !right) return 3;
        if (top && !bottom && left && right) return 4;

        return typeIndex;
    }
    int DetermineSprite(int x, int y)
    {
        var typeIndex = DetermineType(x, y);
        
        int startIndex = 0;
        for (int i = 0; i < typeIndex; i++)
        {
            startIndex += typeCounts[i];
        }

        // 在类型范围内随机选择
        int randomIndex = Random.Range(0, typeCounts[typeIndex]);
        return startIndex + randomIndex;
        
    }

    int GetSpriteIndex(int bitmask)
    {
        int typeIndex = GetTypeIndex(bitmask);
        if (typeIndex == -1) return 0;

        // 计算起始索引
        int startIndex = 0;
        for (int i = 0; i < typeIndex; i++)
        {
            startIndex += typeCounts[i];
        }

        // 在类型范围内随机选择
        int randomIndex = Random.Range(0, typeCounts[typeIndex]);
        return startIndex + randomIndex;
    }
    public List<int> GetMatchingBitmasks(int mustBeOne, int mustBeZero)
    {
        List<int> matchingBitmasks = new List<int>();

        // 遍历所有可能的 8 位 bitmask (0 - 255)
        for (int bitmask = 0; bitmask < 256; bitmask++)
        {
            // 检查必须为1的位
            if ((bitmask & mustBeOne) != mustBeOne)
                continue;

            // 检查必须为0的位
            if ((bitmask & mustBeZero) != 0)
                continue;

            // 如果满足条件，则添加到结果列表
            matchingBitmasks.Add(bitmask);
        }

        return matchingBitmasks;
    }
    
    Dictionary<int,int> bitmaskToTypeIndex = new Dictionary<int,int>();
   int GetTypeIndex(int bitmask)
{
    if (bitmaskToTypeIndex.ContainsKey(bitmask))
    {
        return bitmaskToTypeIndex[bitmask];
    }
    
    
    
    // /*128  1   2
    //   64  x   4
    //   32  16  8*/
    //     // Center x16
    //     if(bitmask == 0b11111111)  return 0;
    //     
    //          
    //     
    //     if(bitmask == 0b00000000)  return 7; // All Edges
    //
    //     if (GetMatchingBitmasks(4 + 8 + 16, 1 + 64 + 128).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 12;
    //         return 12;// Top Left Outer Corner
    //     }
    //     if (GetMatchingBitmasks(16+32+64,1+2+4).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 13;
    //         return 13;// Top Right Outer Corner
    //     }
    //     if (GetMatchingBitmasks(1+128+64, 4+8+16).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 14;
    //         return 14;// Bottom Right Outer Corner
    //     }
    //     if (GetMatchingBitmasks(1+2+4, 16+32+64).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 15;
    //         return 15;// Bottom Left Outer Corner
    //     }
    //     
    //     
    //     if (GetMatchingBitmasks(16, 1+2+4+128+64).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 8;
    //         return bitmaskToTypeIndex[bitmask];// Three Edges, Pointed Top
    //     }
    //     
    //     if (GetMatchingBitmasks(4, 1+128+64+342+16).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 9;
    //         return bitmaskToTypeIndex[bitmask];// Three Edges, Pointed Left
    //     }
    //     
    //     if (GetMatchingBitmasks(64, 1+2+4+8+16).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 10;
    //         return bitmaskToTypeIndex[bitmask];// Three Edges, Pointed Right
    //     }
    //     
    //     if (GetMatchingBitmasks(1, 4+64+32+16+8).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 11;
    //         return bitmaskToTypeIndex[bitmask];// Three Edges, Pointed Down
    //     }
    //     
    //     
    //     if(bitmask == 0b01111111)  return 16; // Top Left Inner Corner
    //     if(bitmask == 0b00100000)  return 17; // Top Left Inner Corner
    //     if(bitmask == 0b00001000)  return 18; // Top Left Inner Corner
    //     if(bitmask == 0b00000010)  return 19; // Top Left Inner Corner
    //     
    //     
    //     if (GetMatchingBitmasks(16+64+4+8, 1+32).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 20;
    //         return bitmaskToTypeIndex[bitmask];// Top Edge with Bottom Left Inner Corner
    //     }
    //     if (GetMatchingBitmasks(4+16+32+64, 1+8).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 21;
    //         return bitmaskToTypeIndex[bitmask];// Top Edge with Bottom Right Inner Corner
    //     }
    //     
    //     if (GetMatchingBitmasks(1+16,4+64).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 5;
    //         return bitmaskToTypeIndex[bitmask];// Left and Right Edge
    //     }
    //     if (GetMatchingBitmasks(4+64,1+16).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 6;
    //         return bitmaskToTypeIndex[bitmask];// Left and Right Edge
    //     }
    //     
    //
    //     
    //     if (GetMatchingBitmasks(0,1).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 1;
    //         return bitmaskToTypeIndex[bitmask];// Top Edge
    //     }
    //     if (GetMatchingBitmasks(0,64).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 2;
    //         return bitmaskToTypeIndex[bitmask];// Left Edge
    //     }
    //     if (GetMatchingBitmasks(0,4).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 3;
    //         return bitmaskToTypeIndex[bitmask];// Top Edge
    //     }
    //     if (GetMatchingBitmasks(0,16).Contains(bitmask))
    //     {
    //         bitmaskToTypeIndex[bitmask] = 4;
    //         return bitmaskToTypeIndex[bitmask];// Top Edge
    //     }
    //     
    //     
    //     //
    //     // case 0b11001111: return 23; // Left Edge with Top Right Inner Corner
    //     // case 0b11111100: return 24; // Left Edge with Bottom Right Inner Corner
    //     // case 0b11011111: return 25; // Left Edge with Both Right Inner Corners
    //     //
    //     // case 0b00111111: return 26; // Right Edge with Top Left Inner Corner
    //     // case 0b01111110: return 27; // Right Edge with Bottom Left Inner Corner
    //     // case 0b01101111: return 28; // Right Edge with Both Left Inner Corners
    //     //
    //     // case 0b10011111: return 29; // Bottom Edge with Top Right Inner Corner
    //     // case 0b11011110: return 30; // Bottom Edge with Top Left Inner Corner
    //     // case 0b10111111: return 31; // Bottom Edge with Both Top Corners
    //     //
    //     // // Mixed Outer and Inner Corners
    //     // case 0b11010100: return 32; // Top Left Outer Corner, Bottom Right Inner Corner
    //     // case 0b00101110: return 33; // Top Right Outer Corner, Bottom Left Inner Corner
    //     // case 0b01100101: return 34; // Bottom Right Outer Corner, Top Left Inner Corner
    //     // case 0b10011001: return 35; // Bottom Left Outer Corner, Top Right Inner Corner

        return 0;
    
}


    void CreateTile(int x, int y, Sprite sprite)
    {
        GameObject tile = Instantiate(tilePrefab, new Vector3(x-8, y-2, 0), Quaternion.identity);
        tileMap[new Vector2Int(x, y)] = tile;
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
