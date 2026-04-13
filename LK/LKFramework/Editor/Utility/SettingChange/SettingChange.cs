using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LucienKlein
{

    public class SettingChange
    {
        //更改宏定义为DEMO
        [MenuItem("LKFramework/Utility/SetDifineSymbol/SetScriptingDefineSymbols_Demo")]
        public static void SetVersionDemo()
        {
            foreach (var group in TargetGroups)
            {
                AddDefine(group, DemoDefine);
            }
            UnityEngine.Debug.Log("已启用 Demo 宏: " + DemoDefine);
        }

        //更改宏定义为Release
        [MenuItem("LKFramework/Utility/SetDifineSymbol/SetScriptingDefineSymbols_Release")]
        public static void SetVersionRelease()
        {
            foreach (var group in TargetGroups)
            {
                RemoveDefine(group, DemoDefine);
            }
            UnityEngine.Debug.Log("已切换到 Release 版本");
        }











        private const string DemoDefine = "DEMO";

        // 你要支持的目标平台
        private static readonly BuildTargetGroup[] TargetGroups = new[]
        {
        BuildTargetGroup.Standalone,
        BuildTargetGroup.WebGL,
        BuildTargetGroup.Android
        // 需要的话还可以加 iOS、PS4 等
    };


        private static void AddDefine(BuildTargetGroup group, string define)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group)
                .Split(';')
                .Where(d => !string.IsNullOrEmpty(d))
                .ToList();

            if (!defines.Contains(define))
            {
                defines.Add(define);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines));
            }
        }

        private static void RemoveDefine(BuildTargetGroup group, string define)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group)
                .Split(';')
                .Where(d => d != define && !string.IsNullOrEmpty(d))
                .ToList();

            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines));
        }




    }

}
