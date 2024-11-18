using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectBuffMenu : MenuBase
{
    public TMP_Text title;

    public List<BuffSelectionCell> buildingCells = new List<BuffSelectionCell>();
    public void SetMilestoneTitle()
    {
        title.text = "You reached a milestone, Select a buff!";
    }
    
    public override void Show(bool immediate = false)
    {
        base.Show(immediate);
        Refresh();
        SFXManager.Instance.PlayBuffSelection();
    }

    void Refresh()
    {
        var allCandidates = BuffManager.Instance.GetAllDrawableBuffs();
        for (int i = 0; i < buildingCells.Count; i++)
        {
            var info = allCandidates.PickItem();
            var buildingCell = buildingCells[i];
            buildingCell.UpdateCell(info);
        }
    }

}
