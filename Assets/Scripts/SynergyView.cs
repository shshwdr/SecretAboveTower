using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SynergyView : Singleton<SynergyView>
{
    private List<int> upgradeRequired = new List<int>() {1,2,3,4 };

    public List<SynergyCell> cells;
    // Start is called before the first frame update
    void Start()
    {
        cells = GetComponentsInChildren<SynergyCell>().ToList();
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
        var pairs = BuildingManager.Instance.synergyToBuildings.ToList();
        //sort pairs by value
        pairs = pairs.OrderByDescending(pair => pair.Value.Count).ToList();
        SynergyManager.Instance.UpdateSynergy();
        foreach (var pair in pairs)
        {
            cells[i].symbol.sprite = SpriteUtils.GetSynergySprite(pair.Key);
            cells[i].gameObject.SetActive(true);
            cells[i].text.text ="lv:"+SynergyManager.Instance.GetLevel((pair.Key))+" "+ pair.Value.Count.ToString()+"/"+SynergyManager.Instance.GetNextLevel((pair.Key));
            i++;
        }
    }
}
