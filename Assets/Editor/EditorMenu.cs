using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public static class EditorMenu {
    [MenuItem("Tools/导出 Lua 界面中英文名对照表", false)]
    private static void ExportAllViewName() {
        string windowRegistPath = "Assets/Editor/LuaScript/Library/Window/WindowRegist.lua";
        if (!File.Exists(windowRegistPath)) {
            Debug.LogError(string.Format("不存在路径\t{0}", windowRegistPath));
            return;
        }
        string exportPath = EditorUtility.OpenFolderPanel("导出目录", "", "");
        if (string.Empty == exportPath || null == exportPath)
            return;
        exportPath = string.Format(@"{0}/界面中英文名对照.csv", exportPath);
        FileStream file = new FileStream(exportPath, FileMode.Create);
        StreamWriter fileWriter = new StreamWriter(file);
        string windowRegistText = File.ReadAllText(windowRegistPath);
        fileWriter.Write(FilterWindowRegist(windowRegistText));
        fileWriter.Close();
        fileWriter.Dispose();
        if (File.Exists(exportPath))
            Debug.Log("导出成功");
    }

    private static string FilterWindowRegist(string windowRegistText) {
        Regex filterNoUse = new Regex("--[^-]+\n_M\\..*\\b");
        Match match = filterNoUse.Match(windowRegistText);
        StringBuilder stringBuilder = new StringBuilder();
        while (match.Success) {
            string originLine = match.ToString();
            string[] nameToView = originLine.Split('\n');
            string csvLine = null;
            if (nameToView.Length == 2)
                csvLine = string.Format("{0},{1}\n", nameToView[1], nameToView[0]);
            else if (nameToView.Length < 2)
                csvLine = string.Format("{0}\n", originLine);
            else {
                stringBuilder.Append(string.Format("{0},{1}\n", nameToView[1], nameToView[0]));
                for (int index = 3; index < nameToView.Length; index++)
                    stringBuilder.Append(nameToView[index]);
                stringBuilder.Append("\n");
            }
            match = match.NextMatch();
            if (csvLine == null)
                continue;
            stringBuilder.Append(csvLine);
        }
        string filtedText = stringBuilder.ToString();
        //过滤未加注释界面花括号内容
        filtedText = Regex.Replace(filtedText, "\\s*=\\s*{[^}]*?}", string.Empty);
        filtedText = Regex.Replace(filtedText, "--\\b*?", "--");
        filtedText = filtedText.Replace("_M.", string.Empty);
        filtedText = filtedText.Replace("--", string.Empty);
        return filtedText;
    }
}
