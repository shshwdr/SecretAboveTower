#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Building))]
public class GridSelectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Building gridManager = (Building)target;

        // 确保 selections 已初始化
        if (gridManager.selections == null || gridManager.selections.Count != gridManager.rows)
        {
            gridManager.Init(); // 初始化 selections
        }

        // 绘制网格
        for (int i = 0; i < gridManager.rows; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < gridManager.cols; j++)
            {
                // 创建复选框
                bool isSelected = gridManager.selections[i].selections[j] == 1;
                bool newSelection = EditorGUILayout.Toggle(isSelected, GUILayout.Width(20));

                // 更新选择状态
                gridManager.selections[i].selections[j] = newSelection ? 1 : 0;
            }
            EditorGUILayout.EndHorizontal();
        }

        // 保存更改
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gridManager); // 标记为脏以保存更改
        }

        // 绘制默认属性
        DrawDefaultInspector();
    }
    }

#endif