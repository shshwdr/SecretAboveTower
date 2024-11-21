using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessageCell : MonoBehaviour
{
    public TMP_Text messageText; // Text component to display the message

    private PopupMessageData message;

    // Setup the cell with a message and the click handler
    public void Setup(PopupMessageData message, System.Action<PopupMessageCell> onClick)
    {
        this.message = message;
        messageText.text = message.messageType.ToString();

        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
            switch (message.messageType)
            {
                case PopupMessageType.SelectBuilding:
                    FindObjectOfType<SelectCardMenu>().Show();
                    break;
                case PopupMessageType.SelectBuff:
                    FindObjectOfType<SelectBuffMenu>().Show(message.title);
                    break;
                case PopupMessageType.Popup:
                    FindObjectOfType<PopupMenu>().Show(message.title, message.desc);
                    break;
            }
            onClick(this);
        });
    }
}