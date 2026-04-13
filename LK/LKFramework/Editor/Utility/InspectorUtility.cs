using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LucienKlein
{

    public class InspectorEditorTool
    {
        [MenuItem("LKFramework/Inspector/全部收起组件... %#&n",false,29)]
        static void Shrinkage()
        {
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = EditorWindow.GetWindow(type);
            FieldInfo info = type.GetField("m_Tracker", BindingFlags.NonPublic | BindingFlags.Instance);
            ActiveEditorTracker tracker = info.GetValue(window) as ActiveEditorTracker;

            for (int i = 0; i < tracker.activeEditors.Length; i++)
            {
                //这里1就是展开，0就是合起来
                tracker.SetVisible(i, 0);
            }
        }
    }

}
