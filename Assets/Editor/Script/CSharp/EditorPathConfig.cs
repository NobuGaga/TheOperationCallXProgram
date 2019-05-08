using UnityEngine;

public static class EditorPathConfig {
    public static readonly string AssetBundleBuildPath = string.Format("Assets/.AssetBundle/{0}/", EditorConst.PlatformName());
    public static readonly string AssetPath = string.Format("{0}/Editor/Asset/", Application.dataPath);
    public static readonly string MaterialPath = string.Format("{0}Material", AssetPath);
}