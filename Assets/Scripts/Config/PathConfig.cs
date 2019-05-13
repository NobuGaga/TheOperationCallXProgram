using UnityEngine;
using System.IO;

public static class PathConfig {
    public static readonly string AssetBundlePath =
#if UNITY_EDITOR
    string.Format("{0}{1}", Application.dataPath, "/.AssetBundle/Windows/assets/");
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN
    string.Format("{0}{1}", Application.streamingAssetsPath, "/assets/");
#else
    string.Empty;
#endif
    /// <summary>
    /// 本地缓存文件路径
    /// </summary>
    public static string PersistentDataPath => m_persistentDataPath;

    private static readonly string m_persistentDataPath;
    // 第一次访问成员或者静态方法时调用静态构造
    static PathConfig() {
        m_persistentDataPath = Application.persistentDataPath.Replace("\\", "/");
#if UNITY_IPHONE
        DebugTool.Log(string.Format("ios m_persistentDataPath {0}", m_persistentDataPath));
        if (!m_persistentDataPath.EndsWith("/Documents"))
            m_PersistentDataPath = string.Format("{0}/Documents", m_PersistentDataPath);
#endif
    }

    public static readonly string AssetBundleMoudlePath = Path.Combine(AssetBundlePath, "prefabs/moudle/");
}