using UnityEngine;

public static class EditorPathConfig {
    public static readonly string AssetBundleBuildPath = string.Format("Assets/.AssetBundle/{0}/", EditorConst.PlatformName());
    public static readonly string AssetPath = "Assets/Editor/Asset/";
    public static readonly string AssetFullPath = string.Format("{0}/Editor/Asset/", Application.dataPath);
    public static readonly string AssetRemovePath = "/Editor/Asset";
    public static readonly string PrefabPath = string.Format("{0}Prefabs", AssetPath);
    public static readonly string AtlasPath = string.Format("{0}Atlas", AssetPath);
    public static readonly string ResourcesPath = "Assets/Resources/";
}