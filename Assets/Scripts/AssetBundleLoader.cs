using UnityEngine;
using System.IO;
using System.Collections;

public static class AssetBundleLoader {
    public static void Load(MonoBehaviour behaviour, string path, string objName, System.Type type, System.Action<Object> callback) {
        behaviour.StartCoroutine(Load(path, objName, type, callback));
    }

    public static void Load<T>(MonoBehaviour behaviour, string path, string objName, System.Action<T> callback) where T : Object {
        behaviour.StartCoroutine(Load(path, objName, callback));
    }

    private static IEnumerator Load(string path, string objName, System.Type type, System.Action<Object> callback) {
        string fullPath = Path.Combine(PathConfig.AssetBundlePath, path);
        AssetBundle assetbundle = AssetBundle.LoadFromFile(fullPath, 0, 0);
        yield return null;
        if (assetbundle == null) {
            Debug.LogError(string.Format("assetbundle load error, path {0}", fullPath));
            yield break;
        }
        Object obj = assetbundle.LoadAsset(objName, type);
        assetbundle.Unload(true);
        if (obj == null) {
            Debug.LogError(string.Format("assetbundle load asset error, object name {0}", objName));
            yield break;
        }
        callback(obj);
    }

    private static IEnumerator Load<T>(string path, string objName, System.Action<T> callback) where T : Object {
        string fullPath = Path.Combine(PathConfig.AssetBundlePath, path);
        AssetBundle assetbundle = AssetBundle.LoadFromFile(fullPath, 0, 0);
        yield return null;
        if (assetbundle == null) {
            Debug.LogError(string.Format("assetbundle load error, path {0}", fullPath));
            yield break;
        }
        Object obj = assetbundle.LoadAsset<T>(objName);
        assetbundle.Unload(false);
        if (obj == null) {
            Debug.LogError(string.Format("assetbundle load asset error, object name {0}", objName));
            yield break;
        }
        callback(obj as T);
    }
}