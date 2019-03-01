using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        string packToPath = string.Format("AssetBundle/{0}/", PlatformName());
        Debug.Log(string.Format("AssetBundle Pack Path {0}", packToPath));
        // 打包目录不存在则打包失败
        if (!Directory.Exists(packToPath))
            Directory.CreateDirectory(packToPath);

        List<AssetBundleBuild> bundles = new List<AssetBundleBuild>();
        // 不会返回子目录文件
        string[] prefabPath = Directory.GetFiles("Assets/Prefabs");
        foreach(string path in prefabPath)
            if (path.EndsWith(".prefab")) {
                //Assets / Prefabs\GreenCube.prefab
                //string[] temp = Path.GetFileNameWithoutExtension(path).Split('/');
                //AssetBundleBuild assetData = new AssetBundleBuild();
                //assetData.assetBundleName = 
            }

        string[] assetGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[]{ "Assets/Prefabs" });
        if (assetGUIDs != null && assetGUIDs.Length > 0)
        {
            foreach (string guid in assetGUIDs)
            {
                Debug.Log(guid);
                Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
                //assets.Add(AssetDatabase.GUIDToAssetPath(guid));
            }
        }
        //Path.Combine(Application.streamingAssetsPath, "myassetBundle")

        //BuildPipeline.BuildAssetBundles(packToPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        //BuildPipeline.BuildAssetBundles(packToPath, bundles.ToArray(),
        //    BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
    }
}
