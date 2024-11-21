using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessageView : Singleton<PopupMessageView>
{
    public GameObject messageCellPrefab; // Prefab for message cell
    public Transform messageContainer; // Container for holding message cells
    public float moveDuration = 0.5f; // Duration for the message cell to move

    private List<PopupMessageCell> activeCells = new List<PopupMessageCell>(); // Active message cells

    public void ShowMessage(PopupMessageData message)
    {
        // Create a new message cell
        GameObject cellObject = Instantiate(messageCellPrefab, messageContainer);
        PopupMessageCell messageCell = cellObject.GetComponent<PopupMessageCell>();
        messageCell.Setup(message, OnMessageCellClicked); // Setup the message cell

        // Set the position to the far right initially
        cellObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(1000, 0);
        
        // Start the animation to move it from right to left
        StartCoroutine(MoveCellToPosition(cellObject.GetComponent<RectTransform>(), new Vector2(100*(activeCells.Count ), 0 )));

        // Add to active cells list
        activeCells.Add(messageCell);
    }

    private IEnumerator MoveCellToPosition(RectTransform cell, Vector2 targetPosition)
    {
        Vector2 startPos = cell.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            cell.anchoredPosition = Vector2.Lerp(startPos, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it's exactly at the target position at the end
        cell.anchoredPosition = targetPosition;
    }

    private void OnMessageCellClicked(PopupMessageCell clickedCell)
    {
        activeCells.Remove(clickedCell);
        // Move all active cells forward by one position
        for (int i = 0; i < activeCells.Count; i++)
        {
            if (activeCells[i] != clickedCell)
            {
                // Move this cell forward (to a new position)
                StartCoroutine(MoveCellToPosition(activeCells[i].GetComponent<RectTransform>(), new Vector2(100*(i ), 0 )));
            }
        }

        // Move the clicked cell to the front (latest position)
        //StartCoroutine(MoveCellToPosition(clickedCell.GetComponent<RectTransform>(), new Vector2(0, 0)));
    }

    void UpdateAllPositions()
    {
        
    }
}
