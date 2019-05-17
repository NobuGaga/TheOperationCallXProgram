using UnityEngine;
using System.IO;

public static class PathConfig {
    public static readonly string AssetBundlePath =
#if UNITY_EDITOR
    Path.Combine(Application.dataPath, ".AssetBundle/Windows/assets/");
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN
    Path.Combine(Application.streamingAssetsPath, "assets/");
#else
    string.Empty;
#endif

    public static readonly string AssetBundlePlatformPath =
#if UNITY_EDITOR
    Path.Combine(Application.dataPath, ".AssetBundle/Windows", GameConst.PlatformName);
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN
    Path.Combine(Application.streamingAssetsPath, GameConst.PlatformName);
#else
    string.Empty;
#endif

    public static readonly string AssetBundleMoudlePath = Path.Combine(AssetBundlePath, "prefabs/moudle/");

    /// <summary>
    /// 本地缓存文件路径
    /// </summary>
    public static string PersistentDataPath => persistentDataPath;
    private static readonly string persistentDataPath;

    // 第一次访问成员或者静态方法时调用静态构造
    static PathConfig() {
        persistentDataPath = Application.persistentDataPath.Replace("\\", "/");
#if UNITY_IPHONE
        DebugTool.Log(string.Format("ios persistentDataPath {0}", persistentDataPath));
        if (!persistentDataPath.EndsWith("Documents"))
            persistentDataPath = Path.Combine(persistentDataPath, "Documents");
#endif
    }
}