using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : Singleton<MilestoneManager>
{
    public GameObject milstoneGO;

    private int currentMilestoneIndex = 0;

    private int currentMaxHeight = -999;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateMilestone();
    }

    void UpdateMilestone()
    {
        if (currentMilestoneIndex >= CSVLoader.Instance.milestones.Count)
        {
            milstoneGO.SetActive(false);
            return;
        }

        var milestoneInfo = CSVLoader.Instance.milestones[currentMilestoneIndex];
        milstoneGO.transform.position = GridManager.Instance.GridToWorldPosition(new Vector2Int(0,milestoneInfo.distance))+new Vector3(0,0.3f,0);
    }

    int currentMilestoneHeight()
    {
        if (currentMilestoneIndex >= CSVLoader.Instance.milestones.Count)
        {
            milstoneGO.SetActive(false);
            return int.MaxValue;
        }

        return CSVLoader.Instance.milestones[currentMilestoneIndex].distance;
    }

    public void CheckMilestone(List<Vector2Int> gridPositions)
    {
        foreach (var position in gridPositions)
        {
            if (position.y >= currentMilestoneHeight())
            {
                AddMilestone();
            }

            if (position.y > currentMaxHeight)
            {
                currentMaxHeight = position.y;
            }
        }
    }

    public void AddMilestone()
    {
        currentMilestoneIndex++;
        UpdateMilestone();
        
        PopupMessageManager.Instance.AddMessage(new PopupMessageData(){messageType= PopupMessageType.SelectBuff,title = "You reached a milestone, Select a buff!"});
        
        //FindObjectOfType<SelectBuffMenu>().SetMilestoneTitle();
        //FindObjectOfType<SelectBuffMenu>().Show();
    }
}
