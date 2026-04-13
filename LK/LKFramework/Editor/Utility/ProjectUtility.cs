using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LucienKlein
{

    public class ProjectEditorTool
    {
        //------------------------------------------------------------------------------------------------------------------------------------------------




        //----------------------------------------------------------------------------------------------------Create common directories

        [MenuItem("LKFramework/Utility/CreateSomeDirectory")]
        static void CreateSomeDirectory()
        {
            List<string> paths = new List<string>();
            paths.Add("/Resources");
            paths.Add("/Scripts");
            paths.Add("/StreamingAssets");
            for (int i = 0; i < paths.Count; i++)
            {
                if (!File.Exists(Application.dataPath + paths[i]))
                    Directory.CreateDirectory(Application.dataPath + paths[i]);
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("LKFramework/Project/折叠 Assets 下所有文件夹 &c", false, 30)]
        public static void CollapseSubFoldersUnderAssets()
        {
            // 获取 Editor 程序集
            var assembly = Assembly.GetAssembly(typeof(Editor));
            var projectBrowserType = assembly.GetType("UnityEditor.ProjectBrowser");

            if (projectBrowserType == null)
                return;

            // 获取最后交互的 Project 窗口
            var browserField = projectBrowserType.GetField("s_LastInteractedProjectBrowser", BindingFlags.Public | BindingFlags.Static);
            var browser = browserField?.GetValue(null);
            if (browser == null)
                return;

            // 判断当前 Project 窗口模式（0 = 单列，1 = 双列）
            var modeField = projectBrowserType.GetField("m_ViewMode", BindingFlags.NonPublic | BindingFlags.Instance);
            bool isOneColumn = (int)modeField.GetValue(browser) == 0;

            // 获取对应树结构字段
            var treeField = projectBrowserType.GetField(isOneColumn ? "m_AssetTree" : "m_FolderTree", BindingFlags.NonPublic | BindingFlags.Instance);
            var tree = treeField.GetValue(browser);
            if (tree == null)
                return;

            // 获取树数据
            var dataProperty = treeField.FieldType.GetProperty("data", BindingFlags.Instance | BindingFlags.Public);
            var data = dataProperty.GetValue(tree, null);
            if (data == null)
                return;

            // 获取行与展开方法
            var getRowsMethod = dataProperty.PropertyType.GetMethod("GetRows", BindingFlags.Instance | BindingFlags.Public);
            var setExpandedMethods = dataProperty.PropertyType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == "SetExpanded")
                .ToList();
            var setExpandedMethod = setExpandedMethods.FirstOrDefault();
            if (setExpandedMethod == null)
                return;

            // 获取所有行
            var rows = (IEnumerable)getRowsMethod.Invoke(data, null);

            bool assetsFound = false;
            foreach (var obj in rows)
            {
                var itemType = obj.GetType();
                var nameField = itemType.GetField("m_DisplayName", BindingFlags.Instance | BindingFlags.NonPublic);
                if (nameField == null)
                    continue;

                string name = (string)nameField.GetValue(obj);

                // 先找到 “Assets” 文件夹
                if (!assetsFound && name == "Assets")
                {
                    // 先展开 Assets 自身
                    setExpandedMethod.Invoke(data, new object[] { obj, true });
                    assetsFound = true;
                    continue;
                }

                // 之后的所有行都是 Assets 下的子项，全部折叠
                if (assetsFound)
                {
                    setExpandedMethod.Invoke(data, new object[] { obj, false });
                }
            }

            // 无需 AssetDatabase.Refresh(); 折叠只是 UI 状态
        }


        //------------------------------------------------------------------------------------------------------------------------/

        [MenuItem("LKFramework/Utility/GC")]
        static void GCCollect()
        {
            if (Application.isPlaying)
            {
                System.GC.Collect();
                Resources.UnloadUnusedAssets();
            }
            else
            {
                Debug.Log("无效");

            }
        }


    }

}
