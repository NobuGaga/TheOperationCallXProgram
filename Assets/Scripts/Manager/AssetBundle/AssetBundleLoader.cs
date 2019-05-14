using UnityEngine;
using System.IO;
using System.Collections;

public static class AssetBundleLoader {
    public static void Load(MonoBehaviour behaviour, string path, System.Action<AssetBundle> callback) {
        behaviour.StartCoroutine(Load(path, callback));
    }

    public static void Load<T>(MonoBehaviour behaviour, string path, string objName, System.Action<T> callback) where T:Object {
        behaviour.StartCoroutine(Load(path, 
            (AssetBundle assetbundle) => {
                behaviour.StartCoroutine(Load(assetbundle, objName, callback));
            })
        );
    }

    public static void Load<T>(MonoBehaviour behaviour, AssetBundle assetbundle, string objName, System.Action<T> callback) where T:Object {
        behaviour.StartCoroutine(Load(assetbundle, objName, callback));
    }

    private static IEnumerator Load(string path, System.Action<AssetBundle> callback) {
        string fullPath = Path.Combine(PathConfig.AssetBundlePath, path);
        AssetBundle assetbundle = AssetBundle.LoadFromFile(fullPath, 0, 0);
        yield return null;
        if (assetbundle == null) {
            DebugTool.LogError(string.Format("assetbundle load error, path {0}", fullPath));
            yield break;
        }
        callback(assetbundle);
    }

    private static IEnumerator Load<T>(AssetBundle assetbundle, string objName, System.Action<T> callback) where T:Object {
        Object obj = assetbundle.LoadAsset<T>(objName);
        if (obj == null) {
            DebugTool.LogError(string.Format("assetbundle load asset error, object name {0}", objName));
            yield break;
        }
        callback(obj as T);
    }
}