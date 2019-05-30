using UnityEngine;
using System.Collections.Generic;

public static class AssetBundleManager {
    private static Dictionary<string, AssetBundle> dicAssetBundle;
    private static Dictionary<string, AssetBundle> dicDelAssetBundle;
    private static Dictionary<string, Dictionary<string, Object>> dicAssetBundleObject;
    private static Dictionary<string, bool> dicCacheAssetBundleName;
    private static Dictionary<string, string> dicCacheObjectName;

    public static void Init(System.Action callback) {
        InitCache();
        Load(PathConfig.AssetBundlePlatformPath, (AssetBundle assetBundle) => UINumberManager.Init(callback));
    }

    #region InitCache
    private static void InitCache() {
        dicAssetBundle = new Dictionary<string, AssetBundle>();
        dicDelAssetBundle = new Dictionary<string, AssetBundle>();
        dicCacheAssetBundleName = new Dictionary<string, bool>();
        dicAssetBundleObject = new Dictionary<string, Dictionary<string, Object>>();
        dicCacheObjectName = new Dictionary<string, string>();

        dicCacheAssetBundleName.Add(PathConfig.AssetBundlePlatformPath, true);
        dicCacheAssetBundleName.Add(PathConfig.AssetBundleNumberAtlasPath, true);
        dicCacheAssetBundleName.Add(PathConfig.AssetBundleNumberPrefabPath, true);
        GameViewInfo viewInfo = ViewManager.GetViewInfo(GameMoudle.Loading, GameView.MainView);
        dicCacheObjectName.Add(viewInfo.AssetBundleName, viewInfo.Name);
        viewInfo = ViewManager.GetViewInfo(GameMoudle.Select, GameView.MainView);
        dicCacheObjectName.Add(viewInfo.AssetBundleName, viewInfo.Name);
        viewInfo = ViewManager.GetViewInfo(GameMoudle.VirtualButton, GameView.MainView);
        dicCacheObjectName.Add(viewInfo.AssetBundleName, viewInfo.Name);
    }

    private static bool IsCacheAssetBundle(string path) {
        return dicCacheAssetBundleName.ContainsKey(path);
    }

    private static bool IsCacheGameObject(string path, string objName) {
        return (dicCacheObjectName.ContainsKey(path) && dicCacheObjectName[path] == objName) || objName.Contains("Item");
    }
    #endregion

    public static void Load(string path, System.Action<AssetBundle> callback) {
        if (dicAssetBundle.ContainsKey(path))
            callback(dicAssetBundle[path]);
        else if (dicDelAssetBundle.ContainsKey(path))
            callback(dicDelAssetBundle[path]);
        else
            AssetBundleLoader.Load(GameManager.LogicScript, path, 
                (AssetBundle assetBundle) => {
                    if (IsCacheAssetBundle(path))
                        dicAssetBundle.Add(path, assetBundle);
                    else
                        dicDelAssetBundle.Add(path, assetBundle);
                    callback(assetBundle);
                });
    }

    public static void Load<T>(string path, string objName, System.Action<T> callback) where T:Object {
        if (dicAssetBundleObject.ContainsKey(path) && dicAssetBundleObject[path].ContainsKey(objName)) {
            callback(dicAssetBundleObject[path][objName] as T);
            return;
        }
        Load(path, (AssetBundle assetBundle) => 
            AssetBundleLoader.Load(GameManager.LogicScript, assetBundle, objName,
                (T obj) => {
                    callback(obj);
                    if (IsCacheGameObject(path, objName)) {
                        if (!dicAssetBundleObject.ContainsKey(path))
                            dicAssetBundleObject.Add(path, new Dictionary<string, Object>());
                        if (dicAssetBundleObject[path].ContainsKey(objName))
                            dicAssetBundleObject[path][objName] = obj as Object;
                        else
                            dicAssetBundleObject[path].Add(objName, obj as Object);
                    } else
                        GameObject.DestroyImmediate(obj, true);
                }));
    }

    public static void Clean() {
        var enumerator = dicDelAssetBundle.GetEnumerator();
        while (enumerator.MoveNext()) {
            var keyValue = enumerator.Current;
            keyValue.Value.Unload(!dicCacheObjectName.ContainsKey(keyValue.Key));
        }
        dicDelAssetBundle.Clear();
    }
}