using UnityEngine;

public static class PathConfig {
    public static readonly string AssetBundlePath =
#if UNITY_ANDROID
    string.Format("{0}{1}", Application.streamingAssetsPath, "/assets/");
#elif UNITY_IPHONE
    string.Format("{0}{1}", Application.streamingAssetsPath, "/assets/");
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
    string.Format("{0}{1}", Application.dataPath, "/../AssetBundle/Windows/assets/");
#else
    string.Empty;  
#endif
}