using UnityEngine;

public static class AssetBundleLoader {
    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。  
    public static readonly string m_PathURL =
#if UNITY_ANDROID
    "jar:file://" + Application.dataPath + "!/assets/";  
#elif UNITY_IPHONE
    Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
    "file://" + Application.dataPath + "/AssetBundleLearn/StreamingAssets/";
#else
    string.Empty;  
#endif

}