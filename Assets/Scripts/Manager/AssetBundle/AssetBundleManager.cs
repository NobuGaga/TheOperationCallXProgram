using UnityEngine;
using System.Collections.Generic;

public static class AssetBundleManager {
    private static Dictionary<string, AssetBundle> m_dicAssetBundle = new Dictionary<string, AssetBundle>();

    public static void Init(System.Action callback) {
        Load(PathConfig.AssetBundlePlatformPath,
            delegate (AssetBundle assetBundle) {
                m_dicAssetBundle.Add(PathConfig.AssetBundlePlatformPath, assetBundle);
                callback();
            }
        );
    }

    public static void Load(string path, System.Action<AssetBundle> callback) {
        if (m_dicAssetBundle.ContainsKey )
        AssetBundleLoader.Load(GameManager.LogicScript, path, callback);
    }

    public static void Load<T>(string path, string objName, System.Action<T> callback) where T:Object {
        AssetBundleLoader.Load(GameManager.LogicScript, path, objName, callback);
    }
}