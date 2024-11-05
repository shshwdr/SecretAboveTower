using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : Singleton<PlayerControllerManager>
{
    private GameObject currentBuilding; // 当前正在拖动的 Building
    public Transform uiDropArea; // 取消UI 放置区域

    public void StartDragging(GameObject building)
    {
        currentBuilding = building;
    }
    
    public void Update()
    {
        // 检查当前是否有 Building 在拖动
        if (currentBuilding != null)
        {
            DragBuilding();
        }
    }
    private void DragBuilding()
    {
        // 获取鼠标位置并转换为世界坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // 确保 z 轴为 0

        
        // 自动对齐到网格位置
        // Vector2Int gridPosition = GridManager.Instance.WorldToGridPosition(mousePosition);
        // Vector3 snapPosition =  GridManager.Instance.GridToWorldPosition(gridPosition);
        Vector2Int buildingGridPosition = GridManager.Instance.GetBuildingGridPosition(mousePosition, currentBuilding);
        Vector3 snapPosition = GridManager.Instance.GridToWorldPosition(buildingGridPosition);

        // 更新当前 Building 的位置
        currentBuilding.transform.position = snapPosition;
        // 更新当前 Building 的位置
       // currentBuilding.transform.position = mousePosition;

        // 检查网格状态
        bool canPlace = GridManager.Instance.CanPlace(currentBuilding.GetComponent<Building>(),buildingGridPosition);

        if (canPlace)
        {
            currentBuilding.GetComponent<Building>().SetWhite();
        }
        else
        {
            currentBuilding.GetComponent<Building>().SetRed();
        }

        // 检查放置
        if (Input.GetMouseButtonUp(0)) // 左键放下
        {
            if (IsInDropArea(mousePosition) || !canPlace)
            {
                // 在可放置区域且未被占用，固定住 Building
               // currentBuilding.GetComponent<Building>().LockBuilding();
                Destroy(currentBuilding);
                currentBuilding = null; // 取消选择
            }
            else
            {
                // 在占用状态下或不在放置区域，取消选择并删除 Building
                //Destroy(currentBuilding);
                GridManager.Instance.PlaceBuilding(currentBuilding.GetComponent<Building>(),buildingGridPosition);
                currentBuilding = null; // 取消选择
            }
        }
        
        
    }
    
    private bool IsInDropArea(Vector3 position)
    {
        // 将鼠标屏幕坐标转换为本地坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiDropArea.GetComponent<RectTransform>(), 
            Camera.main.WorldToScreenPoint(position), 
            null, // 使用 null 来表示没有特殊的事件系统
            out localPoint);

        // 获取 uiDropArea 的 RectTransform
        RectTransform dropAreaRect = uiDropArea.GetComponent<RectTransform>();

        // 检查鼠标坐标是否在 uiDropArea 的范围内
        return dropAreaRect.rect.Contains(localPoint);
    }
}
