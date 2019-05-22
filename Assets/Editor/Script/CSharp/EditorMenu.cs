using UnityEditor;

public static class EditorMenu {
    [MenuItem("Custom/AssetBundle/Pack")]
    private static void PackAssetBundle() {
        AssetBundlePacker.Pack();
    }

    [MenuItem("Custom/Tool/CheckMaterial %M")]
    private static void CheckMaterial() {
        MaterialChecker.CheckMeshRender();
    }
}