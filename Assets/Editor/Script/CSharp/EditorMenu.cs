using UnityEditor;

public static class EditorMenu {
    [MenuItem("Custom/AssetBundle/Pack")]
    private static void PackAssetBundle() {
        AssetBundlePacker.Pack();
    }

    [MenuItem("Custom/Tool/CheckRenderMaterial %M")]
    private static void CheckRenderMaterial() {
        ComponentChecker.CheckRenderMaterial();
    }

    [MenuItem("Custom/Tool/CheckMissingComponent %L")]
    private static void CheckMissingComponent() {
        ComponentChecker.CheckMissingComponent();
    }
}