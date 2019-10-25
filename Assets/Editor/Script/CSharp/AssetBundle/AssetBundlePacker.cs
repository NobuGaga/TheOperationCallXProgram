using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public static class AssetBundlePacker {
    public static void Pack() {
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        // 打包目录不存在则打包失败
        if (!Directory.Exists(EditorPathConfig.AssetBundleBuildPath))
            Directory.CreateDirectory(EditorPathConfig.AssetBundleBuildPath);

        List<AssetBundleBuild> packBundles = new List<AssetBundleBuild>();
        PackAtlas(packBundles);
        PackAllPrefab(packBundles);
        BuildPipeline.BuildAssetBundles(EditorPathConfig.AssetBundleBuildPath, packBundles.ToArray(),
            BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
    }

    private static void PackAtlas(List<AssetBundleBuild> packBundles) {
        // 不用文件流读取, 因为文件流读取的路径在最终文件上一级会出现 \ 反斜杠, 采用读取 GUID 保证文件唯一性
        string[] assetGUIDs = AssetDatabase.FindAssets("t:Sprite", new string[]{ EditorPathConfig.AtlasPath });
        if (assetGUIDs == null || assetGUIDs.Length <= 0)
            return;
        Dictionary<string, List<string>> dicBundle = new Dictionary<string, List<string>>();
        foreach (string guid in assetGUIDs) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string assetBundleName = assetPath.Replace(EditorPathConfig.AssetRemovePath, string.Empty);
            string[] splitStr = assetBundleName.Split('/');
            splitStr[splitStr.Length - 1] = null;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < splitStr.Length - 1; i++) {
                strBuilder.Append(splitStr[i]);
                strBuilder.Append('/');
            }
            strBuilder.Remove(strBuilder.Length - 2, 2);
            assetBundleName = strBuilder.ToString();
            if (!dicBundle.ContainsKey(assetBundleName))
                dicBundle.Add(assetBundleName, new List<string>());
            dicBundle[assetBundleName].Add(assetPath);
        }
        foreach (KeyValuePair<string, List<string>> bundleNameResList in dicBundle) {
            AssetBundleBuild bundleData = new AssetBundleBuild();
            bundleData.assetBundleName = bundleNameResList.Key;
            bundleData.assetNames = bundleNameResList.Value.ToArray();
            packBundles.Add(bundleData);
        }
    }

    private static void PackAllPrefab(List<AssetBundleBuild> packBundles) {
        // 不用文件流读取, 因为文件流读取的路径在最终文件上一级会出现 \ 反斜杠, 采用读取 GUID 保证文件唯一性
        string[] assetGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[]{ EditorPathConfig.PrefabPath });
        if (assetGUIDs == null || assetGUIDs.Length <= 0)
            return;
        Dictionary<string, List<string>> dicUIBundle = null;
        foreach (string guid in assetGUIDs) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string assetBundleName = assetPath.Replace(EditorPathConfig.AssetRemovePath, string.Empty);
            if (assetBundleName.Contains(EditorConst.MoudleString) && assetBundleName.Contains(EditorConst.UIString)) {
                int startIndex = assetBundleName.IndexOf(EditorConst.UIString);
                assetBundleName = assetBundleName.Remove(startIndex + EditorConst.UIStringLength);
                if (dicUIBundle == null)
                    dicUIBundle = new Dictionary<string, List<string>>();
                if (!dicUIBundle.ContainsKey(assetBundleName))
                    dicUIBundle.Add(assetBundleName, new List<string>());
                dicUIBundle[assetBundleName].Add(assetPath);
                continue;
            }
            assetBundleName = StringTool.RemoveExtension(assetBundleName);
            AssetBundleBuild bundleData = new AssetBundleBuild();
            bundleData.assetBundleName = assetBundleName;
            bundleData.assetNames = new string[] { assetPath };
            packBundles.Add(bundleData);
        }
        if (dicUIBundle == null)
            return;
        foreach (KeyValuePair<string, List<string>> bundleNameResList in dicUIBundle) {
            AssetBundleBuild bundleData = new AssetBundleBuild();
            bundleData.assetBundleName = bundleNameResList.Key;
            bundleData.assetNames = bundleNameResList.Value.ToArray();
            packBundles.Add(bundleData);
        }
    }
}