using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessageCell : MonoBehaviour
{
    public TMP_Text messageText; // Text component to display the message

    private PopupMessageData message;
    public Image icon;

    // Setup the cell with a message and the click handler
    public void Setup(PopupMessageData message, System.Action<PopupMessageCell> onClick)
    {
        
        this.message = message;
        icon.sprite = SpriteUtils.GetPopupMessageSprite(message.messageType.ToString());
        messageText.text = message.messageType.ToString();

        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
            switch (message.messageType)
            {
                case PopupMessageType.SelectBuilding:
                    FindObjectOfType<SelectCardMenu>().Show(message.title);
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