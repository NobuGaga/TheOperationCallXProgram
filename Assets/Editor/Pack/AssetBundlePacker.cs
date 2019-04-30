using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class AssetBundlePacker{
    private static string PlatformName() {
        switch (EditorUserBuildSettings.activeBuildTarget) {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            default:
                return "UnusePlatform";
        }
    }

    public static void Pack() {
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        // 打包目录不存在则打包失败
        if (!Directory.Exists(EditorPathConfig.AssetBundleBuildPath))
            Directory.CreateDirectory(EditorPathConfig.AssetBundleBuildPath);

        List<AssetBundleBuild> packBundles = new List<AssetBundleBuild>();

        // 不用文件流读取, 因为文件流读取的路径在最终文件上一级会出现 \ 反斜杠, 才用读取 GUID 保证文件唯一性
        string[] assetGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[]{ "Assets/Prefabs" });
        if (assetGUIDs != null && assetGUIDs.Length > 0)
            foreach (string guid in assetGUIDs) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string extension = Path.GetExtension(assetPath);
                string assetBundleName = assetPath.Replace(extension, "");
                Debug.Log(assetPath.Replace(extension, ""));
                AssetBundleBuild bundleData = new AssetBundleBuild();
                bundleData.assetBundleName = assetBundleName;
                bundleData.assetNames = new string[] { assetPath };
                packBundles.Add(bundleData);
            }

        BuildPipeline.BuildAssetBundles(EditorPathConfig.AssetBundleBuildPath, packBundles.ToArray(),
            BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
    }
}
