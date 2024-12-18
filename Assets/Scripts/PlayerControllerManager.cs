using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerManager : Singleton<PlayerControllerManager>
{
    private GameObject currentBuilding; // 当前正在拖动的 Building
    public Transform uiDropArea; // 取消UI 放置区域
    GameObject cell;
    public void StartDragging(GameObject building,GameObject cell)
    {
        currentBuilding = building;
        this.cell = cell;
    }

    public HoveredObject hoveredObject;
    public void Update()
    {
        // 检查当前是否有 Building 在拖动
        if (currentBuilding != null)
        {
            DragBuilding();
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 如果鼠标在 UI 上，不显示弹框
                HoverOverMenu.FindFirstInstance<HoverOverMenu>().Hide();
                hoveredObject = null;
                return;
            }
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 获取鼠标位置
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // 使用射线检测鼠标指向的物体
            
            
            if (hit.collider != null && hit.collider.GetComponent<ClickableObject>() ) // 检查是否碰到当前物体
            {
                if( Input.GetMouseButtonDown(0))
                {
                    hit.collider.GetComponent<ClickableObject>().Click();
                }
            }
            
            
            
            
            
            
            bool isMouseOverSprite;
            if (hit.collider != null && hit.collider.GetComponent<HoveredObject>() ) // 检查是否碰到当前物体
            {
                if( hit.collider.GetComponent<HoveredObject>() !=  hoveredObject)
                {
                    hoveredObject = hit.collider.GetComponent<HoveredObject>();

                    hoveredObject.Show();
                }
            }
            else
            {
                hoveredObject = null;
                HoverOverMenu.FindFirstInstance<HoverOverMenu>().Hide();
            }
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
        var offset = GridManager.Instance.GetBuildingOffset(currentBuilding);
        Vector3 snapPosition = GridManager.Instance.GridToWorldPosition(buildingGridPosition, currentBuilding);

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
        if (Input.GetMouseButtonDown(0)) // 左键放下
        {
            if (/*IsInDropArea(mousePosition) ||*/ !canPlace)
            {
               // // currentBuilding.GetComponent<Building>().LockBuilding();
               //  Destroy(currentBuilding);
               //  currentBuilding = null; // 取消选择
            }
            else
            {
                //Destroy(currentBuilding);
                GridManager.Instance.PlaceBuilding(currentBuilding.GetComponent<Building>(),buildingGridPosition);
                currentBuilding = null; // 取消选择
                Destroy(cell);
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
