using UnityEngine;

public static class PathConfig {
    public static readonly string AssetBundlePath =
#if UNITY_EDITOR
    string.Format("{0}{1}", Application.dataPath, "/.AssetBundle/Windows/assets/");
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN
    string.Format("{0}{1}", Application.streamingAssetsPath, "/assets/");
#else
    string.Empty;
#endif
}