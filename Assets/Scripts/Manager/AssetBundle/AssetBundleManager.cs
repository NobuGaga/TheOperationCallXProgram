using UnityEngine;
using System.Collections.Generic;

public static class AssetBundleManager {
    private static Dictionary<string, AssetBundle> m_dicAssetBundle;
    private static Dictionary<string, AssetBundle> m_dicDelAssetBundle;
    private static Dictionary<string, Dictionary<string, Object>> m_dicAssetBundleObject;
    private static Dictionary<string, bool> m_dicCacheAssetBundleName;
    private static Dictionary<string, string> m_dicCacheObjectName;

    public static void Init(System.Action callback) {
        InitCache();
        Load(PathConfig.AssetBundlePlatformPath, (AssetBundle assetBundle) => callback());
    }

    #region InitCache
    private static void InitCache() {
        m_dicAssetBundle = new Dictionary<string, AssetBundle>();
        m_dicDelAssetBundle = new Dictionary<string, AssetBundle>();
        m_dicCacheAssetBundleName = new Dictionary<string, bool>();
        m_dicAssetBundleObject = new Dictionary<string, Dictionary<string, Object>>();
        m_dicCacheObjectName = new Dictionary<string, string>();

        m_dicCacheAssetBundleName.Add(PathConfig.AssetBundlePlatformPath, true);
        GameViewInfo viewInfo = ViewManager.GetViewInfo(GameMoudle.Loading, GameView.MainView);
        m_dicCacheObjectName.Add(viewInfo.AssetBundleName, viewInfo.Name);
        viewInfo = ViewManager.GetViewInfo(GameMoudle.Select, GameView.MainView);
        m_dicCacheObjectName.Add(viewInfo.AssetBundleName, viewInfo.Name);
    }

    private static bool IsCacheAssetBundle(string path) {
        return m_dicCacheAssetBundleName.ContainsKey(path);
    }

    private static bool IsCacheGameObject(string path, string objName) {
        return (m_dicCacheObjectName.ContainsKey(path) && m_dicCacheObjectName[path] == objName) || objName.Contains("Item");
    }
    #endregion

    public static void Load(string path, System.Action<AssetBundle> callback) {
        if (m_dicAssetBundle.ContainsKey(path))
            callback(m_dicAssetBundle[path]);
        else if (m_dicDelAssetBundle.ContainsKey(path))
            callback(m_dicDelAssetBundle[path]);
        else
            AssetBundleLoader.Load(GameManager.LogicScript, path, 
                (AssetBundle assetBundle) => {
                    if (IsCacheAssetBundle(path))
                        m_dicAssetBundle.Add(path, assetBundle);
                    else
                        m_dicDelAssetBundle.Add(path, assetBundle);
                    callback(assetBundle);
                });
    }

    public static void Load<T>(string path, string objName, System.Action<T> callback) where T:Object {
        if (m_dicAssetBundleObject.ContainsKey(path) && m_dicAssetBundleObject[path].ContainsKey(objName)) {
            callback(m_dicAssetBundleObject[path][objName] as T);
            return;
        }
        Load(path, (AssetBundle assetBundle) => 
            AssetBundleLoader.Load(GameManager.LogicScript, assetBundle, objName,
                (T obj) => {
                    callback(obj);
                    if (IsCacheGameObject(path, objName)) {
                        if (!m_dicAssetBundleObject.ContainsKey(path))
                            m_dicAssetBundleObject.Add(path, new Dictionary<string, Object>());
                        if (m_dicAssetBundleObject[path].ContainsKey(objName))
                            m_dicAssetBundleObject[path][objName] = obj as Object;
                        else
                            m_dicAssetBundleObject[path].Add(objName, obj as Object);
                    } else
                        GameObject.DestroyImmediate(obj, true);
                }));
    }

    public static void Clean() {
        var enumerator = m_dicDelAssetBundle.GetEnumerator();
        while (enumerator.MoveNext()) {
            var keyValue = enumerator.Current;
            keyValue.Value.Unload(!m_dicCacheObjectName.ContainsKey(keyValue.Key));
        }
        m_dicDelAssetBundle.Clear();
    }
}