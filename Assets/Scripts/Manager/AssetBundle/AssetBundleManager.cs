using UnityEngine;

public static class AssetBundleManager {

    public static void Load(string path, string objName, System.Type type, System.Action<Object> callback) {
        AssetBundleLoader.Load(GameManager.LogicScript, path, objName, type, callback);
    }

    public static void Load<T>(string path, string objName, System.Action<T> callback) where T : Object {
        AssetBundleLoader.Load(GameManager.LogicScript, path, objName, callback);
    }
}