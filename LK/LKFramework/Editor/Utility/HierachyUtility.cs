using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LucienKlein
{

    public class HierachyUtility
    {
        [MenuItem("GameObject/Divider ---------", false, 0)]
        public static void CreateDivider()
        {
            GameObject divider = new GameObject("---------");

            if (Selection.activeGameObject != null)
            {
                GameObject selected = Selection.activeGameObject;
                Transform parent = selected.transform.parent;

                divider.transform.SetParent(parent);
                divider.transform.SetSiblingIndex(selected.transform.GetSiblingIndex() + 1);
            }

            Selection.activeGameObject = divider;
            Undo.RegisterCreatedObjectUndo(divider, "Create Divider");
        }

        //展开Hierarchy中所有物体(递归式)
        [MenuItem("LKFramework/Hierarchy/ExpandAll",false,28)]
        public static void ExpandAll()
        {
            //获取hierarchy所在窗口
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
            EditorWindow window = EditorWindow.focusedWindow;
            //获取指定程序集以反射获取hierarchy
            object sceneHierarchy = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow").GetProperty("sceneHierarchy").GetValue(window);

            MethodInfo method = sceneHierarchy.GetType().GetMethod("SetExpandedRecursive", BindingFlags.Public | BindingFlags.Instance);
            //method.Invoke(sceneHierarchy, new object[] { });

            GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < objs.Length; i++)
            {
                ExpandRecursive(sceneHierarchy, method, objs[i]);
            }
        }





        //获取场景所有物体(会通过递归获取子物体)
        static List<GameObject> GetSceneGameObjects()
        {
            GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
            List<GameObject> allObjs = new List<GameObject>();
            for (int i = 0; i < objs.Length; i++)
            {
                GetSceneGameObjectsRecursive(allObjs, objs[i]);
            }
            return allObjs;

            //GetSceneGameObjects()专用递归
            static void GetSceneGameObjectsRecursive(List<GameObject> allObjs, GameObject obj)
            {
                allObjs.Add(obj);
                //print(allObjs[allObjs.Count - 1].name);
                for (int j = 0; j < obj.transform.childCount; j++)
                {
                    GetSceneGameObjectsRecursive(allObjs, obj.transform.GetChild(j).gameObject);
                }
            }

        }
        //ExpandAll专用
        static void ExpandRecursive(object sceneHierarchy, MethodInfo method, GameObject obj)
        {
            method.Invoke(sceneHierarchy, new object[] { obj.GetInstanceID(), true });
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                ExpandRecursive(sceneHierarchy, method, obj.transform.GetChild(i).gameObject);
            }
        }











    }

}
