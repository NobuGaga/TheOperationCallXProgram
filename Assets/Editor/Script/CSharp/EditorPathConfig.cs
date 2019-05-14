using UnityEngine;
using System.IO;

public static class EditorPathConfig {
    public static readonly string AssetBundleBuildPath = Path.Combine("Assets/.AssetBundle", EditorConst.PlatformName());
    public static readonly string AssetPath = "Assets/Editor/Asset/";
    public static readonly string AssetFullPath = Path.Combine(Application.dataPath, "Editor/Asset/");
    public static readonly string AssetRemovePath = "/Editor/Asset";
    public static readonly string PrefabPath = Path.Combine(AssetPath, "Prefabs");
    public static readonly string AtlasPath = Path.Combine(AssetPath, "Atlas");
    public static readonly string ResourcesPath = "Assets/Resources/";
}