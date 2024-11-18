using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SynergyData
{
    
}
public class SynergyManager : Singleton<SynergyManager>
{
    List<int> amountToUpgrade =  new List<int>() { 1,2,3,4};
    Dictionary<string,int> synergyLevels = new Dictionary<string, int>();
    Dictionary<string,string> synergyNextLevels = new Dictionary<string, string>();
    public void UpdateSynergy()
    {
        
        var pairs = BuildingManager.Instance.synergyToBuildings.ToList();
        //sort pairs by value
        pairs = pairs.OrderByDescending(pair => pair.Value.Count).ToList();
        foreach (var pair in pairs)
        {
            synergyLevels[pair.Key] = level(pair.Key,pair.Value.Count);
        }
    }

    public int GetLevel(string synergy)
    {
        return synergyLevels.GetValueOrDefault(synergy, 0);
    }

    public string GetNextLevel(string synergy)
    {
        return synergyNextLevels.GetValueOrDefault(synergy, amountToUpgrade[0].ToString());
    }
    
    int level(string synergy, int value)
    {
        var level = 0;
        for (int i = 0; i < amountToUpgrade.Count; i++)
        {
            if (value >= amountToUpgrade[i])
            {
                level++;
            }
            else
            {
                break;
            }
        }

        if (level >= amountToUpgrade.Count)
        {
            synergyNextLevels[synergy] = "MAX";
        }

        synergyNextLevels[synergy] = amountToUpgrade[level].ToString();
        
        return level;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
