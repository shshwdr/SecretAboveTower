using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSensitivity = 2f; // 鼠标滚轮灵敏度
    public float minY = -10f; // 相机的最小 Y 位置
    public float maxY = 10f; // 相机的最大 Y 位置

    private void Update()
    {
        // 获取鼠标滚轮输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 如果有滚轮输入，调整相机的 Y 位置
        if (scrollInput != 0f)
        {
            // 计算新的 Y 位置
            float newY = transform.position.y + scrollInput * scrollSensitivity;

            // 使用 scrollInput 的绝对值来增加移动速度
            newY += scrollInput * scrollSensitivity * Mathf.Abs(scrollInput) * 5f; // 根据滚轮滚动的速度增加移动量

            // 限制 Y 位置在上下限之间
            newY = Mathf.Clamp(newY, minY, maxY);

            // 更新相机位置
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}