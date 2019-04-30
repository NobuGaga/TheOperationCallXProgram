using UnityEditor;

public static class EditorMenu {
    [MenuItem("Custom/AssetBundle/Pack")]
    private static void PackAssetBundle() {
        AssetBundlePacker.Pack();
    }
}
