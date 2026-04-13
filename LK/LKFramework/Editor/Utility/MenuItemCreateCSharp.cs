using System.IO;
using UnityEditor;
using UnityEngine;

namespace LucienKlein
{

    public class MenuItemCreateCSharp
    {
        //****************************************************************************************************/
        static string TEMPLATE_FOLDER_PATH = "Assets/Plugins/LK/LKFramework/Editor/Templates/";

        [MenuItem("Assets/Create/Custom C# MonobehaviourScript", false, -1000)]
        private static void CreateCustomScript()
        {
            string templateName = "NewMonoBehaviourScript.txt";
            CreateCSharpScript(templateName);
        }

        [MenuItem("Assets/Create/Custom C# SimpleScript", false, -999)]
        private static void CreateCustomSimpleScript()
        {
            string templateName = "SimpleScript.txt";
            CreateCSharpScript(templateName);
        }

        [MenuItem("Assets/Create/Custom C# ScriptableObject", false, -998)]
        private static void CreateCustomScriptableObjectScript()
        {
            string templateName = "ScriptableObjectScript.txt";
            CreateCSharpScript(templateName);
        }

        static void CreateCSharpScript(string templateName)
        {
            string templatePath = TEMPLATE_FOLDER_PATH+templateName;
            string selectedPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            bool isFolder = ProjectWindowUtil.IsFolder(Selection.activeInstanceID);
            string targetFolderPath;

            if (isFolder)
            {
                targetFolderPath = selectedPath;
            }
            else
            {
                targetFolderPath = Path.GetDirectoryName(selectedPath);
            }
            string newScriptPath = Path.Combine(targetFolderPath, "NewScript.cs");
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, newScriptPath);
        }
    }
}
