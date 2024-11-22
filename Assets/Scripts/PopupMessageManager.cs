using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupMessageType{SelectBuilding, SelectBuff,Popup}

public class PopupMessageData
{
    public PopupMessageType messageType;
    public string title;
    public string desc;

}

public class PopupMessageManager : Singleton<PopupMessageManager>
{
    List<PopupMessageData> messageList;

    public void Init()
    {
        messageList = new List<PopupMessageData>();
        
    }

    private void Start()
    {
        AddMessage((new PopupMessageData(){messageType = PopupMessageType.SelectBuilding}));
    }

    public void AddMessage(PopupMessageData data)
    {
        messageList.Add(data);
        PopupMessageView.Instance.ShowMessage((data));
    }

    public void RemoveMessage(PopupMessageData data)
    {
        messageList.Remove(data);
    }
}
