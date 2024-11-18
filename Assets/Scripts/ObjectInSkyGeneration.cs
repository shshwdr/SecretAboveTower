using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectInSkyGeneration : Singleton<ObjectInSkyGeneration>
{

    public int startY = 1;

    public int splitEveryY = 3;

    public List<int> shownChanceList = new List<int>() { 0, 0, 0, 0, 0, 1, 2, 3, 4 };
    private List<int> chance;
    public void Init()
    {
        chance = shownChanceList.ToList();
        chance.Shuffle();
        int yNow = startY;
        for (int i = 0; i < 5; i++)
        {
            var x = Random.Range(0, CloudManagerNew.Instance.offsetX);
            var y1 = Random.Range(yNow, yNow + splitEveryY);
            var y2 = Random.Range(yNow, yNow + splitEveryY);
            var left = new Vector2Int(-x, y1);
            var right = new Vector2Int(x+1, y2);
            tryAddObject(left);
            tryAddObject(right);
            
            
            yNow += splitEveryY;
        }
    }

    void tryAddObject(Vector2Int pos)
    {
        // if (!GridManager.Instance.CanPlace(pos))
        // {
        //     return;
        // }
        
        if (chance.Count <= 0)
        {
            
            chance = shownChanceList.ToList();
            chance.Shuffle();
        }

        var r = chance[0];
        chance.RemoveAt(0);
        switch (r)
        {
            case 0:
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                var type = (SkyObjectType)(r-1);
                //get type string
                
                var go = Instantiate(Resources.Load<GameObject>("ObjectInSky/"+type.ToString()));
                GridManager.Instance.MarkCell(pos,go.GetComponent<SkyObject>());
                go.transform.position = GridManager.Instance.GridToWorldPositionWithHalf(pos);
                break;
        }
        
        
    }
}
