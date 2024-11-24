using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffView : Singleton<BuffView>
{

    public List<BuffCell> cells;
    // Start is called before the first frame update
    void Start()
    {
        cells = GetComponentsInChildren<BuffCell>().ToList();
        UpdateView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateView()
    {
        foreach (var cell in cells)
        {
            cell.gameObject.SetActive(false);
        }

        int i = 0;
        var pairs = BuffManager.Instance.buffs.ToList();
        //sort pairs by value
        pairs = pairs.OrderByDescending(pair => pair.Value).ToList();
        foreach (var pair in pairs)
        {
            cells[i].symbol.sprite = SpriteUtils.GetBuffSprite(CSVLoader.Instance.buffInfoDict[pair.Key]);
            cells[i].gameObject.SetActive(true);
            cells[i].text.text =pair.Value.ToString();
            i++;
        }
    }
}
