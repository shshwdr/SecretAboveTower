using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreRecordManager : Singleton<ScoreRecordManager>
{
    public Dictionary<Building, int> scoresInTotal = new Dictionary<Building, int>();
    public Dictionary<Building, int> scoresInRound = new Dictionary<Building, int>();

    public Transform parent;
    public GameObject prefab;

    public void UpdateView()
    {
        foreach (Transform cell in parent)
        {
            cell.gameObject.SetActive(false);
        }
        
        int i = 0;
        
        //sort scoresInRound based on value
        var sortedScores = scoresInRound.OrderByDescending(pair => pair.Value).ToList();
        
        foreach (var pair in sortedScores)
        {
            if (i >= parent.childCount)
            {
                Instantiate(prefab, parent);
            }
            var cell = parent.GetChild(i);
            cell.gameObject.SetActive(true);
            cell.GetComponent<ScoreCell>().icon.sprite =SpriteUtils.GetBuildingSprite(pair.Key.info);
            cell.GetComponent<ScoreCell>().scoreText.text = pair.Value.ToString();
            i++;
        }
        
    }

    public void ClearScore()
    {
        scoresInRound.Clear();
    }
    public void AddScore(Building building, int value)
    {
        if (scoresInTotal.ContainsKey(building))
        {
            scoresInTotal[building] += value;
        }
        else
        {
            scoresInTotal.Add(building, value);
        }
        if (scoresInRound.ContainsKey(building))
        {
            scoresInRound[building] += value;
        }
        else
        {
            scoresInRound.Add(building, value);
        }

        UpdateView();
    }
}
