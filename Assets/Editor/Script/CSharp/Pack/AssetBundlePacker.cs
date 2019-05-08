using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class AssetBundlePacker {
    public static void Pack() {
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        // 打包目录不存在则打包失败
        if (!Directory.Exists(EditorPathConfig.AssetBundleBuildPath))
            Directory.CreateDirectory(EditorPathConfig.AssetBundleBuildPath);

        List<AssetBundleBuild> packBundles = new List<AssetBundleBuild>();
        PackMaterial(packBundles);
        PackPrefab(packBundles);
        BuildPipeline.BuildAssetBundles(EditorPathConfig.AssetBundleBuildPath, packBundles.ToArray(),
            BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
    }
    
    private static void PackMaterial(List<AssetBundleBuild> packBundles) {
        string[] assetGUIDs = AssetDatabase.FindAssets("t:Mat", new string[]{ EditorPathConfig.MaterialPath });
        if (assetGUIDs == null || assetGUIDs.Length <= 0)
            return;
        AssetBundleBuild bundleData = new AssetBundleBuild();
        bundleData.assetBundleName = EditorConst.AssetBundleMaterialName;
        List<string> assetNames = new List<string>();
        foreach (string guid in assetGUIDs) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string extension = Path.GetExtension(assetPath);
            string assetBundleName = assetPath.Replace(extension, "");
            Debug.Log(assetPath.Replace(extension, ""));
            assetNames.Add(assetPath);
        }
        bundleData.assetNames = assetNames.ToArray();
        packBundles.Add(bundleData);
    }

    private static void PackPrefab(List<AssetBundleBuild> packBundles) {
        // 不用文件流读取, 因为文件流读取的路径在最终文件上一级会出现 \ 反斜杠, 采用读取 GUID 保证文件唯一性
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
    }
}