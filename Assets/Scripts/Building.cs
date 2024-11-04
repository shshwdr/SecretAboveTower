using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public SpriteRenderer shape;
    public SpriteRenderer shapeSupport;
    public SpriteRenderer building;

    public int rows = 3;

    public int cols = 3;

    public List<List<int>> selections;
    
    private void Awake()
    {
        // 初始化选择状态
        Init();
    }
    public void Init()
    {
        
        selections = new List<List<int>>(rows);
        for (int i = 0; i < rows; i++)
        {
            selections.Add(new List<int>(new int[cols]));
        }
    }
  
}
